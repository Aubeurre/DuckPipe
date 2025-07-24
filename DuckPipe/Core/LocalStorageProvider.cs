using System.IO;

namespace DuckPipe.Core
{
    public class LocalStorageProvider : IStorageProvider
    {
        public Task<List<string>> ListFoldersAsync(string path)
        {
            var folders = Directory.Exists(path)
                ? Directory.GetDirectories(path).Select(Path.GetFileName).ToList()
                : new List<string>();

            return Task.FromResult(folders);
        }

        public Task<bool> FolderExistsAsync(string path)
        {
            return Task.FromResult(Directory.Exists(path));
        }

        public Task CreateFolderAsync(string path)
        {
            Directory.CreateDirectory(path);
            return Task.CompletedTask;
        }

        public Task CreateFileAsync(string path, byte[] content)
        {
            File.WriteAllBytes(path, content);
            return Task.CompletedTask;
        }

        public Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            File.Copy(sourcePath, destinationPath, true);
            return Task.CompletedTask;
        }
    }
}
