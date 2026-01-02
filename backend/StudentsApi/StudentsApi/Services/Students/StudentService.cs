using Microsoft.EntityFrameworkCore;
using StudentsApi.Data;
using StudentsApi.Domain.Entities;
using StudentsApi.Dtos;

namespace StudentsApi.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _db;

        public StudentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 200;
        

        public async Task<PagedResultDto<StudentReadDto>> GetAllAsync(
            string? search, 
            int page,
            int pageSize,
            string? sortBy,
            string? sortDirection,
            CancellationToken ct = default)
        {
            
            // pagination
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? DefaultPageSize : pageSize;
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;


            var q = _db.Students.AsNoTracking();            



            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(x =>
                    x.FirstName.ToLower().Contains(s) ||
                    x.LastName.ToLower().Contains(s) ||
                    x.Email.ToLower().Contains(s));
            }


            // sorting
            var by = (sortBy ?? "id").Trim().ToLowerInvariant();
            var desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

            if (by == "id")
            {
                q = desc
                    ? q.OrderByDescending(x => x.Id)
                    : q.OrderBy(x => x.Id);
            }
            else if (by == "name")
            {
                q = desc
                    ? q.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName)
                    : q.OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
            }
            else if (by == "email")
            {
                q = desc 
                    ? q.OrderByDescending(x => x.Email)
                    : q.OrderBy(x => x.Email);
            }
            else if (by == "age")
            {
                // Age is derived from DateOfBirth
                q = desc 
                    ? q.OrderBy(x => x.DateOfBirth)
                    : q.OrderByDescending(x => x.DateOfBirth);
            }
            else if (by == "studyyear")
            {
                // Studyyear (how many year student is studying) is derived from EnrolmentDate
                q = desc
                    ? q.OrderBy(x => x.EnrollmentDate)
                    : q.OrderByDescending(x => x.EnrollmentDate);
            }            
            else
            {
                // fallback (default case)
                q = q.OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
            }




            var totalCount = await q.CountAsync(ct);

            var students = await q                
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            //return students.Select(x => new StudentReadDto
            //{
            //    Id = x.Id,
            //    FirstName = x.FirstName,
            //    LastName = x.LastName,
            //    Email = x.Email,
            //    DateOfBirth = x.DateOfBirth,
            //    CreatedAt = x.CreatedAt,
            //    EnrollmentDate = x.EnrollmentDate,
            //    Notes = x.Notes,
            //    YearOfStudy = x.CalculateYearOfStudy(),
            //    Age = x.CalculateAge()
            //}).ToList();

            return new PagedResultDto<StudentReadDto>
            {
                TotalCount = totalCount,
                Items = students.Select(x => new StudentReadDto
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
                }).ToList()
            };
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

            
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            
            if (dto.DateOfBirth > today)
            {
                return (false, "Date of birth can't be in the future.", null);
            }
            if (dto.DateOfBirth < today.AddYears(-100))
            {
                return (false, "Date of birth looks too old (100+ years)", null);
            }

            if (dto.EnrollmentDate > today)
            {
                return (false, "Enrollment date can't be in the future.", null);
            }
            if (dto.EnrollmentDate < today.AddYears(-100))
            {
                return (false, "Enrollment date looks too old (100+ years)", null);
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

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (dto.DateOfBirth > today)
            {
                return (false, "Date of birth can't be in the future.");
            }
            if (dto.DateOfBirth < today.AddYears(-100))
            {
                return (false, "Date of birth looks too old (100+ years)");
            }

            if (dto.EnrollmentDate > today)
            {
                return (false, "Enrollment date can't be in the future.");
            }
            if (dto.EnrollmentDate < today.AddYears(-100))
            {
                return (false, "Enrollment date looks too old (100+ years)");
            }


            student.FirstName = dto.FirstName.Trim();
            student.LastName = dto.LastName.Trim();
            student.Email = email;
            student.DateOfBirth = dto.DateOfBirth;
            student.EnrollmentDate = dto.EnrollmentDate;
            student.Notes = string.IsNullOrWhiteSpace(dto.Notes)
                ? null 
                : dto.Notes.Trim();

            await _db.SaveChangesAsync(ct);

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
