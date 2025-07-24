using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckPipe.Core
{
    public interface IStorageProvider
    {
        Task<List<string>> ListFoldersAsync(string path);
        Task<bool> FolderExistsAsync(string path);
        Task CreateFolderAsync(string path);
        Task CreateFileAsync(string path, byte[] content);
        Task CopyFileAsync(string sourcePath, string destinationPath);

    }
}
