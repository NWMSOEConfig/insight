using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsightApi.Models;

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
            return await _context.Categories.ToListAsync();
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
