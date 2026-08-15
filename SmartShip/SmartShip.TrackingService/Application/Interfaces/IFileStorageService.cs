namespace SmartShip.TrackingService.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        //IFormFile represents a file sent through an HTTP request using

        void DeleteFile(string filePath);
    }
}
