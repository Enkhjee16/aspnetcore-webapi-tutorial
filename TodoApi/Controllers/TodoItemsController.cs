using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;
    public TodoItemsController(TodoContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll() =>
        await _context.TodoItems.AsNoTracking().ToListAsync();

    [HttpGet("{id:long}")]
    public async Task<ActionResult<TodoItem>> Get(long id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        return item is null ? NotFound() : item;
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(TodoItem item)
    {
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, TodoItem item)
    {
        if (id != item.Id) return BadRequest();
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item is null) return NotFound();
        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
