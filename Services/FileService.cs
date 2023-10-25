using Microsoft.Extensions.Logging;

namespace Services
{
    /// <summary>
    /// Provides functionalities related to file operations.
    /// </summary>
    public interface IFileService
    {
        void Delete(string path);
        void WriteAllBytes(string path, byte[] bytes);
        byte[] ReadAllBytes(string path);
        bool Exists(string path);
    }

    /// <summary>
    /// Service responsible for file operations.
    /// </summary>
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }

        public void Delete(string path)
        {
            try
            {
                _logger.LogInformation($"Deleting file at path: {path}");
                File.Delete(path);
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, $"Failed to delete file at path: {path}");
            }
        }
        public void WriteAllBytes(string path, byte[] bytes)
        {
            try
            {
                var directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to write bytes to file at path: {path}");
                throw;  // re-throw the exception or handle it as necessary
            }
        }
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
