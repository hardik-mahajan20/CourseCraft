using CourseCraft.Repository.Interfaces;
using CourseCraft.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseCraft.Repository.Implementations;

public class CourseStudentMappingRepository(ApplicationDbContext applicationDbContext) : ICourseStudentMappingRepository
{
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    #region GetCourseStudentMappings

    public IQueryable<CourseStudentMapping> GetCourseStudentMappingsAsQueryable()
    {
        return _applicationDbContext.CourseStudentMappings
                             .AsQueryable();
    }

    #endregion

    #region GetCourseStudentMappingsWithUsers

    public IQueryable<CourseStudentMapping> GetCourseStudentMappingsWithUsersAsQueryable()
    {
        return _applicationDbContext.CourseStudentMappings
                             .Include(csm => csm.User)
                             .AsQueryable();
    }

    #endregion

    #region GetCourseStudentMappingsWithUsersAndCourse

    public IQueryable<CourseStudentMapping> GetCourseStudentMappingsWithUsersAndCourseAsQueryable()
    {
        return _applicationDbContext.CourseStudentMappings
                             .Include(csm => csm.User)
                             .Include(csm => csm.Course)
                             .AsQueryable();
    }

    #endregion

    #region AddCourseStudentMapping

    public async Task<bool> AddCourseStudentMappingAsync(CourseStudentMapping courseStudentMapping)
    {
        await _applicationDbContext.CourseStudentMappings.AddAsync(courseStudentMapping);
        return await _applicationDbContext.SaveChangesAsync() > 0;
    }

    #endregion

    #region SaveChanges

    public async Task<int> SaveChangesAsync()
    {
        return await _applicationDbContext.SaveChangesAsync();
    }

    #endregion
}
