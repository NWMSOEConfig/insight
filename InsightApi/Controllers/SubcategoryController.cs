using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsightApi.Models;

namespace InsightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly SubcategoryContext _context;

        public SubcategoryController(SubcategoryContext context)
        {
            _context = context;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subcategory>>> GetSubcategories()
        {
            if (_context.Subcategories == null)
            {
                return NotFound();
            }
            return await _context.Subcategories.ToListAsync();
        }

        // POST: api/category
        [HttpPost]
        public async Task<ActionResult<Subcategory>> PostSubcategory(Subcategory subcategory)
        {
            if (_context.Subcategories == null)
            {
                return Problem("Entity set 'TodoContext.TodoItems'  is null.");
            }
            _context.Subcategories.Add(subcategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = subcategory.Id }, subcategory);
        }
    }
}
