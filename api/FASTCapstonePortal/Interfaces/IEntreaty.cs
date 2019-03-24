using FASTCapstonePortal.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Interfaces
{
    public interface IEntreaty
    {
        Task CreateInviteAsync(Entreaty invite, int userId);
        Task CreateRequestAsync(Entreaty invite, int userId);
        Task RejectAsync(Entreaty entreaty);
        Task<bool> ExistsAsync(int studentId, int groupId);
        Task SaveAsync();
    }
}
