using CourseCraft.Repository.Models;

namespace CourseCraft.Repository.Intefaces;

public interface ICoursesRepository
{
    /// <summary>
    /// Retrieves all courses as quearyable.
    /// </summary>
    /// <returns>All courses as quearyable.</returns>
    IQueryable<Course> GetAllCoursesAsQueryable();

    /// <summary>
    /// Retrieves a course by its ID asynchronously.
    /// </summary>
    /// <param name="courseId">The ID of the course to retrieve.</param>
    /// <returns>A task that returns the course if found in the database, otherwise null.</returns>
    Task<Course?> GetCourseByIdAsync(int courseId);

    /// <summary>
    /// Adds a new course asynchronously to the database.
    /// </summary>
    /// <param name="course">The course entity to add.</param>
    /// <returns>A task that returns true if the update was successful, otherwise false.</returns>
    Task<bool> AddNewCourseAsync(Course course);

    /// <summary>
    /// Updates an existing course asynchronously in the database.
    /// </summary>
    /// <param name="course">The course to update.</param>
    /// <returns>A task that returns true if the update was successful, otherwise false.</returns>
    Task<bool> UpdateCourseAsync(Course course);
}
