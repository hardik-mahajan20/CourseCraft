using System.ComponentModel.DataAnnotations;

namespace CourseCraft.Repository.ViewModels;

public class CourseViewModel
{
    public int CourseId { get; set; }

    [Required(ErrorMessage = "Course Name is required.")]
    [StringLength(100, ErrorMessage = "Course Name cannot exceed 100 characters.")]
    public string? CourseName { get; set; }

    [Required(ErrorMessage = "Course Content  is required.")]
    [StringLength(100, ErrorMessage = "Course Content  cannot exceed 100 characters.")]
    public string? CourseContent { get; set; }

    [Required(ErrorMessage = "Course Credits is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Course Credits must be a positive number.")]
    public int? CourseCredits { get; set; }

    [Required(ErrorMessage = "Course Department is required.")]
    [StringLength(100, ErrorMessage = "Course Department cannot exceed 100 characters.")]
    public string? CourseDepartment { get; set; }

    public bool? IsOpen { get; set; }
    public bool? IsDeleted { get; set; }

    public bool? IsEnrolled { get; set; }

    public bool? IsCompleted { get; set; }
}
