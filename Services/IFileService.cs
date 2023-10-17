namespace Services
{
    public interface IFileService
    {
        bool Exists(string path);
        void Delete(string path);
        void WriteAllBytes(string path, byte[] bytes);
        byte[] ReadAllBytes(string path);
    }

    public class FileService : IFileService
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

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
