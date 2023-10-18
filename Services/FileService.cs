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
    }

    /// <summary>
    /// Service responsible for file operations.
    /// </summary>
    public class FileService : IFileService
    {
        public void Delete(string path)
        {
            File.Delete(path);
        }

        public void WriteAllBytes(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
