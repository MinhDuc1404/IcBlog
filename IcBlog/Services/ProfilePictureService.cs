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
        Task SaveProfilePictureFromUrlAsync(string userId, string imageUrl);
    }

    public class ProfilePictureService : IProfilePictureService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient _httpClient;

        public ProfilePictureService(IWebHostEnvironment webHostEnvironment, HttpClient httpClient)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpClient = httpClient;
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

        public async Task SaveProfilePictureFromUrlAsync(string userId, string imageURL)
        {
            if (!string.IsNullOrEmpty(imageURL))
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string userFolderPath = Path.Combine(webRootPath, "UserFiles", userId);
                string filePath = Path.Combine(userFolderPath, "ProfilePicture.jpg");

                Directory.CreateDirectory(userFolderPath);

                using (var response = await _httpClient.GetAsync(imageURL))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await response.Content.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Failed to download the image from the provided URL.");
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid image URL provided.");
            }
        }

    }
}
