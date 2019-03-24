using FASTCapstonePortal.Model;
using FASTCapstonePortal.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Interfaces
{
    public interface IStudent
    {
        Task<IEnumerable<Student>> GetByGroupAsync(int id);
        Task<IEnumerable<Student>> GetByProgramAsync(Programs program);
        Task<IEnumerable<Student>> GetByGroupAdminAsync(bool admin);
        Task<Student> GetAdminByGroupAsync(int id);
        Task<IEnumerable<Student>> GetByCampusAsync(Campuses campus);
        Task<bool> PictureExistsAsync(string picture);
        Task AddStudentSkillsAsync(IEnumerable<string> skills, Student student);
        Task UpdateStudentThroughRequestModelAsync(Student student, StudentUpdate studentUpdate);
        Task PutStudentInGroupAsync(Student student, Group group, int userId);
        Task RemoveStudentFromGroupAsync(Student student, int groupId, int userId);
        Task ChangeGroupAdminAsync(Student oldAdmin, Student newAdmin, int userId);
        Task UploadStudentImageAsync(Student student, string imageName);
        Task SaveAsync();
        Task CreateAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(Student student);
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
