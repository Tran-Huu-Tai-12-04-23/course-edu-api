using course_edu_api.Entities;

namespace course_edu_api.Service;

public interface ICategoryService
{
    Task<List<CategoryCourse>> GetAllCategories();
    Task<CategoryCourse> GetCategoryById(long id);
    Task<CategoryCourse> CreateCategory(CategoryCourse category);
    Task<CategoryCourse> UpdateCategory(long id, CategoryCourse updatedCategory);
    Task<bool> DeleteCategory(long id);
}