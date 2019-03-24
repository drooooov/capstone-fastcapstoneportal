using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Repositories
{
    public class GroupRepositoryService : IGroup
    {
        protected readonly CapstoneDBContext _context;
        protected readonly IHubContext<SignalServer> _hubContext;

        public GroupRepositoryService(CapstoneDBContext context, IHubContext<SignalServer> hubContext)

        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public async Task CreateAsync(Group group)
        {
            await _context.AddAsync(group);
            await SaveAsync();
        }

        public async Task UpdateAsync(Group group)
        {
            _context.Entry(group).State = EntityState.Modified;
            await SaveAsync();

            //Group g = await GetByIdAsync(group.Id);
            //foreach (Student s in g.Students)
            //{
            //    await _hubContext.Clients.Group(s.Id.ToString()).SendAsync("GroupHasBeenUpdated");
            //}
        }

        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            return await _context.Set<Group>().ToListAsync();
        }


        public async Task<IEnumerable<Group>> GetAllWithoutProposedProjectsAsync()
        {
            return await _context.Groups.Where(g => g.ProposedProject == null).ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetAllWithProposedProjectsAsync()
        {
            return await _context.Groups.Where(g => g.ProposedProject != null).ToListAsync();
        }

        public async Task<Group> GetByIdAsync(int groupId)
        {
            return await _context.Groups.FindAsync(groupId);
        }

        public async Task<bool> IsStudentInGroupAsync(int studentId, int groupId)
        {
            return await _context.Groups.AnyAsync(g => g.Id == groupId && g.Students.Any(s => s.Id == studentId));
        }

        public async Task<bool> ExistsAsync(int groupId)
        {
            return await _context.Groups.AnyAsync(g => g.Id == groupId);
        }

        public async Task<IEnumerable<Project>> GetPreferencesInOrderAsync(int groupId)
        {
            return await _context.Groups
                .Where(g => g.Id == groupId)
                .SelectMany(g => g.PreferredProjects
                    .OrderBy(gp => gp.Preference)
                    .Select(gp => gp.Project))
                .ToListAsync();
        }

        public async Task<bool> PictureExistsAsync(string picture)
        {
            return await _context.Groups.AnyAsync(g => g.Picture == picture);
        }

        public async Task MatchGroupsProjectsAsync(int userId)
        {
            Random rnd = new Random();
            List<int> groupsPicked = new List<int>();
            List<Group> groups = _context.Groups.Where(g => g.FinalProjectAssigned == null).ToList();

            for (int i = 0; i < groups.Count(); i++)
            {
                int random = rnd.Next(groups.Count);
                while (groupsPicked.Contains(random))
                {
                    random = rnd.Next(groups.Count);
                }
                groupsPicked.Add(random);

                List<GroupsProjects> preferredProjects = groups[random].PreferredProjects.ToList();

                for (int j = 0; j < preferredProjects.Count; j++)
                {
                    if (preferredProjects[j].Project.AssignedGroup == null)
                    {
                        await AssignProjectAsync(preferredProjects[j].Project, groups[random], userId);
                        break;
                    }
                }
            }
        }

        public async Task<bool> ProjectExistsInPreferencesAsync(Group group, Project project)
        {
            return await _context.GroupsProjects.AnyAsync(gp => gp.GroupId == group.Id && gp.ProjectId == project.Id);
        }

        public async Task AddToPreferencesAsync(Project project, Group group, int userId)
        {
            int count = group.PreferredProjects.Count;
            group.PreferredProjects.Add(new GroupsProjects { Group = group, Project = project, Preference = count + 1 });
             await this.UpdateAsync(group);

            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("{0} has been added to your project preferences", project.ProjectName),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };

            await SendNotificationsToRangeOfStudentsAsync(notificationContext, group.Students);
        }

        public async Task RemoveFromPreferencesAsync(Project project, Group group, int userId)
        {
            group.PreferredProjects.Remove(_context.GroupsProjects.Where(p => p.Project.Equals(project)).First());
            await this.UpdateAsync(group);


            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("{0} has been removed from your project preferences", project.ProjectName),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };
            await SendNotificationsToRangeOfStudentsAsync(notificationContext, group.Students);
        }

        public async Task UnAssignProjectAsync(Group group, int userId)
        {
            group.FinalProjectAssigned = null;
            await UpdateAsync(group);

            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("The project assigned to your group has been unassigned"),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };
            await SendNotificationsToRangeOfStudentsAsync(notificationContext, group.Students);
        }

        public async Task AssignProjectAsync(Project project, Group group, int userId)
        {
            group.FinalProjectAssigned = project;
            await UpdateAsync(group);

            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("{0} has been assigned to your group", project.ProjectName),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };
            await SendNotificationsToRangeOfStudentsAsync(notificationContext, group.Students);
        }

        public async Task UpdatePreferencesAsync(Dictionary<int,int> Projects, Group group, int userId)
        {
            group.PreferredProjects.Clear();
            for (int i = 1; i <= Projects.Count; i++)
            {
                group.PreferredProjects.Add(new GroupsProjects { Group = group, Project = await _context.Projects.FindAsync(Projects[i]), Preference = i });
            }
            await UpdateAsync(group);

            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("Your groups project preferences have been updated"),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };
            await SendNotificationsToRangeOfStudentsAsync(notificationContext, group.Students);
        }

        public async Task UpdateDescriptionAsync(Group group, string newDescription, int userId)
        {
            group.Description = newDescription;
            await UpdateAsync(group);

            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("Your groups description has been updated"),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };
            await SendNotificationsToRangeOfStudentsAsync(notificationContext, group.Students);
        }

        public async Task UpdateNameAsync(Group group, string newName, int userId)
        {
            group.Name = newName;
            await UpdateAsync(group);

            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("Your groups name has been updated"),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };
            await SendNotificationsToRangeOfStudentsAsync(notificationContext, group.Students);
        }

        public async Task DeleteAsync(Group group, int userId)
        {
            List<Student> students = group.Students.ToList();
            _context.Remove(group);
            await SaveAsync();

            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("Your group has been deleted"),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };
            await SendNotificationsToRangeOfStudentsAsync(notificationContext, students);
        }

        public async Task RankProjectsRandomlyAsync()
        {
            Random rnd = new Random();
            List<Project> projects = _context.Projects.ToList();
            foreach (Group group in await GetAllAsync())
            {
                group.PreferredProjects.Clear();
                await UpdateAsync(group);
            }
            foreach (Group group in await GetAllAsync())
            {
                int random = rnd.Next(projects.Count);

                for (int i = 0; i < 5; i++)
                {
                    while (await _context.GroupsProjects.AnyAsync(gp => gp.Group.Equals(group) && gp.Project.Equals(projects[random])))
                        random = rnd.Next(projects.Count);
                    _context.GroupsProjects.Add(new GroupsProjects { Group = group, Project = projects[random], Preference = i + 1 });
                    await SaveAsync();
                }
            }
        }

        public async Task UploadGroupImageAsync(Group group, string imageName, int userId)
        {
            group.Picture = imageName;
            await UpdateAsync(group);

            NotificationContext notificationContext = new NotificationContext()
            {
                CreatedBy = await _context.Users.FindAsync(userId),
                Data = string.Format("Your groups image has been updated"),
                NotificationType = NotificationType.SYSTEM,
                Time = DateTime.UtcNow
            };
            await SendNotificationsToRangeOfStudentsAsync(notificationContext, group.Students);
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