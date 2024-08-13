using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IcBlog.Services
{
    public interface IProfilePictureService
    {
        Task SaveProfilePictureAsync(string userId, IFormFile file);
    }

    public class ProfilePictureService : IProfilePictureService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfilePictureService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task SaveProfilePictureAsync(string userId, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string userFolderPath = Path.Combine(webRootPath, "UserFiles", userId);
                string filePath = Path.Combine(userFolderPath, "ProfilePicture.jpg");

                Directory.CreateDirectory(userFolderPath);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }
    }
}
