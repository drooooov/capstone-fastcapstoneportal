using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Services
{
    public interface IImageWriterService
    {
        Task<string> UploadImage(IFormFile file);
        void DeleteImage(string fileName);
        Task DeleteAllImages();
        string ConvertFileToB64(string fileName);
    }
}
