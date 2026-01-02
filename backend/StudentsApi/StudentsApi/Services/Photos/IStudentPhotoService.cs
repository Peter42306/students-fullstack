using StudentsApi.Dtos;
using StudentsApi.Services.Storage;

namespace StudentsApi.Services.Photos
{
    public interface IStudentPhotoService
    {
        // Avatar (1:1)
        Task<PhotoMetaDto> UploadAvatarAsync(
            int studentId, 
            IFormFile file, 
            CancellationToken ct = default);
        Task<ReadFileResult> GetAvatarFileAsync(
            int studentId, 
            CancellationToken ct = default);
        Task<bool> DeleteAvatarAsync(
            int studentId, 
            CancellationToken ct = default);


        // Enclosures (1:N)
        Task<IReadOnlyList<PhotoMetaDto>> UploadEnclosuresAsync(
            int studentId,
            List<IFormFile> files,
            CancellationToken ct = default);
        Task<IReadOnlyList<PhotoMetaDto>> GetEnclosuresAsync(
            int studentId,
            CancellationToken ct = default);
        Task<ReadFileResult> GetEnclosureAsync(
            int studentId,
            int enclosureId,
            CancellationToken ct = default);
        Task<bool> DeleteEnclosureAsync(
            int studentId,
            int enclosureId,
            CancellationToken ct = default);
    }
}
