using CourseCraft.Repository.ViewModels;
using CourseCraft.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseCraft.Web.Controllers;

public class EnrollmentsController(ICoursesService courseService) : Controller
{
    private readonly ICoursesService _courseService = courseService;

    #region Enrollments

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Enrollments()
    {
        return View();
    }

    #endregion

    #region GetEnrollments

    [HttpGet]
    public async Task<IActionResult> GetEnrollments()
    {
        List<StudentEnrollmentViewModel>? enrollments = await _courseService.GetEnrollmentsAsync();
        return Json(enrollments);
    }

    #endregion
}
