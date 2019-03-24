using FASTCapstonePortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Interfaces
{
    public interface IGroup
    {
        Task<IEnumerable<Group>> GetAllWithProposedProjectsAsync();
        Task<IEnumerable<Group>> GetAllWithoutProposedProjectsAsync();
        Task<IEnumerable<Project>> GetPreferencesInOrderAsync(int groupId);
        Task AddToPreferencesAsync(Project project, Group group, int userID);
        Task RemoveFromPreferencesAsync(Project project, Group group, int userID);
        Task MatchGroupsProjectsAsync(int userID);
        Task<bool> IsStudentInGroupAsync(int studentId, int groupId);
        Task<bool> PictureExistsAsync(string picture);
        Task UpdatePreferencesAsync(Dictionary<int, int> projects, Group group, int userID);
        Task UnAssignProjectAsync(Group group, int userID);
        Task RankProjectsRandomlyAsync();
        Task UpdateDescriptionAsync(Group group, string newDescription, int userID);
        Task UpdateNameAsync(Group group, string newName, int userID);
        Task AssignProjectAsync(Project project, Group group, int userID);
        Task UploadGroupImageAsync(Group group, string imageName, int userID);
        Task SaveAsync();
        Task CreateAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(Group group, int userID);
        Task<IEnumerable<Group>> GetAllAsync();
        Task<Group> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ProjectExistsInPreferencesAsync(Group group, Project project);
    }
}
