using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

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
            if(_context.DateItems.Count() == 0)
                _context.DateItems.Add(new DateItem  {DueDate = DateTime.Today });
            if(_context.TodoItems.Count() == 0)
                _context.TodoItems.Add(new TodoItem { Name = "Walk dog", Desc = "Take dog on a walk in Slottsskogen", IsDone = false});
            _context.SaveChanges();
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
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item, int? dateItemId)
        {
            // If dateItemId is specified, add add TodoItem to DateItems.TodoItems
            var dateitem = await _context.DateItems.FindAsync(dateItemId);
            if(dateitem != null)
              dateitem.TodoItems.Add(item);

            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.TodoItemId }, item);
        }

        // PUT api/todo/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TodoItem>> PutTodoItem(int id, TodoItem item, int? dateItemId)
        {
            if(id != item.TodoItemId)
                return BadRequest();

            // If PUT request also has id of DateItem, add TodoItem to that DateItems' list
            if (dateItemId != null)
            {
              DateItem date = await _context.DateItems.FindAsync(dateItemId);
              date.TodoItems.Add(item);
            }

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


        // GET api/todo/day
        // Return all DateItems in Db 
        [HttpGet("day")]
        public async Task<ActionResult<IEnumerable<DateItem>>> GetDateItems()
        {
            return await _context.DateItems.ToListAsync();
        }

        // GET api/todo/day/{id}
        // Return all TodoItems related to a given day
        [HttpGet("day/{id:int}")]
        public async Task<ActionResult<DateItem>> GetDateItem(int id)
        { 
            return await _context.DateItems.Include(list => list.TodoItems).Where(todo => todo.DateItemId == id).SingleAsync();
        }

        // POST api/todo/day
        // Create new DateItem resource
        [HttpPost("day")]
        public async Task<ActionResult<DateItem>> PostDateItems(DateItem item)
        {
            _context.DateItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDateItem), new { id = item.DateItemId }, item);
        }

        // DELETE api/todo/day/{id}
        // Note: Deletes related TodoItems using Cascade
        [HttpDelete("day/{id:int}")]
        public async Task<ActionResult<DateItem>> DeleteDateItem(int id)
        {
            DateItem item = _context.DateItems.Include(list => list.TodoItems).Where(todo => todo.DateItemId == id).Single();
            if(item == null)
              return NotFound();

            // Cascading remove policy
            _context.TodoItems.RemoveRange(item.TodoItems);
            _context.DateItems.Remove(item);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT api/todo/day/{id}
        [HttpPut("day/{id:int}")]
        public async Task<ActionResult<DateItem>> PutDateItem(int id, DateItem item)
        {
            if(id != item.DateItemId)
               return BadRequest();

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}

