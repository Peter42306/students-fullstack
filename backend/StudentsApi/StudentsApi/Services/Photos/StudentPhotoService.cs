using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StudentsApi.Data;
using StudentsApi.Domain.Entities;
using StudentsApi.Dtos;
using StudentsApi.Services.Storage;

namespace StudentsApi.Services.Photos
{
    public class StudentPhotoService : IStudentPhotoService
    {
        private readonly ApplicationDbContext _db;
        private readonly IFileStorage _storage;
        private readonly FileStorageOptions _options;

        public StudentPhotoService(
            ApplicationDbContext db,
            IFileStorage storage,
            IOptions<FileStorageOptions> options
            )
        {
            _db = db;
            _storage = storage;
            _options = options.Value;
        }

        private static string AvatarFolder(int studentId) => $"students/{studentId}/avatar";
        private static string EnclosuresFolder(int studentId) => $"students/{studentId}/enclosures";


        public async Task<PhotoMetaDto> UploadAvatarAsync(
            int studentId,
            IFormFile file,
            CancellationToken ct = default)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (file.Length <= 0)
            {
                throw new ArgumentException("File is empty", nameof(file));
            }

            if (file.Length > _options.MaxAvatarBytes)
            {
                throw new ArgumentException($"Avatar too large. Max {_options.MaxAvatarBytes} bytes.");
            }

            var studentExists = await _db.Students.AnyAsync(s => s.Id == studentId, ct);
            if (!studentExists)
            {
                throw new KeyNotFoundException($"Student {studentId} not found.");
            }

            // delete previous
            var previousAvatar = await _db.PhotoAvatars.SingleOrDefaultAsync(a => a.StudentId == studentId, ct);
            if (previousAvatar is not null)
            {
                await _storage.DeleteIfExistsAsync(AvatarFolder(studentId), previousAvatar.StoredFileName, ct);
                _db.PhotoAvatars.Remove(previousAvatar);
                await _db.SaveChangesAsync(ct);
            }

            // save new avatar
            var newAvatar = await _storage.SaveAsync(file, AvatarFolder(studentId), ct);

            // save new avatar to db
            var avatar = new PhotoAvatar
            {
                StudentId = studentId,
                FileName = newAvatar.OriginalFileName,
                StoredFileName = newAvatar.StoredFileName,
                ContentType = newAvatar.ContentType,
                Size = newAvatar.Size,
                UploadedAt = DateTime.UtcNow
            };

            _db.PhotoAvatars.Add(avatar);
            await _db.SaveChangesAsync(ct);

            return new PhotoMetaDto(
                avatar.Id,
                avatar.FileName,
                avatar.StoredFileName,
                avatar.ContentType,
                avatar.Size,
                avatar.UploadedAt);
        }


        // Avatar (1:1)
        public async Task<ReadFileResult> GetAvatarFileAsync(
            int studentId,
            CancellationToken ct = default)
        {
            var avatar = await _db.PhotoAvatars.SingleOrDefaultAsync(a => a.StudentId == studentId, ct);
            if (avatar is null)
            {
                throw new KeyNotFoundException("Avatar not found");
            }

            var file = await _storage.OpenReadAsync(AvatarFolder(studentId), avatar.StoredFileName, ct);

            return new ReadFileResult(
                file.Stream,
                avatar.ContentType,
                file.Size);
        }

        
        public async Task<bool> DeleteAvatarAsync(
            int studentId,
            CancellationToken ct = default)
        {
            var avatar = await _db.PhotoAvatars.SingleOrDefaultAsync(a => a.StudentId == studentId, ct);
            if (avatar is null)
            {
                return false;
            }

            await _storage.DeleteIfExistsAsync(AvatarFolder(studentId), avatar.StoredFileName, ct);
            _db.PhotoAvatars.Remove(avatar);
            
            await _db.SaveChangesAsync(ct);
            return true;
        }




        // Enclosures (1:N)
        public async Task<IReadOnlyList<PhotoMetaDto>> UploadEnclosuresAsync(
            int studentId, 
            List<IFormFile> files,
            CancellationToken ct = default)
        {
            if (files is null || files.Count == 0)
            {
                throw new ArgumentException("No files provided.", nameof(files));
            }

            var studentExists = await _db.Students.AnyAsync(s => s.Id == studentId, ct);
            if (!studentExists)
            {
                throw new KeyNotFoundException($"Student {studentId} not found.");
            }

            var entities = new List<PhotoEnclosure>(files.Count);

            foreach (var file in files)
            {
                if (file is null || file.Length <= 0)
                {
                    continue;
                }

                if (file.Length > _options.MaxEnclosureBytes)
                {
                    throw new ArgumentException($"Enclosure too large. Max {_options.MaxEnclosureBytes} bytes.");
                }

                var saved = await _storage.SaveAsync(file, EnclosuresFolder(studentId), ct);

                entities.Add(new PhotoEnclosure
                {
                    StudentId = studentId,
                    FileName = saved.OriginalFileName,
                    StoredFileName = saved.StoredFileName,
                    ContentType = saved.ContentType,
                    Size = saved.Size,
                    UploadedAt = DateTime.UtcNow
                });
            }

            if (entities.Count == 0)
            {
                return Array.Empty<PhotoMetaDto>();
            }

            _db.PhotoEnclosures.AddRange(entities);
            await _db.SaveChangesAsync(ct);

            return entities.Select(e => new PhotoMetaDto
            (
                e.Id,
                e.FileName,
                e.StoredFileName,
                e.ContentType,
                e.Size,
                e.UploadedAt
            )).ToList();            
        }


        public async Task<IReadOnlyList<PhotoMetaDto>> GetEnclosuresAsync(
            int studentId,
            CancellationToken ct = default)
        {
            var items = await _db.PhotoEnclosures
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.UploadedAt)
                .ToListAsync(ct);

            return items.Select(x => new PhotoMetaDto(
                x.Id,
                x.FileName,
                x.StoredFileName,
                x.ContentType,
                x.Size,
                x.UploadedAt)).ToList();
        }


        public async Task<ReadFileResult> GetEnclosureAsync(
            int studentId,
            int enclosureId,
            CancellationToken ct = default)
        {
            var item = await _db.PhotoEnclosures.SingleOrDefaultAsync(x => x.Id == enclosureId && x.StudentId == studentId, ct);            
            
            if (item is null)
            {
                throw new KeyNotFoundException("Enclosure not found.");
            }

            var file = await _storage.OpenReadAsync(EnclosuresFolder(studentId), item.StoredFileName, ct);

            return new ReadFileResult(
                file.Stream,
                item.ContentType,
                file.Size);

        }        


        public async Task<bool> DeleteEnclosureAsync(
            int studentId,
            int enclosureId,
            CancellationToken ct = default)
        {
            var item = await _db.PhotoEnclosures.SingleOrDefaultAsync(x => x.Id == enclosureId && x.StudentId == studentId, ct);

            if (item is null)
            {
                return false;
            }

            await _storage.DeleteIfExistsAsync(EnclosuresFolder(studentId), item.StoredFileName, ct);
            _db.PhotoEnclosures.Remove(item);
            await _db.SaveChangesAsync(ct);

            return true;
        }        
    }
}
