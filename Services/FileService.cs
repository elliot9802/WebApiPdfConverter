using Microsoft.Extensions.Logging;

namespace Services
{
    /// <summary>
    /// Provides functionalities related to file operations.
    /// </summary>
    public interface IFileService
    {
        Task DeleteAsync(string path);
        Task WriteAllBytesAsync(string path, byte[] bytes);
        Task<byte[]> ReadAllBytesAsync(string path);
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

        public async Task DeleteAsync(string path)
        {
            try
            {
                _logger.LogInformation($"Deleting file at path: {path}");
                await Task.Run(() => File.Delete(path));
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, $"Failed to delete file at path: {path}");
            }
        }

        public async Task WriteAllBytesAsync(string path, byte[] bytes)
        {
            try
            {
                var directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                await File.WriteAllBytesAsync(path, bytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to write bytes to file at path: {path}");
                throw;  // re-throw the exception or handle it as necessary
            }
        }

        public async Task<byte[]> ReadAllBytesAsync(string path)
        {
            return await File.ReadAllBytesAsync(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
