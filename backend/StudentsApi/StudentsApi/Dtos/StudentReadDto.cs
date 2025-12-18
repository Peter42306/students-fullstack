using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsApi.Dtos
{
    public class StudentReadDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!; 

        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateOnly EnrollmentDate { get; set; }
        public string? Notes { get; set; }
        
        public int YearOfStudy { get; set; }
    }
}
