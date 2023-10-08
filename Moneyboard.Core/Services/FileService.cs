using Microsoft.Extensions.Options;
using Moneyboard.Core.ApiModels;
using Moneyboard.Core.Helpers;
using Moneyboard.Core.Interfaces.Services;

namespace Moneyboard.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IOptions<FileSettings> _fileSettings;
        private readonly ILocaleStorageService _localeStorageService;

        public FileService(IOptions<FileSettings> fileSettings,
            ILocaleStorageService localeStorageService)
        {
            _fileSettings = fileSettings;
            _localeStorageService = localeStorageService;
        }

        public async Task<string> AddFileAsync(Stream stream, string folderPath, string fileName)
        {
            return await _localeStorageService.AddFileAsync(stream, folderPath, fileName);

        }

        public async Task<DownloadFile> GetFileAsync(string dbPath)
        {
            DownloadFile file = null;
            var storedFilePath = dbPath.Split(":");

            file = await _localeStorageService.GetFileAsync(storedFilePath[1]);
            return file;
        }

        public async Task DeleteFileAsync(string dbPath)
        {
            var storedFilePath = dbPath.Split(":");
          

            await _localeStorageService.DeleteFileAsync(storedFilePath[1]);

        }
    }
}
