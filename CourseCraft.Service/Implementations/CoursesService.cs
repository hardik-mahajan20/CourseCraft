using CourseCraft.Repository.Interfaces;
using CourseCraft.Repository.Models;
using CourseCraft.Repository.ViewModels;
using CourseCraft.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseCraft.Service.Implementations;

public class CoursesService(ICoursesRepository coursesRepository, ICourseStudentMappingRepository courseStudentMappingRepository) : ICoursesService
{
    private readonly ICoursesRepository _coursesRepository = coursesRepository;
    private readonly ICourseStudentMappingRepository _courseStudentMappingRepository = courseStudentMappingRepository;

    #region GetAllCourses

    public async Task<List<CourseViewModel>> GetAllCoursesAsync(int userId)
    {
        List<CourseViewModel>? courses = await _coursesRepository
         .GetAllCoursesAsQueryable()
         .Where(c => c.IsOpen && !c.IsDeleted)
         .Select(c => new CourseViewModel
         {
             CourseId = c.CourseId,
             CourseName = c.CourseName,
             CourseContent = c.CourseContent,
             CourseCredits = c.CourseCredits,
             CourseDepartment = c.CourseDepartment,

             IsEnrolled = _courseStudentMappingRepository
                 .GetCourseStudentMappingsWithUsersAndCourseAsQueryable()
                 .Any(csm => csm.CourseId == c.CourseId && csm.UserId == userId)
         })
         .ToListAsync();

        return courses;
    }

    #endregion

    #region GetEnrollments

    public async Task<List<StudentEnrollmentViewModel>> GetEnrollmentsAsync()
    {
        List<CourseStudentMapping>? courseStudentMappings
        = await _courseStudentMappingRepository
            .GetCourseStudentMappingsWithUsersAndCourseAsQueryable()
                .ToListAsync();

        return courseStudentMappings.Select(e => new StudentEnrollmentViewModel
        {
            StudentName = e.User!.UserName,
            CourseName = e.Course!.CourseName,
            CourseId = e.Course!.CourseId,
            StudentId = e.User!.UserId,
            IsCompleted = e.IsCompleted,
        }).ToList();
    }

    #endregion

    #region GetCoursesByUserId

    public async Task<List<CourseViewModel>> GetCoursesByUserIdAsync(int userId)
    {
        List<CourseStudentMapping>? courseStudentMappings = await _courseStudentMappingRepository
                                                                    .GetCourseStudentMappingsWithUsersAsQueryable()
                                                                        .Where(csm => csm.UserId == userId)
                                                                         .ToListAsync();

        List<CourseViewModel>? courses = courseStudentMappings
               .Where(csm => csm.Course != null)
               .Select(csm => new CourseViewModel
               {
                   CourseId = csm.Course!.CourseId,
                   CourseName = csm.Course.CourseName,
                   CourseContent = csm.Course.CourseContent,
                   CourseCredits = csm.Course.CourseCredits,
                   CourseDepartment = csm.Course.CourseDepartment,
                   IsOpen = csm.Course.IsOpen,
                   IsCompleted = csm.IsCompleted
               })
               .ToList();

        return courses;
    }

    #endregion

    #region GetCourseViewModelById

    public async Task<CourseViewModel> GetCourseViewModelByIdAsync(int courseId)
    {
        Course? course = await _coursesRepository.GetCourseByIdAsync(courseId);
        if (course == null) return new CourseViewModel();

        return new CourseViewModel
        {
            CourseId = course.CourseId,
            CourseName = course.CourseName,
            CourseContent = course.CourseContent,
            CourseCredits = course.CourseCredits,
            CourseDepartment = course.CourseDepartment,
            IsOpen = course.IsOpen
        };
    }

    #endregion

    #region GetPaginatedCourses

    public async Task<PaginatedList<CourseViewModel>> GetPaginatedCoursesAsync(string search, int page, int pageSize, string sortColumn, string sortDirection)
    {
        IQueryable<Course>? query = _coursesRepository.GetAllCoursesAsQueryable().Where(c => c.IsDeleted == false);

        if (!string.IsNullOrWhiteSpace(search))
        {
            string trimmedSearch = search.Trim().ToLower();
            query = query.Where(t =>
                t.CourseName.ToLower().Contains(trimmedSearch) ||
                t.CourseDepartment.ToLower().Contains(trimmedSearch)
            );
        }

        query = sortColumn switch
        {
            "CourseName" => sortDirection == "asc"
                ? query.OrderBy(t => t.CourseName)
                : query.OrderByDescending(t => t.CourseName),
            _ => sortDirection == "asc"
                ? query.OrderBy(t => t.CourseId)
                : query.OrderByDescending(t => t.CourseId),
        };

        PaginatedList<Course>? courses = await PaginatedList<Course>.CreateAsync(query, page, pageSize);

        List<CourseViewModel>? coursesViewModel = courses.Select(t => new CourseViewModel
        {
            CourseId = t.CourseId,
            CourseName = t.CourseName,
            CourseDepartment = t.CourseDepartment,
            CourseContent = t.CourseContent,
            CourseCredits = t.CourseCredits,
            IsOpen = t.IsOpen
        }).ToList();

        return new PaginatedList<CourseViewModel>(coursesViewModel, courses.TotalItems, page, pageSize);
    }

