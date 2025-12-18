using StudentsApi.Dtos;

namespace StudentsApi.Services
{
    public interface IStudentService
    {
        Task<List<StudentReadDto>> GetAllAsync(string? search, CancellationToken ct = default);
        Task<StudentReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<(bool ok, string? error, StudentReadDto? created)> CreateAsync(StudentCreateDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
