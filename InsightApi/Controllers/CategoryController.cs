using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsightApi.Models;
using AutoMapper;

namespace InsightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryContext _context;

        public CategoryController(CategoryContext context)
        {
            _context = context;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var categories = await _context.Categories.ToListAsync();
            var subcategories = from c in categories
                                select new Subcategory()
                                {
                                    Id = c.Id,
                                    Name = c.Name
                                };

            return Ok(categories);
        }

        // POST: api/category
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'TodoContext.TodoItems'  is null.");
            }
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = category.Id }, category);
        }
    }
}
