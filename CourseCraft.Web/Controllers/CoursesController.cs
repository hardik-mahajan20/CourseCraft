using CourseCraft.Repository.ViewModels;
using CourseCraft.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseCraft.Web.Controllers;

public class CoursesController(ICoursesService coursesService, ILogger<CoursesController> logger) : Controller
{
    private readonly ICoursesService _coursesService = coursesService;
    private readonly ILogger<CoursesController> _logger = logger;

    #region  Index

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            return View();
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again.";
            return View();
        }
    }

    #endregion

    #region GetCoursesTable

    [HttpGet]
    public async Task<IActionResult> GetCoursesTable(string search = "", int page = 1, int pageSize = 5, string sortColumn = "CourseName", string sortDirection = "asc")
    {
        try
        {
            PaginatedList<CourseViewModel>? courseViewModels = await _coursesService
                                .GetPaginatedCoursesAsync(search, page, pageSize, sortColumn, sortDirection);

            ViewBag.PageNumber = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = courseViewModels.TotalItems;
            ViewBag.TotalPages = courseViewModels.TotalPages;
            ViewBag.FromRec = ((page - 1) * pageSize) + 1;
            ViewBag.ToRec = Math.Min(page * pageSize, courseViewModels.TotalItems);

            return PartialView("_CoursesTablePartialView", courseViewModels);
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    #endregion

    #region AddNewCourse

    [HttpPost]
    public async Task<IActionResult> AddNewCourse(CourseViewModel model)
    {
        try
        {
            if (await _coursesService
                    .IsDuplicateCourseNameAsync(model.CourseName ?? string.Empty, model.CourseId))
            {
                ModelState.AddModelError("CourseName", "Course name already exists.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new
                {
                    success = false,
                    errors
                });
            }

            bool isUserAdded = await _coursesService.AddNewCourseAsync(model);
            return Json(new
            {
                success = isUserAdded,
                message = isUserAdded ? "Course added successfully!" : "Failed to add tax."
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    #endregion

    #region GetCourseById

    [HttpGet]
    public async Task<JsonResult> GetCourseById(int id)
    {
        try
        {
            CourseViewModel? courseViewModel = await _coursesService.GetCourseViewModelByIdAsync(id);
            if (courseViewModel == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Course not found."
                });
            }

            return Json(new
            {
                success = true,
                courseId = courseViewModel.CourseId,
                courseName = courseViewModel.CourseName,
                courseContent = courseViewModel.CourseContent,
                courseCredits = courseViewModel.CourseCredits,
                courseDepartment = courseViewModel.CourseDepartment,
                isOpen = courseViewModel.IsOpen
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    #endregion

    #region UpdateCourse

    [HttpPost]
    public async Task<IActionResult> UpdateCourse(CourseViewModel model)
    {
        try
        {
            if (await _coursesService
                        .IsDuplicateCourseNameAsync(model.CourseName ?? string.Empty, model.CourseId))
            {
                ModelState.AddModelError("CourseName", "Course name already exists.");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { success = false, errors });
            }

            bool isUserUpdated = await _coursesService.UpdateCourseAsync(model);
            if (!isUserUpdated)
            {
                return StatusCode(500, "Error updating course.");
            }

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    #endregion

    #region SoftDeleteCourse 

    [HttpPost]
    public async Task<IActionResult> SoftDeleteCourse(int id)
    {
        try
        {
            bool isDeleted = await _coursesService.DeleteCourseAsync(id);
            return Json(new { success = isDeleted });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    #endregion

    #region ToggleCourseField

    [HttpPost]
    public async Task<IActionResult> ToggleCourseField(int courseId, bool isChecked, string field)
    {
        try
        {
            bool isUpdated = await _coursesService.ToggleCourseFieldAsync(courseId, isChecked);
            return Json(new { success = isUpdated, message = $"{field} updated." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    #endregion

    #region EnrollStudentInCourse

    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> EnrollStudentInCourse(int courseId)
    {
        int userId = int.Parse(User.FindFirst("UserId")!.Value);

        if (userId == 0)
        {
            return Json(new { message = "User not found" });
        }

        bool isAlreadyEnrolled = await _coursesService.IsStudentEnrolledInCourseAsync(userId, courseId);

        if (isAlreadyEnrolled)
        {
            return Json(new { success = false, message = "Student is already enrolled in this course." });
        }

        bool added = await _coursesService.EnrollStudentInCourseAsync(userId, courseId);

        if (added)
        {
            return Json(new { success = true, message = "Enrollment successful" });
        }
        else
        {
            return Json(new { success = false, message = "Enrollment failed. Please try again later." });
        }
    }

    #endregion

    #region GetMyCourses

    [HttpGet]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyCourses()
    {
        int userId = int.Parse(User.FindFirst("UserId")!.Value);

        List<CourseViewModel>? courseViewModels = await _coursesService.GetCoursesByUserIdAsync(userId);

        return Json(courseViewModels);
    }

    #endregion

    #region MarkAsComplete

    [HttpPost]
    public async Task<IActionResult> MarkAsComplete(int courseId)
    {
        int userId = int.Parse(User.FindFirst("UserId")!.Value);

        // Mark the course as complete
        bool success = await _coursesService.MarkCourseAsCompleteAsync(userId, courseId);

        return Json(new { success });
    }

    #endregion
}
