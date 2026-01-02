using System.ComponentModel;

namespace StudentsApi.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateOnly EnrollmentDate { get; set; }        
        public string? Notes { get; set; }

        public PhotoAvatar? Avatar { get; set; }
        public List<PhotoEnclosure> Enclosures { get; set; } = new();
        

        public int CalculateYearOfStudy()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            int GetAcademicYear(DateOnly date)
            {
                var academicYearStart = new DateOnly(date.Year, 9, 1);
                return date < academicYearStart 
                    ? date.Year - 1 
                    : date.Year;
            }

            var enrollmentAcademicYear = GetAcademicYear(EnrollmentDate);
            var currentAcademicYear = GetAcademicYear(today);

            var yearOfStudy = currentAcademicYear - enrollmentAcademicYear + 1;

            return Math.Max(1, yearOfStudy);
        }        

        public int CalculateAge()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            
            var age = today.Year - DateOfBirth.Year;
            if (today <DateOfBirth.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
