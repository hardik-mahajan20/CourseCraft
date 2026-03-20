using System.ComponentModel.DataAnnotations;

namespace CourseCraft.Repository.Models;

public class CourseStudentMapping
{
    [Key]
    public int CourseStudentMappingId { get; set; }
    public int CourseId { get; set; }
    public Course? Course { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public bool? IsCompleted { get; set; }
}
