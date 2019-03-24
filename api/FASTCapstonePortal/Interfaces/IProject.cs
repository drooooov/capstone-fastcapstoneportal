using FASTCapstonePortal.Model;
using FASTCapstonePortal.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Interfaces
{
    public interface IProject
    {
        Task<IEnumerable<Project>> GetByClientAsync(string clientName);
        Task<IEnumerable<Project>> GetByDifficultyAsync(int difficulty);
        Task<IEnumerable<Project>> GetApprovedProjectsAsync();
        Task<IEnumerable<Project>> GetUnapprovedProjectsAsync();
        Task<IEnumerable<Project>> GetByIpTypeAsync(int type);
        Task<IEnumerable<Group>> GetGroupsByPreferredProjectAsync(int projectId);
        Task ApproveProjectAsync(Project project, int userId);
        Task UnApproveProjectAsync(Project project, int userId);
        Task UpdateProjectThroughRequestModelAsync(Project project, ProjectUpdate projectUpdate, int userId);
        Task UnassignAllProjectsAsync();
        Task SaveAsync();
        Task CreateAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project> GetByIdAsync(int projectId);
        Task<bool> ExistsAsync(int projectId);

    }
}
