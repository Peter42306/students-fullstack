using StudentsApi.Dtos;

namespace StudentsApi.Services.Students
{
    public interface IStudentService
    {
        Task<PagedResultDto<StudentReadDto>> GetAllAsync(
            string? search, 
            int page,
            int pageSize,
            string? sortBy,
            string? sortDirection,
            CancellationToken ct = default);

        Task<StudentReadDto?> GetByIdAsync(
            int id, 
            CancellationToken ct = default);

        Task<(bool ok, string? error, StudentReadDto? created)> CreateAsync(
            StudentCreateDto dto, 
            CancellationToken ct = default);

        Task<(bool ok, string? error)> UpdateAsync(
            int id,
            StudentUpdateDto dto,
            CancellationToken ct = default);

        Task<bool> DeleteAsync(
            int id, 
            CancellationToken ct = default);
    }
}
