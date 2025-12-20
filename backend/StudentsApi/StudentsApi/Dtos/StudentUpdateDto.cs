using System.ComponentModel.DataAnnotations;

namespace StudentsApi.Dtos
{
    public class StudentUpdateDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!; // unique

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public DateOnly EnrollmentDate { get; set; }

        [MaxLength(4000)]
        public string? Notes { get; set; }
    }
}
