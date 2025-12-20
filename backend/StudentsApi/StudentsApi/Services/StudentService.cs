using Microsoft.EntityFrameworkCore;
using StudentsApi.Data;
using StudentsApi.Domain.Entities;
using StudentsApi.Dtos;

namespace StudentsApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _db;

        public StudentService(ApplicationDbContext db)
        {
            _db = db;
        }



        public async Task<List<StudentReadDto>> GetAllAsync(
            string? search, 
            CancellationToken ct = default)
        {
            var q = _db.Students.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(x =>
                    x.FirstName.ToLower().Contains(s) ||
                    x.LastName.ToLower().Contains(s) ||
                    x.Email.ToLower().Contains(s));
            }

            var students = await q.ToListAsync(ct);

            return students.Select(x => new StudentReadDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                DateOfBirth = x.DateOfBirth,
                CreatedAt = x.CreatedAt,
                EnrollmentDate = x.EnrollmentDate,
                Notes = x.Notes,
                YearOfStudy = x.CalculateYearOfStudy(),
                Age = x.CalculateAge()
            }).ToList();
        }


        public async Task<StudentReadDto?> GetByIdAsync(
            int id, 
            CancellationToken ct = default)
        {
            var student = await _db.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (student is null)
            {
                return null;
            }

            return new StudentReadDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                CreatedAt = student.CreatedAt,
                EnrollmentDate = student.EnrollmentDate,
                Notes = student.Notes,
                YearOfStudy = student.CalculateYearOfStudy(), 
                Age = student.CalculateAge()
            };
        }


        public async Task<(
            bool ok,
            string? error,
            StudentReadDto? created)> CreateAsync(
            StudentCreateDto dto,
            CancellationToken ct = default)
        {
            var email = dto.Email.Trim().ToLowerInvariant();
            var exists = await _db.Students.AnyAsync(x => x.Email == email, ct);

            if (exists)
            {
                return (false, "Email already exists.", null);
            }

            var student = new Student
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = email,
                DateOfBirth = dto.DateOfBirth,
                EnrollmentDate = dto.EnrollmentDate,
                Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim(),
            };

            _db.Students.Add(student);
            await _db.SaveChangesAsync(ct);

            return (true, null, new StudentReadDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                CreatedAt = student.CreatedAt,
                EnrollmentDate = student.EnrollmentDate,
                Notes = student.Notes,
                YearOfStudy = student.CalculateYearOfStudy(),
                Age = student.CalculateAge()
            });
        }


        public async Task<(bool ok, string? error)> UpdateAsync(
            int id, 
            StudentUpdateDto dto, 
            CancellationToken ct = default)
        {
            var student = await _db.Students.FirstOrDefaultAsync(x => x.Id == id, ct);

            if (student is null)
            {
                return (false, "Student not found.");
            }

            var email = dto.Email.Trim().ToLowerInvariant();

            var emailExists = await _db.Students.AnyAsync(x => x.Email == email && x.Id != id, ct);

            if (emailExists)
            {
                return (false, "Email already exists.");
            }

            student.FirstName = dto.FirstName.Trim();
            student.LastName = dto.LastName.Trim();
            student.Email = email;
            student.DateOfBirth = dto.DateOfBirth;
            student.EnrollmentDate = dto.EnrollmentDate;
            student.Notes = string.IsNullOrWhiteSpace(dto.Notes)
                ? null 
                : dto.Notes.Trim();

            await _db.SaveChangesAsync();

            return (true, null);
        }


        public async Task<bool> DeleteAsync(
            int id, 
            CancellationToken ct = default)
        {
            var student = await _db.Students.FirstOrDefaultAsync(x => x.Id == id, ct);

            if (student is null)
            {
                return false;
            }

            _db.Students.Remove(student);
            await _db.SaveChangesAsync(ct);
            return true;
        }
        
    }
}
