using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using course_edu_api.Entities;
using course_edu_api.Service;

namespace course_edu_api.Controllers
{
    [Route("api/category-course")]
    [ApiController]
    public class CategoryCourseController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryCourseController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryCourse>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryCourse>> GetCategory(long id)
        {
            var category = await _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryCourse>> CreateCategory(CategoryCourse categoryCourse)
        {
            var createdCategory = await _categoryService.CreateCategory(categoryCourse);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(long id, CategoryCourse updatedCategoryCourse)
        {
            if (id != updatedCategoryCourse.Id)
            {
                return BadRequest();
            }

            var updatedCategory = await _categoryService.UpdateCategory(id, updatedCategoryCourse);
            if (updatedCategory == null)
            {
                return NotFound();
            }

            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            var success = await _categoryService.DeleteCategory(id);
            if (!success)
            {
                return NotFound();
            }

            return Ok(true);
        }
    }
}
