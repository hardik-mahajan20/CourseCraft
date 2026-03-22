using CourseCraft.Repository.Models;
using CourseCraft.Repository.ViewModels;

namespace CourseCraft.Service.Interfaces;

public interface ICoursesService
{
    /// <summary>
    /// Retrieves all course asynchronously.
    /// </summary>
    /// <returns>A list of course asynchronously.</returns>
    Task<List<Course>> GetAllCoursesAsync();

    /// <summary>
    /// Retrieves all studentEnrollmentViewModel asynchronously.
    /// </summary>
    /// <returns>A list of studentEnrollmentViewModel asynchronously.</returns>
    Task<List<StudentEnrollmentViewModel>> GetEnrollmentsAsync();

    /// <summary>
    /// Retrieves all course asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve the course for.</param>
    /// <returns>A list of course asynchronously.</returns>
    Task<List<Course>> GetCoursesByUserIdAsync(int userId);

    /// <summary>
    /// Retrieves all courseViewModel asynchronously.
    /// </summary>
    /// <param name="courseId">The ID of the course to retrieve the courseViewModel for.</param>
    /// <returns>A courseViewModel asynchronously.</returns>
    Task<CourseViewModel> GetCourseViewModelByIdAsync(int courseId);

    /// <summary>
    /// Retrieves a paginated list of courseViewModel..
    /// </summary>
    /// <param name="search">An optional search query to filter items.</param>
    /// <param name="pageNumber">The page number for pagination.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortColumn">The filed on the sorting to apply.</param>
    /// <param name="sortDirection">The directino of the sort.</param>
    /// <returns>A task that returns a paginated list of courseViewModel view models.</returns>
    Task<PaginatedList<CourseViewModel>> GetPaginatedCoursesAsync(string search, int page, int pageSize, string sortColumn, string sortDirection);

    /// <summary>
    /// Checks for the duplicate course name.
    /// </summary>
    /// <param name="courseName">The name of the course to check the duplicate for.</param>
    /// <param name="courseId">The courseId of the course to check the duplicate for.</param>
    /// <returns>A task that returns true if the duplicate exist, otherwise false.</returns>
    Task<bool> IsDuplicateCourseNameAsync(string courseName, int? courseId = null);

    /// <summary>
    /// Adds a new courseViewModel asynchronously to the database.
    /// </summary>
    /// <param name="courseViewModel">The courseViewModel entity to add.</param>
    /// <returns>A task that returns true if the update was successful, otherwise false.</returns>
    Task<bool> AddNewCourseAsync(CourseViewModel courseViewModel);

    /// <summary>
    /// Updates an existing courseViewModel asynchronously in the database.
    /// </summary>
    /// <param name="courseViewModel">The courseViewModel to update.</param>
    /// <returns>A task that returns true if the update was successful, otherwise false.</returns>
    Task<bool> UpdateCourseAsync(CourseViewModel courseViewModel);

    /// <summary>
    /// Delete an existing course asynchronously in the database.
    /// </summary>
    /// <param name="courseId">The courseId to delete for.</param>
    /// <returns>A task that returns true if the update was successful, otherwise false.</returns>
    Task<bool> DeleteCourseAsync(int courseId);

    /// <summary>
    /// Toggles an existing course field asynchronously in the database.
    /// </summary>
    /// <param name="courseId">The courseId to toggle for.</param>
    /// <param name="isChecked">The value which should be set.</param>
    /// <returns>A task that returns true if the update was successful, otherwise false.</returns>
    Task<bool> ToggleCourseFieldAsync(int courseId, bool isChecked);

    /// <summary>
    /// Adds student in the course asynchronously.
    /// </summary>
    /// <param name="studentId">The studentId of the student to make mapping for.</param>
    /// <param name="courseId">The courseId of the course to make mapping for.</param>
    /// <returns>A task that returns true if the update was successful, otherwise false.</returns>
    Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId);

    /// <summary>
    /// Checks if the student enrolled in the course or not asynchronously.
    /// </summary>
    /// <param name="userId">The userId of the student to check the existence for.</param>
    /// <param name="courseId">The courseId of the course to check the existence for.</param>
    /// <returns>A task that returns true if student is enrolled, otherwise false.</returns>
    Task<bool> IsStudentEnrolledInCourseAsync(int userId, int courseId);

    /// <summary>
    /// Marks course as complete asynchronously.
    /// </summary>
    /// <param name="userId">The userId of the student to make update for.</param>
    /// <param name="courseId">The courseId of the course to make update for.</param>
    /// <returns>A task that returns true if the update was successful, otherwise false.</returns>
    Task<bool> MarkCourseAsCompleteAsync(int userId, int courseId);
}
