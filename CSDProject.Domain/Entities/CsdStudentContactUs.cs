using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDProject.Domain.Entities
{
    [Table("csd_Student_ContactUS")]
    public class CsdStudentContactUs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? MblNumber { get; set; }

        [MaxLength(200)]
        public string? Subject { get; set; }

        [MaxLength(1000)]
        public string? Message { get; set; }
    }
}
