using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

namespace DuckPipe.Core
{
    public class SynologyStorageProvider : IStorageProvider
    {
        private readonly SynologyFileStation _fileStation;

        public SynologyStorageProvider(SynologyFileStation fileStation)
        {
            _fileStation = fileStation;
        }

        public async Task<List<string>> ListFoldersAsync(string path)
        {
            return await _fileStation.ListFoldersAsync(path);
        }

        public async Task<bool> FolderExistsAsync(string path)
        {
            var folders = await _fileStation.ListFoldersAsync(Path.GetDirectoryName(path));
            var target = Path.GetFileName(path);
            return folders.Contains(target);
        }
        public async Task CreateFolderAsync(string path)
        {
            await _fileStation.CreateFolderAsync(path);
        }

        public async Task CreateFileAsync(string path, byte[] content)
        {
            await _fileStation.CreateFileAsync(path, content);
        }

        public async Task CopyFileAsync(string sourcePath, string destinationPath)
        {

        }

    }
}
