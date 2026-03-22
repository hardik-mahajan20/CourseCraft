using CourseCraft.Repository.Models;

namespace CourseCraft.Repository.Interfaces;

public interface ICourseStudentMappingRepository
{
    /// <summary>
    /// Retrieves all courseStudentMapping as queryable.
    /// </summary>
    /// <returns>All courseStudentMapping as queryable.</returns>
    IQueryable<CourseStudentMapping> GetCourseStudentMappingsAsQueryable();

    /// <summary>
    /// Retrieves all courseStudentMapping with users as queryable.
    /// </summary>
    /// <returns>All courseStudentMapping with users as queryable.</returns>
    IQueryable<CourseStudentMapping> GetCourseStudentMappingsWithUsersAsQueryable();

    /// <summary>
    /// Retrieves all courseStudentMapping with users and course as queryable.
    /// </summary>
    /// <returns>All courseStudentMapping with users and course as queryable.</returns>
    IQueryable<CourseStudentMapping> GetCourseStudentMappingsWithUsersAndCourseAsQueryable();

    /// <summary>
    /// Adds a new courseStudentMapping asynchronously to the database.
    /// </summary>
    /// <param name="courseStudentMapping">The courseStudentMapping entity to add.</param>
    /// <returns>A task that returns true if the update was successful, otherwise false.</returns>
    Task<bool> AddCourseStudentMappingAsync(CourseStudentMapping courseStudentMapping);

    /// <summary>
    /// Saves changes to the data source asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    Task<int> SaveChangesAsync();
}
