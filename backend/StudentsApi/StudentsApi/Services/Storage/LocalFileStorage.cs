
using Microsoft.Extensions.Options;

namespace StudentsApi.Services.Storage
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly FileStorageOptions _options;

        public LocalFileStorage(IOptions<FileStorageOptions> options)
        {
            _options = options.Value;

            if (string.IsNullOrWhiteSpace(_options.RootPath))
            {
                throw new InvalidOperationException("FileStorage: RootPath is not configured.");
            }
        }


        public async Task<StoredFileResult> SaveAsync(
            IFormFile file, 
            string subFolder,
            CancellationToken ct = default)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (file.Length <= 0)
            {
                throw new ArgumentException("File is empty.", nameof(file));
            }

            await using var input = file.OpenReadStream();
            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(ext))
            {
                ext = ".bin";
            }

            return await SaveAsync(
                input,
                originalFileName: file.FileName,
                contentType: string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
                subFolder: subFolder,
                fileExtension: ext,
                ct: ct);
        }

        public async Task<StoredFileResult> SaveAsync(
            Stream content, 
            string originalFileName,
            string contentType,
            string subFolder, 
            string fileExtension, 
            CancellationToken ct = default)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (!content.CanRead)
            {
                throw new ArgumentException("Content stream is not readable.", nameof(content));
            }

            var safeSubFolder = NormalizeSubFolder(subFolder);
            var folderPath = Path.Combine(_options.RootPath, safeSubFolder);
            Directory.CreateDirectory(folderPath);

            var storedFileName = $"{Guid.NewGuid():N}{NormalizeExtension(fileExtension)}";
            var fullPath = Path.Combine(folderPath, storedFileName);

            await using (var fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, useAsync: true))
            {
                await content.CopyToAsync(fs, ct);
            }

            var fileInfo = new FileInfo(fullPath);
            var relativePath = safeSubFolder.Replace(Path.DirectorySeparatorChar, '/');

            return new StoredFileResult(
                StoredFileName: storedFileName,
                RelativePath: relativePath,
                Size: fileInfo.Length,
                ContentType: string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType,
                OriginalFileName: string.IsNullOrWhiteSpace(originalFileName) ? storedFileName : originalFileName);

        }

        public Task DeleteIfExistsAsync(string subFolder, string storedFileName, CancellationToken ct = default)
        {
            var path = BuildFullPath(subFolder, storedFileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(
            string subFolder,
            string storedFileName,
            CancellationToken ct = default)
        {
            var path = BuildFullPath(subFolder, storedFileName);
            return Task.FromResult(File.Exists(path));
        }

        public Task<ReadFileResult> OpenReadAsync(
            string subFolder, 
            string storedFileName, 
            CancellationToken ct = default)
        {
            var path = BuildFullPath(subFolder, storedFileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found.", storedFileName);
            }

            var stream = new FileStream(
                path,
                FileMode.Open, 
                FileAccess.Read,
                FileShare.Read,
                81920,
                useAsync: true);

            return Task.FromResult(
                new ReadFileResult(
                    stream,
                    ContentType: "application/octet-stream",
                    Size: stream.Length));
        }                

        
        private string BuildFullPath(
            string subFolder, 
            string storedFileName)
        {
            var safeSubFolder = NormalizeSubFolder(subFolder);
            var safeFileName = Path.GetFileName(storedFileName);
            return Path.Combine(
                _options.RootPath, 
                safeSubFolder, 
                safeFileName);
        }

        private static string NormalizeSubFolder(string subFolder)
        {
            subFolder ??= "";
            subFolder = subFolder.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            subFolder = subFolder.Trim(Path.DirectorySeparatorChar);

            if (subFolder.Contains(".."))
            {
                throw new ArgumentException("Invalid subFolder.", nameof(subFolder));
            }

            return subFolder;
        }

        private static string NormalizeExtension(string ext)
        {
            if (string.IsNullOrWhiteSpace(ext))
            {
                return ".bin";
            }

            return ext.StartsWith(".")
                ? ext
                : "." + ext;
        }
    }
}
