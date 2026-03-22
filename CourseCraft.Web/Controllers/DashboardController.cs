using CourseCraft.Repository.ViewModels;
using CourseCraft.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseCraft.Web.Controllers;

public class DashboardController(ICoursesService courseService) : Controller
{
    private readonly ICoursesService _courseService = courseService;

    #region Dashboard

    [HttpGet]
    [Authorize(Roles = "Admin, Student")]
    public IActionResult Index()
    {
        return View();
    }

    #endregion

    #region Enrollments

    [HttpGet]
    public IActionResult Enrollments()
    {
        return View();
    }

    #endregion

    #region GetAllCourses

    [HttpGet]
    [Authorize(Roles = "Admin, Student")]
    public async Task<IActionResult> GetAllCourses()
    {
        int userId = int.Parse(User.FindFirst("UserId")!.Value);
        List<CourseViewModel>? courses = await _courseService.GetAllCoursesAsync(userId);
        return Json(courses);
    }

    #endregion
}
