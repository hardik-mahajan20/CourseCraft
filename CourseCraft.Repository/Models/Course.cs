using System.ComponentModel.DataAnnotations;

namespace CourseCraft.Repository.Models;

public class Course
{
    [Key]
    public int CourseId { get; set; }
    [Required]
    public required string CourseName { get; set; }
    [Required]
    public required string CourseContent { get; set; }
    [Required]
    public required int CourseCredits { get; set; }
    [Required]
    public required string CourseDepartment { get; set; }
    [Required]
    public required bool IsOpen { get; set; }
    [Required]
    public required bool IsDeleted { get; set; }
    public ICollection<CourseStudentMapping>? CourseStudentMappings { get; set; }
}
