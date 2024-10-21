using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace UKHRM.Helper
{
    public class ImageChecker
    {
        private readonly IWebHostEnvironment _env;
        public ImageChecker(IWebHostEnvironment env)
        {
            _env = env;
        }

        public bool ImageExists(string imageName)
        {
            // Combine the wwwroot path with the image name
            string imagePath = Path.Combine(_env.WebRootPath, imageName);

            // Check if the file exists
            return File.Exists(imagePath);
        }
    }
}
