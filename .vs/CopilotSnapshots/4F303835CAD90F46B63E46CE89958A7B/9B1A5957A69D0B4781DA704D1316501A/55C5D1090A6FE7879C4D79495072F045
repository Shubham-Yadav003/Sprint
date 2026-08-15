using SmartShip.TrackingService.Application.Interfaces;

namespace SmartShip.TrackingService.Application.Services
{
    public class FileStorageService: IFileStorageService
    {
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024;

        private static readonly HashSet<string> AllowedExtensions = new(
            StringComparer.OrdinalIgnoreCase)
        {
            ".pdf",
            ".doc",
            ".docx",
            ".jpg",
            ".jpeg",
            ".png"
        };

        private readonly IWebHostEnvironment _environment;
        // information about the application's environment
        public FileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            // IFormFile file, This represents the uploaded file. string folderName, This tells us where to store it.
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required");
            }

            if (file.Length > MaxFileSizeInBytes)
            {
                throw new ArgumentException("File size must be 5 MB or less.");
            }

            var extension = Path.GetExtension(file.FileName);

            if (string.IsNullOrWhiteSpace(extension) ||
                !AllowedExtensions.Contains(extension))
            {
                throw new ArgumentException(
                    "Only .pdf, .doc, .docx, .jpg, .jpeg, and .png files are allowed.");
            }

            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "UploadedFiles", folderName);
            // _environment.ContentRootPath, gives the root directory of our application
            // "C:\\SmartShip\\TrackingService\\UploadedFiles\\" + folderName

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(
                uploadsFolder,
                uniqueFileName);
            // C:\SmartShip\TrackingService\UploadedFiles\DeliveryProofs\8a4f2c31.jpg

            using var stream = new FileStream(
                filePath, FileMode.Create);

            // This opens/creates a file at:
            // FileMode.Create -> Create a new file. If a file with the same name already exists, overwrite it.

            await file.CopyToAsync(stream);
            //This is the actual upload operation

            return Path.Combine(
                "UploadedFiles",
                folderName,
                uniqueFileName);

            // returns path
        }

        public void DeleteFile(string filePath)
        {
            if(string.IsNullOrWhiteSpace(filePath)){
                return;

            }

            var fullPath = Path.Combine(
                _environment.ContentRootPath,
                filePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            
        }
    }
}
