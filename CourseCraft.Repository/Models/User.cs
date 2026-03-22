using System.ComponentModel.DataAnnotations;

namespace CourseCraft.Repository.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    public required string UserName { get; set; }
    [Required]
    public required string UserEmail { get; set; }
    [Required]
    public required string UserPassword { get; set; }
    [Required]
    public required string UserRole { get; set; }
    public ICollection<CourseStudentMapping>? CourseStudentMappings { get; set; }
}
