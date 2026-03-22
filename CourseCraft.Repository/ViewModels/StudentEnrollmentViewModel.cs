namespace CourseCraft.Repository.ViewModels;

public class StudentEnrollmentViewModel
{
    public string? StudentName { get; set; }
    public string? CourseName { get; set; }
    public int? CourseId { get; set; }
    public int? StudentId { get; set; }
    public bool? IsCompleted { get; set; }
}
