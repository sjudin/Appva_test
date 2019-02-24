using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Appva_test.Models;

namespace Appva_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
            // Put some test data into database if empty
            if(_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Walk dog", Desc = "Take dog to a walk in Slottsskogen", IsDone = false });
                _context.SaveChanges();
            }
        }

        // GET api/todo
        // Return list of all TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET api/todo/{id}
        // Get specific TodoItem with id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if(item == null)
              return NotFound();
            
            return item;
        }

        // POST api/todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.TodoItemId }, item);
        }

        // PUT api/todo/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TodoItem>> PutTodoItem(int id, TodoItem item)
        {
            if(id != item.TodoItemId)
                return BadRequest();

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/todo/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if(item == null)
                return NotFound();

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
