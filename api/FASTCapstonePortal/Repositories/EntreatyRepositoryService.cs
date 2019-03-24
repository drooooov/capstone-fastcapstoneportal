using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Repositories
{
    public class EntreatyRepositoryService : IEntreaty
    {
        protected readonly CapstoneDBContext _context;
        protected readonly IHubContext<SignalServer> _hubContext;

        public EntreatyRepositoryService(CapstoneDBContext context, IHubContext<SignalServer> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public async Task CreateInviteAsync(Entreaty invite, int userId)
        {
            invite.EntreatyType = EntreatyType.INVITE;
            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("{0} has sent an invite", invite.Group.Name),
                NotificationType = NotificationType.ENTREATY,
                Time = DateTime.UtcNow
            };
            foreach (Student s in invite.Group.Students)
            {
                notificationContext.NotificationsSent.Add(new Notification()
                {
                    NotificationContext = notificationContext,
                    Read = false,
                    Receiver = await _context.Users.FindAsync(s.Id)
                });
            }
            notificationContext.NotificationsSent.Add(new Notification()
            {
                NotificationContext = notificationContext,
                Read = false,
                Receiver = await _context.Users.FindAsync(invite.Student.Id)
            });
            invite.NotificationContext = notificationContext;
            await _context.AddAsync(invite);
            await SaveAsync();

            foreach(Notification n in notificationContext.NotificationsSent)
            {
                if (_hubContext.Clients.Group(n.ReceiverId.ToString()) != null)
                    await _hubContext.Clients.Group(n.ReceiverId.ToString()).SendAsync("ReceiveNotification");
            }
        }
   
        public async Task CreateRequestAsync(Entreaty request, int userId)
        {
            request.EntreatyType = EntreatyType.REQUEST;
            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("{0} has requested to join your group", request.Student.FirstName),
                NotificationType = NotificationType.ENTREATY,
                Time = DateTime.UtcNow
            };
            foreach (Student s in request.Group.Students)
            {
                notificationContext.NotificationsSent.Add(new Notification()
                {
                    NotificationContext = notificationContext,
                    Read = false,
                    Receiver = await _context.Users.FindAsync(s.Id)
                });
            }
            request.NotificationContext = notificationContext;
            await _context.AddAsync(request);
            await SaveAsync();

            foreach (Notification n in notificationContext.NotificationsSent)
            {
                if (_hubContext.Clients.Group(n.ReceiverId.ToString()) != null)
                    await _hubContext.Clients.Group(n.ReceiverId.ToString()).SendAsync("ReceiveNotification");
            }
        }

        public async Task RejectAsync(Entreaty entreaty)
        {
            Group group = entreaty.Group;
            Student student = entreaty.Student;
            Student groupAdmin = group.Students.Where(s => s.GroupAdmin == true).FirstOrDefault();
            EntreatyType type = entreaty.EntreatyType;
            _context.Remove(entreaty);
            await SaveAsync();
            NotificationContext notificationContext = new NotificationContext()
            {
                NotificationType = NotificationType.ENTREATY,
                Time = DateTime.UtcNow
            };
            foreach (Student s in group.Students)
            {
                notificationContext.NotificationsSent.Add(new Notification()
                {
                    NotificationContext = notificationContext,
                    Read = false,
                    Receiver = await _context.Users.FindAsync(s.Id)
                });
            }
            if (type == EntreatyType.REQUEST)
            {
                notificationContext.CreatedBy = _context.Users.Find(groupAdmin.Id);
                notificationContext.Data = string.Format("{0} has rejected request", group.Name);
                notificationContext.NotificationsSent.Add(new Notification()
                {
                    NotificationContext = notificationContext,
                    Read = false,
                    Receiver = student.User
                });
            }
            else
            {
                notificationContext.CreatedBy = _context.Users.Find(student.Id);
                notificationContext.Data = string.Format("{0} {1} has rejected invite", student.FirstName, student.LastName);
            }
            _context.Add(notificationContext);
            await SaveAsync();

            foreach (Notification n in notificationContext.NotificationsSent)
            {
                if (_hubContext.Clients.Group(n.ReceiverId.ToString()) != null)
                    await _hubContext.Clients.Group(n.ReceiverId.ToString()).SendAsync("ReceiveNotification");
            }
        }

        public async Task<bool> ExistsAsync(int studentId, int groupId)
        {
            return await _context.Entreaties.AnyAsync(e => e.StudentId == studentId && e.GroupId == groupId);
        }
    }
}
