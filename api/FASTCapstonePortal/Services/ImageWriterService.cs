using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Services
{
    public class ImageWriterService : IImageWriterService
    {
        private readonly IStudent _studentService;
        private readonly IGroup _groupService;

        public ImageWriterService(IStudent studentService, IGroup groupService)
        {
            _studentService = studentService;
            _groupService = groupService;
        }

        public enum ImageFormat
        {
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }

        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");  
            var gif = Encoding.ASCII.GetBytes("GIF");    
            var png = new byte[] { 137, 80, 78, 71 };              
            var tiff = new byte[] { 73, 73, 42 };                  
            var tiff2 = new byte[] { 77, 77, 42 };                 
            var jpeg = new byte[] { 255, 216, 255, 224 };          
            var jpeg2 = new byte[] { 255, 216, 255, 225 };         

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.jpeg;

            return ImageFormat.unknown;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            if(GetImageFormat(fileBytes) == ImageFormat.unknown)
            {
                throw new Exception("Invalid file");
            }

            return await WriteFile(file);
        }

        public async Task<string> WriteFile(IFormFile file)
        {
            string fileName;

            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            fileName = Guid.NewGuid().ToString() + extension;
            while ( await _studentService.PictureExistsAsync(fileName) || await _groupService.PictureExistsAsync(fileName))
            {
                fileName = Guid.NewGuid().ToString() + extension;
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

            using (var bits = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(bits);
            }

            return fileName;
        }

        public void DeleteImage(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);
            if(File.Exists(path))
                File.Delete(path);
        }

        public async Task DeleteAllImages()
        {
            foreach(Student s in await _studentService.GetAllAsync())
            {
                if (s.Picture != null)
                {
                    DeleteImage(s.Picture);
                    s.Picture = null;
                    await _studentService.UpdateAsync(s);
                }
            }
            foreach(Group g in await _groupService.GetAllAsync())
            {
                if (g.Picture != null)
                {
                    DeleteImage(g.Picture);
                    g.Picture = null;
                    await _groupService.UpdateAsync(g);
                }
            }
        }

        public string ConvertFileToB64(string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

            return Convert.ToBase64String(File.ReadAllBytes(path));
        }
    }
}
