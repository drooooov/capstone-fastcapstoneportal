using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using FASTCapstonePortal.RequestModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Repositories
{
    public class StudentRepositoryService : IStudent
    {
        protected readonly CapstoneDBContext _context;
        protected readonly IHubContext<SignalServer> _hubContext;


        public StudentRepositoryService(CapstoneDBContext context, IHubContext<SignalServer> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public async Task CreateAsync(Student student)
        {
            await _context.AddAsync(student);
            await SaveAsync();
        }

        public async Task DeleteAsync(Student student)
        {        
            _context.Remove(student);
            //Not included in cascade to avoid cycles
            _context.Remove(student.User);
            await SaveAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            await SaveAsync();
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Set<Student>().ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetByGroupAdminAsync(bool admin)
        {
            return await _context.Students.Where(s => s.GroupAdmin == admin).ToListAsync();
        }

        public async Task<Student> GetAdminByGroupAsync(int groupId)
        {
            return await _context.Students.Where(s => s.Group.Id == groupId && s.GroupAdmin == true).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Student>> GetByGroupAsync(int groupId)
        {
            return await _context.Students.Where(s => s.Group.Id == groupId).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetByProgramAsync(Programs program)
        {
            return await _context.Students.Where(s => s.Program == program).ToListAsync();
        }

        public async Task<bool> ExistsAsync(int studentId)
        {
            return await _context.Students.AnyAsync(e => e.Id == studentId);
        }

        public async Task<IEnumerable<Student>> GetByCampusAsync(Campuses campus)
        {
            return await _context.Students.Where(s => s.Campus == campus).ToListAsync();
        }

        public async Task<bool> PictureExistsAsync(string picture)
        {
            return await _context.Students.AnyAsync(s => s.Picture == picture);
        }

        public async Task<Student> GetByIdAsync(int studenId)
        {
            return await _context.Students.FindAsync(studenId);
        }

        public async Task AddStudentSkillsAsync(IEnumerable<string> skills, Student student)
        {
            student.Skills.Clear();
            Skill sk;
            foreach (string skill in skills)
            {
                sk = await _context.Skills.FirstOrDefaultAsync(s => s.Name.Equals(skill, StringComparison.InvariantCultureIgnoreCase));
                if (sk == null)
                {
                    student.Skills.Add(new StudentsSkills { Student = student, Skill = new Skill { Name = skill } });
                }
                else
                {
                    student.Skills.Add(new StudentsSkills { Student = student, Skill = sk });
                }
            }
        }

        public async Task UpdateStudentThroughRequestModelAsync(Student student, StudentUpdate studentUpdate)
        {
            student.FirstName = studentUpdate.FirstName;
            student.LastName = studentUpdate.LastName;
            student.Campus = studentUpdate.Campus;
            student.Description = studentUpdate.Description;
            student.LinkedInLink = studentUpdate.LinkedInLink;
            student.PortfolioLink = studentUpdate.PortfolioLink;
            student.Picture = studentUpdate.Picture;
            student.Role = studentUpdate.Role;
            await AddStudentSkillsAsync(studentUpdate.Skills, student);
            await UpdateAsync(student);
        }

        public async Task PutStudentInGroupAsync(Student student, Group group, int userId)
        {
            HashSet<int> studentsToBroadcastTo = student.Entreaties.SelectMany(e => e.Group.Students.Select(s => s.Id)).ToHashSet();
            student.Group = group;
            _context.RemoveRange(student.Entreaties);
            if (group.Students.Count >= 4)
            {
                foreach(Entreaty entreaty in group.Entreaties)
                    studentsToBroadcastTo.Add(entreaty.StudentId);
                _context.RemoveRange(group.Entreaties);
            };
            await UpdateAsync(student);

            foreach(int id in studentsToBroadcastTo)
            {
               await  _hubContext.Clients.Group(id.ToString()).SendAsync("UpdateEntreatyLists");
            }

            //Notify everyone in group
            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("{0} {1} has been added to {2}", student.FirstName, student.LastName, group.Name),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };

            await SendNotificationsToRangeOfStudentsAsync(notificationContext, _context.Groups.Find(group.Id).Students);
        }

        public async Task RemoveStudentFromGroupAsync(Student student, int groupId, int userId)
        {
            Group group = await _context.Groups.FindAsync(groupId);
            student.Group = null;
            await UpdateAsync(student);
            //Notify everyone in group
            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("{0} {1} has been removed from group {2}", student.FirstName, student.LastName, group.Name),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };

            await SendNotificationsToRangeOfStudentsAsync(notificationContext, (await _context.Groups.FindAsync(groupId)).Students);
        }

        public async Task ChangeGroupAdminAsync(Student oldAdmin, Student newAdmin, int userId)
        {
            newAdmin.GroupAdmin = true;
            oldAdmin.GroupAdmin = false;
            await UpdateAsync(oldAdmin);
            await UpdateAsync(newAdmin);

            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("{0}{1} is the new group admin", newAdmin.FirstName,newAdmin.LastName),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };
            await SendNotificationsToRangeOfStudentsAsync(notificationContext, newAdmin.Group.Students);
        }

        public async Task UploadStudentImageAsync(Student student, string imageName)
        {
            student.Picture = imageName;
            await UpdateAsync(student);
        }

        public async Task SendNotificationsToRangeOfStudentsAsync(NotificationContext notificationContext, ICollection<Student> students)
        {
            foreach (Student s in students)
            {
                notificationContext.NotificationsSent.Add(new Notification()
                {
                    Read = false,
                    Receiver = await _context.Users.FindAsync(s.Id),
                });
            }
            _context.Add(notificationContext);
            await SaveAsync();

            foreach (Notification n in notificationContext.NotificationsSent)
            {
                if (_hubContext.Clients.Group(n.ReceiverId.ToString()) != null)
                    await _hubContext.Clients.Group(n.ReceiverId.ToString()).SendAsync("ReceiveNotification");
            }
        }
    }
}
