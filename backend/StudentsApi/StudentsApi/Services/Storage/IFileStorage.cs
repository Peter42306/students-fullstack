namespace StudentsApi.Services.Storage
{
    public interface IFileStorage
    {
        Task<StoredFileResult> SaveAsync(
            IFormFile file,
            string subFolder,
            CancellationToken ct = default);

        Task<StoredFileResult> SaveAsync(
            Stream Stream,
            string originalFileName,
            string contentType,
            string subFolder,
            string fileExtension,
            CancellationToken ct = default);

        Task DeleteIfExistsAsync(
            string subFolder,
            string storedFileName,
            CancellationToken ct = default);

        Task<ReadFileResult> OpenReadAsync(
            string subFolder,
            string storedFileName,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            string subFolder,
            string storedFileName,
            CancellationToken ct = default);
    }
}