    #endregion

    #region IsDuplicateCourseName

    public async Task<bool> IsDuplicateCourseNameAsync(string courseName, int? courseId = null)
    {
        bool isDuplicateCourseName = await _coursesRepository
                                            .GetAllCoursesAsQueryable()
                                                .Where(c => c.IsDeleted == false).AnyAsync(t =>
                                                    t.CourseName == courseName &&
                                                        (!courseId.HasValue || t.CourseId != courseId));

        return isDuplicateCourseName;
    }

    #endregion

    #region AddNewCourse

    public async Task<bool> AddNewCourseAsync(CourseViewModel courseViewModel)
    {
        Course course = new()
        {
            CourseName = courseViewModel.CourseName ?? string.Empty,
            CourseContent = courseViewModel.CourseContent ?? string.Empty,
            CourseCredits = courseViewModel.CourseCredits ?? 0,
            CourseDepartment = courseViewModel.CourseDepartment ?? string.Empty,
            IsOpen = courseViewModel.IsOpen ?? true,
            IsDeleted = courseViewModel.IsDeleted ?? false
        };

        return await _coursesRepository.AddNewCourseAsync(course);
    }

    #endregion

    #region UpdateCourse

    public async Task<bool> UpdateCourseAsync(CourseViewModel courseViewModel)
    {
        Course? course = await _coursesRepository.GetCourseByIdAsync(courseViewModel.CourseId);
        if (course == null) return false;

        course.CourseName = courseViewModel.CourseName ?? string.Empty;
        course.CourseContent = courseViewModel.CourseContent ?? string.Empty;
        course.CourseCredits = courseViewModel.CourseCredits ?? 0;
        course.CourseDepartment = courseViewModel.CourseDepartment ?? string.Empty;

        return await _coursesRepository.UpdateCourseAsync(course);
    }

    #endregion

    #region DeleteCourse

    public async Task<bool> DeleteCourseAsync(int courseId)
    {
        Course? course = await _coursesRepository.GetCourseByIdAsync(courseId);
        if (course == null) return false;

        course.IsDeleted = true;

        return await _coursesRepository.UpdateCourseAsync(course);
    }

    #endregion

    #region ToggleCourseField

    public async Task<bool> ToggleCourseFieldAsync(int courseId, bool isChecked)
    {
        Course? course = await _coursesRepository.GetCourseByIdAsync(courseId);
        if (course == null) return false;

        course.IsOpen = isChecked;

        return await _coursesRepository.UpdateCourseAsync(course);
    }

    #endregion

    #region EnrollStudentInCourse

    public async Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId)
    {
        CourseStudentMapping courseStudentMapping = new()
        {
            CourseId = courseId,
            UserId = studentId,
            IsCompleted = false
        };
        return await _courseStudentMappingRepository.AddCourseStudentMappingAsync(courseStudentMapping);
    }

    #endregion

    #region IsStudentEnrolledInCourse

    public async Task<bool> IsStudentEnrolledInCourseAsync(int userId, int courseId)
    {
        CourseStudentMapping? courseStudentMappings = await _courseStudentMappingRepository
                                            .GetCourseStudentMappingsAsQueryable()
                                                .FirstOrDefaultAsync(csm => csm.UserId == userId
                                                    && csm.CourseId == courseId);
        return courseStudentMappings != null;
    }

    #endregion

    #region MarkCourseAsComplete

    public async Task<bool> MarkCourseAsCompleteAsync(int userId, int courseId)
    {
        CourseStudentMapping? courseStudentMapping = await _courseStudentMappingRepository
                                                                .GetCourseStudentMappingsAsQueryable()
                                                                    .FirstOrDefaultAsync(csm => csm.UserId == userId
                                                                        && csm.CourseId == courseId);

        if (courseStudentMapping != null)
        {
            courseStudentMapping.IsCompleted = !courseStudentMapping.IsCompleted;
            return await _courseStudentMappingRepository.SaveChangesAsync() > 0;
        }

        return false;
    }

    #endregion
}
