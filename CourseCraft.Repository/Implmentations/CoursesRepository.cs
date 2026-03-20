using CourseCraft.Repository.Intefaces;
using CourseCraft.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseCraft.Repository.Implmentations;

public class CoursesRepository(ApplicationDbContext applicationDbContext) : ICoursesRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    #region GetAllCourses

    public IQueryable<Course> GetAllCoursesAsQueryable()
    {
        return _applicationDbContext.Courses.AsQueryable();
    }

    #endregion

    #region GetCourseById

    public async Task<Course?> GetCourseByIdAsync(int courseId)
    {
        return await _applicationDbContext.Courses.FirstOrDefaultAsync(t => t.CourseId == courseId);
    }

    #endregion

    #region AddNewCourse

    public async Task<bool> AddNewCourseAsync(Course course)
    {
        await _applicationDbContext.Courses.AddAsync(course);
        return await _applicationDbContext.SaveChangesAsync() > 0;
    }

    #endregion

    #region UpdateCourse

    public async Task<bool> UpdateCourseAsync(Course course)
    {
        _applicationDbContext.Courses.Update(course);
        return await _applicationDbContext.SaveChangesAsync() > 0;
    }

    #endregion
}
