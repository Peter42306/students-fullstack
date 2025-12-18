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

        public int CalculateYearOfStudy()
        {
            return DateTime.UtcNow.Year - EnrollmentDate.Year + 1;
        }        
    }
}
