// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using TodoApi.Models;

// namespace InsightApi.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class TodoItemsController : ControllerBase
//     {
//         private readonly TodoContext _context;

//         public TodoItemsController(TodoContext context)
//         {
//             _context = context;
//         }

//         // GET: api/TodoItems
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
//         {
//             if (_context.TodoItems == null)
//             {
//                 return NotFound();
//             }
//             return await _context.TodoItems.ToListAsync();
//         }

//         // POST: api/TodoItems
//         [HttpPost]
//         public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
//         {
//             if (_context.TodoItems == null)
//             {
//                 return Problem("Entity set 'TodoContext.TodoItems'  is null.");
//             }
//             _context.TodoItems.Add(todoItem);
//             await _context.SaveChangesAsync();

//             return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
//         }
//     }
// }
