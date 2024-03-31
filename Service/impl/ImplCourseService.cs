using System.Collections;
using course_edu_api.Data;
using course_edu_api.Data.RequestModels;
using course_edu_api.Data.ResponseModels;
using course_edu_api.Entities;
using course_edu_api.Entities.Enum;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
namespace course_edu_api.Service.impl;

public class ImplCourseService : ICourseService
{
    private readonly DataContext _context;

    public ImplCourseService(DataContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetCourseById(long id)
    {
        return await _context.Courses.Include(c => c.CategoryCourse)
            .Include(c => c.GroupLessons.OrderBy(g => g.Index))
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Course>> GetAllCourses(int page, int size)
    {
        var skipCount = ((page + 1) - 1) * size;
        return await _context.Courses
            .Include(c => c.CategoryCourse)
            .Skip(skipCount)
            .Take(size)
            .ToListAsync();
    }
    
    /// <summary>
    /// Pagination request
    /// </summary>
    /// <param name="paginationRequestDto"></param>
    /// <returns></returns>
    public async Task<PaginatedResponse<Course>> GetCoursePagination(PaginationRequestDto<CourseQueryDto> paginationRequestDto)
    {
        var skip = (paginationRequestDto.PageNumber - 1) * paginationRequestDto.PageSize;
        var take = paginationRequestDto.PageSize;
        var courseQuery = paginationRequestDto.Where;
        IQueryable<Course> query = _context.Courses;
        
        if (courseQuery.CategoryId != null)
        {
            query = query.Where(p => p.CategoryCourse.Id == courseQuery.CategoryId);
        }
        if (!String.IsNullOrEmpty(courseQuery.Query))
        {
            query = query.Where(p => p.Title.Contains(courseQuery.Query) || p.Description.Contains(courseQuery.Query) || p.SubTitle.Contains(courseQuery.Query));
        }

        if (courseQuery.Status != null)
        {
            var statusValue = (CourseStatus)courseQuery.Status;
            query = query.Where(p => p.Status == statusValue);  
        }
        
        if (courseQuery.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= courseQuery.MinPrice);
        }
        if (courseQuery.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= courseQuery.MaxPrice);
        }
        
        var paginatedData = await query.Include(p => p.CategoryCourse).Skip(skip).Take(take).ToListAsync();

        return new PaginatedResponse<Course>
        {
            Data = paginatedData,
            PageNumber = paginationRequestDto.PageNumber,
            PageSize = paginationRequestDto.PageSize,
            TotalItems = paginatedData.Count
        };
    }

    public async Task<Course?> CreateCourse(Course? newCourse)
    {
        _context.Courses.Add(newCourse);
        await _context.SaveChangesAsync();
        return newCourse;
    }

    public async Task<Course> UpdateCourse(long id, CourseRequest updatedCourse)
    {
        var categoryCourse = await _context.CategoriesCourse.FindAsync(updatedCourse.CategoryId);
        var existingCourse = await _context.Courses.FindAsync(id);
        if (existingCourse == null || categoryCourse == null)
            throw new InvalidOperationException("Course | Category course not found.");

        existingCourse.Title = updatedCourse.Title;
        existingCourse.Description = updatedCourse.Description;
        existingCourse.Price = updatedCourse.Price;
        existingCourse.CategoryCourse = categoryCourse;
        existingCourse.Status = (CourseStatus)updatedCourse.Status!;
        existingCourse.Target = updatedCourse.Target;
        existingCourse.RequireSkill = updatedCourse.RequireSkill;
        existingCourse.AdviseVideo = updatedCourse.AdviseVideo;
        existingCourse.SubTitle = updatedCourse.SubTitle;
        existingCourse.Thumbnail = updatedCourse.Thumbnail;

        await _context.SaveChangesAsync();
        return existingCourse;
    }

    public async Task<bool> DeleteCourse(long id)
    {
        var courseToDelete = await _context.Courses.FindAsync(id);
        if (courseToDelete == null)
            return false;

        _context.Courses.Remove(courseToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<long> CountTotalCourse(CourseQueryDto courseQueryDto)
    {
        IQueryable<Course> query = _context.Courses;
        if (courseQueryDto.CategoryId != null)
        {
            query = query.Where(p => p.CategoryCourse.Id == courseQueryDto.CategoryId);
        }
        if (!String.IsNullOrEmpty(courseQueryDto.Query))
        {
            query = query.Where(p => p.Title.Contains(courseQueryDto.Query) || p.Description.Contains(courseQueryDto.Query) || p.SubTitle.Contains(courseQueryDto.Query));
        }

        if (courseQueryDto.Status != null)
        {
            var statusValue = (CourseStatus)courseQueryDto.Status;
            query = query.Where(p => p.Status == statusValue);  
        }
        
        if (courseQueryDto.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= courseQueryDto.MinPrice);
        }
        if (courseQueryDto.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= courseQueryDto.MaxPrice);
        }
        
        return await query.CountAsync();
    }

    public async Task<GroupLesson> AddGroupLesson(long courseId, GroupLesson groupLesson)
    {
        var existCourse = await _context.Courses.FindAsync(courseId);
        if (existCourse == null)
        {
            throw new InvalidOperationException("Không tìm thấy khóa học bằng id");
        }

        var exitsGroupLesson = existCourse.GroupLessons?.FirstOrDefault(x => x.Title == groupLesson.Title);
        if (exitsGroupLesson != null)
        {
            throw new InvalidOperationException("Lỗi: Nhóm bài học đã tồn tại trong khóa học này");
        }

        existCourse.GroupLessons ??= new List<GroupLesson>();
        existCourse.GroupLessons.Add(groupLesson);

        await _context.SaveChangesAsync();

        return groupLesson;
    }

    

    public async Task<GroupLesson> UpdateGroupLesson(long groupLessonId, GroupLesson updateGroupLesson)
    {
        var existingGroupLesson = await _context.GroupLessons.FindAsync(groupLessonId);
    
        if (existingGroupLesson == null)
        {
            throw new InvalidOperationException("Không tìm thấy nhóm bài học bằng ID");
        }

        existingGroupLesson.Title = updateGroupLesson.Title;
        existingGroupLesson.Index = updateGroupLesson.Index;
        await _context.SaveChangesAsync();

        return existingGroupLesson;
    }
    
    public async Task<bool> RemoveGroupLesson(long groupLessonId)
    {
        var groupLessonToRemove = await _context.GroupLessons.FindAsync(groupLessonId);

        if (groupLessonToRemove == null)
        {
            return false;
        }

        _context.GroupLessons.Remove(groupLessonToRemove);
        await _context.SaveChangesAsync();
        return true;
    }
}