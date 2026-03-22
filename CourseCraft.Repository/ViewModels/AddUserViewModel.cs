using System.ComponentModel.DataAnnotations;

namespace CourseCraft.Repository.ViewModels;

public class AddUserViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
    [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid Email Address format")]
    public required string UserEmail { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters long.")]
    public required string UserPassword { get; set; }

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("UserPassword", ErrorMessage = "Passwords do not match.")]
    public required string ConfirmPassword { get; set; }

    public string UserRole { get; set; } = "Student";

    [Required(ErrorMessage = "UserName  is required")]
    [StringLength(100, ErrorMessage = "UserName cannot be longer than 100 characters.")]
    public required string UserName { get; set; }
}
