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
    public class ProjectRepositoryService : IProject
    {
        protected readonly CapstoneDBContext _context;

        protected readonly IHubContext<SignalServer> _hubContext;

        public ProjectRepositoryService(CapstoneDBContext context, IHubContext<SignalServer> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public async Task CreateAsync(Project project)
        {
            await _context.AddAsync(project);
            await SaveAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            Group group = _context.Groups.Where(g => g.ProposedProject.Id == project.Id).First();
            group.ProposedProject = null;
            _context.Remove(project);
            await SaveAsync();
        }

        public async Task UpdateAsync(Project entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Set<Project>().ToListAsync();
        }


        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Projects.AnyAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>> GetApprovedProjectsAsync()
        {
            return await _context.Projects.Where(p => p.Approved == true && p.Proposed == true).ToListAsync();
        }


        public async Task<IEnumerable<Project>> GetByClientAsync(string clientName)
        {
            return await _context.Projects.Where(p => p.ClientName == clientName).ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetByDifficultyAsync(int difficulty)
        {
            return await _context.Projects.Where(p => p.Difficulty == difficulty).ToListAsync();
        }

        public async Task<Project> GetByIdAsync(int projectId)
        {
            return await _context.Projects.FindAsync(projectId);
        }

        public async Task<IEnumerable<Project>> GetByIpTypeAsync(int type)
        {
            return await _context.Projects.Where(p => p.IPType == type).ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetUnapprovedProjectsAsync()
        {
            return await _context.Projects.Where(p => p.Approved == false && p.Proposed == true).ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupsByPreferredProjectAsync(int projectId)
        {
            return await _context.GroupsProjects
                .Where(gp => gp.ProjectId == projectId)
                .Select(gp => gp.Group)
                .ToListAsync();
        }

        public async Task ApproveProjectAsync(Project project, int userId)
        {
            project.Approved = true;
            Group group = _context.Groups.Where(g => g.ProposedProject.Id == project.Id).First();
            project.AssignedGroup = group;
            await UpdateAsync(project);

            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("Your million dollar idea has been approved as a project"),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };

            await SendNotificationsToRangeOfStudentsAsync(notificationContext, group.Students);
        }

        public async Task UnApproveProjectAsync(Project project, int userId)
        {
            Group group = _context.Groups.Where(g => g.ProposedProject.Id == project.Id).First();
            await DeleteAsync(project);   

            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("Your million dollar idea has been rejected"),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };
            await SendNotificationsToRangeOfStudentsAsync(notificationContext, group.Students);
        }

        public async Task UpdateProjectThroughRequestModelAsync(Project project, ProjectUpdate projectUpdate, int userId)
        {
            project.Approved = projectUpdate.Approved;
            project.ClientContact = projectUpdate.ClientContact;
            project.ClientEmail = projectUpdate.ClientEmail;
            project.ClientName = projectUpdate.ClientName;
            project.Comments = projectUpdate.Comments;
            project.Description = projectUpdate.Description;
            project.Difficulty = projectUpdate.Difficulty;
            project.IPType = projectUpdate.IPType;
            project.ProjectName = projectUpdate.ProjectName;
            project.Proposed = projectUpdate.Proposed;
            await UpdateAsync(project);

            if (project.AssignedGroup == null) return;
            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("The project assigned to your team has been updated"),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };

            await SendNotificationsToRangeOfStudentsAsync(notificationContext, project.AssignedGroup.Students);
        }

        //Seed Method
        public async Task UnassignAllProjectsAsync()
        {
            List<Project> projects = _context.Projects.ToList();
            foreach(Project proj in projects)
            {
                proj.AssignedGroup = null;
                await UpdateAsync(proj);
            }
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
