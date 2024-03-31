using course_edu_api.Data;
using course_edu_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace course_edu_api.Service.impl;

public class ImplCategoryService : ICategoryService
{
    private readonly DataContext _context;

    public ImplCategoryService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryCourse>> GetAllCategories()
    {
        return await _context.CategoriesCourse.ToListAsync();
    }

    public async Task<CategoryCourse> GetCategoryById(long id)
    {
        return await _context.CategoriesCourse.FindAsync(id);
    }

    public async Task<CategoryCourse> CreateCategory(CategoryCourse category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        _context.CategoriesCourse.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<CategoryCourse> UpdateCategory(long id, CategoryCourse updatedCategory)
    {
        var existingCategory = await _context.CategoriesCourse.FindAsync(id);
        if (existingCategory == null)
        {
            throw new InvalidOperationException("Category not found.");
        }

        existingCategory.CategoryName = updatedCategory.CategoryName;
        existingCategory.IsLock = updatedCategory.IsLock;

        await _context.SaveChangesAsync();
        return existingCategory;
    }

    public async Task<bool> DeleteCategory(long id)
    {
        var existingCategory = await _context.CategoriesCourse.FindAsync(id);
        if (existingCategory == null)
        {
            return false;
        }

        _context.CategoriesCourse.Remove(existingCategory);
        await _context.SaveChangesAsync();
        return true;
    }
}