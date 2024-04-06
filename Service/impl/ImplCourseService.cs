using System.Collections;
using System.Diagnostics;
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
        return await _context.Courses
            .Include(c => c.CategoryCourse)
            .Include(c => c.GroupLessons.OrderBy(g => g.Index))
            .ThenInclude(g => g.Lessons.OrderBy(l => l.Index))
            .ThenInclude(g => g.Quiz.OrderBy(q => q.Index))
            .Include(c => c.GroupLessons.OrderBy(g => g.Index))
            .ThenInclude(g => g.Lessons)
            .ThenInclude(g => g.Post)
            .Include(c => c.GroupLessons.OrderBy(g => g.Index))
            .ThenInclude(g => g.Lessons)
            .ThenInclude(g => g.Video)
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

    public async Task<Lesson> AddNewLesson(long groupLessonId, long courseId, Lesson newLesson)
    {

        var exitsGroupLesson = await _context.GroupLessons?.Include(gr => gr.Lessons).FirstOrDefaultAsync(x => x.Id == groupLessonId);
        if (exitsGroupLesson == null )
        {
            throw new InvalidOperationException("Lỗi: Không tìm thấy nhóm bài học!");
        }

        var lessonExist = exitsGroupLesson.Lessons.FirstOrDefault(lesson => lesson.Title == newLesson.Title);

        if (lessonExist != null)
        {
            throw new InvalidOperationException("Lỗi: Bài hoc đã có sẵn!");
        }
        
        
        exitsGroupLesson.Lessons ??= new List<Lesson>();
        exitsGroupLesson.Lessons.Add(newLesson);

        await _context.SaveChangesAsync();

        return newLesson;
    }

    public async Task<Lesson> UpdateLesson(long lessonId, Lesson updateLesson)
    {
        var existLesson = await _context.Lessons.FindAsync(lessonId);
    
        if (existLesson == null)
        {
            throw new InvalidOperationException("Không tìm thấy bài học bằng ID");
        }

        existLesson.Title = updateLesson.Title;
        existLesson.Index = updateLesson.Index;
        existLesson.Description = updateLesson.Description;
        existLesson.Post = updateLesson.Post;
        existLesson.Video = updateLesson.Video;
        existLesson.Quiz = updateLesson.Quiz;
        await _context.SaveChangesAsync();

        return existLesson;
    }

    public async Task<bool> RemoveLesson(long lessonId)
    {
        var lessonToRemove = await _context.Lessons
            .Include(l => l.Quiz)
            .Include(l => l.Video)
            .Include(l => l.Post)
            .FirstOrDefaultAsync(lesson => lesson.Id == lessonId);

        if (lessonToRemove == null)
        {
            return false;
        }

        if (lessonToRemove.Quiz != null)
        {
              _context.Question.RemoveRange(lessonToRemove.Quiz);
        }
        if (lessonToRemove.Video != null)  _context.VideoLesson.Remove(lessonToRemove.Video);
        if (lessonToRemove.Post != null)  _context.PostLesson.Remove(lessonToRemove.Post);
        await _context.SaveChangesAsync();
        _context.Lessons.Remove(lessonToRemove);
        await _context.SaveChangesAsync();
        return true;
    }

    public async  Task<UserCourse> RegisterCourse(RegisterCourseRequestDto registerCourseRequestDto)
    {
        var userCourseExist = await _context.UserCourse
            .Include(u=> u.Course)
            .ThenInclude(uc => uc.GroupLessons)
            .ThenInclude(ucc => ucc.Lessons)
            .FirstOrDefaultAsync(u =>
            u.User.Id == registerCourseRequestDto.UserId && u.Course.Id == registerCourseRequestDto.CourseId);
        if (userCourseExist != null) throw new Exception("Ngươời dùng đã đăng ký khóa học rồi!");
        
        
        var userExist = await _context.Users.FindAsync(registerCourseRequestDto.UserId);
        var courseExist = await _context.Courses.FindAsync(registerCourseRequestDto.CourseId);
    
        if (userExist == null || courseExist == null) throw new Exception("User hoặc khóa học không tồn tại!");

        var userCourse = new UserCourse();
        userCourse.Course = courseExist;
        userCourse.User = userExist;
        if (courseExist.GroupLessons!.Count > 0)
        {
            var firstGroupLesson = courseExist.GroupLessons[0];
            if (firstGroupLesson.Lessons.Count > 0)
            {
                var firstLesson = firstGroupLesson.Lessons[0];            
                userCourse.CurrentLesson = firstLesson;
            }
        }

        _context.UserCourse.Add(userCourse);
        await _context.SaveChangesAsync();

        return userCourse;
    }

    public Task<UserCourse> PaymentCourse(RegisterCourseRequestDto registerCourseRequestDto)
    {
        throw new NotImplementedException();
    }


    public async Task<bool> CheckUserCourseExist(long userId, long courseId)
    {
       var res =   await _context.UserCourse.Where(u => u.User.Id == userId && u.Course.Id == courseId).ToListAsync();
       if (res.Count > 0)
       {
           return true;
       }

       return false;

    }

    public async Task<UserCourse> GetUserCourse(long userId, long courseId)
    {
        var userCourse =
            await _context.UserCourse
                .Include(u => u.CurrentLesson)
                .ThenInclude(uc => uc.Quiz)
                .Include(uc => uc.CurrentLesson)
                .ThenInclude(uc => uc.Video)
                .Include(uc => uc.CurrentLesson)
                .ThenInclude(uc => uc.Post)
                .ThenInclude(ucc => ucc.items)
                .FirstOrDefaultAsync(course =>
                course.User.Id == userId && course.Course.Id == courseId);
        if (userCourse == null) throw new Exception("Không tìm thấy dữ liệu!");

        return userCourse;
    }
    
    public async Task<UserCourse> ChangeCurrentProcessCourse(ChangeCurrentProcessCourseRequestDto changeCurrentProcessCourseRequestDto)
    {
        var userCourseExist = await _context.UserCourse
            .Include(uc => uc.CurrentLesson)
            .ThenInclude(uc => uc.Post)
            .ThenInclude(ucc => ucc.items)
            .Include(uc => uc.CurrentLesson)
            .ThenInclude(uc => uc.Video)
            .Include(uc => uc.CurrentLesson)
            .ThenInclude(uc => uc.Quiz)
            .Include(uc => uc.LessonPassed)
            .FirstOrDefaultAsync(uc => uc.Course.Id == changeCurrentProcessCourseRequestDto.CourseId);

        if (userCourseExist == null)
        {
            throw new ApplicationException("Course not found"); // Use a more specific exception type
        }

        var lessonExist = await _context.Lessons.Include(uc => uc.Post)
            .ThenInclude(ucc => ucc.items)
            .Include(uc => uc.Video)
            .Include(uc => uc.Quiz)
            .FirstOrDefaultAsync(uc => uc.Id == changeCurrentProcessCourseRequestDto.LessonId);

        if (lessonExist == null )
        {
            throw new ApplicationException("Lesson or group lesson not found"); // Same exception type for consistency
        }

        // Update userCourseExist with retrieved entities
        userCourseExist.CurrentLesson = lessonExist;

        // Ensure lesson is added to LessonPassed only once
        if (!userCourseExist.LessonPassed.Contains(lessonExist))
        {
            userCourseExist.LessonPassed.Add(lessonExist);
        }

        await _context.SaveChangesAsync();
        return userCourseExist;
    }

}