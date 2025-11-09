using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BooksService _service;
    public BooksController(BooksService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> Get() =>
        await _service.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> Get(string id)
    {
        var book = await _service.GetAsync(id);
        return book is null ? NotFound() : book;
    }

    [HttpPost]
    public async Task<ActionResult<Book>> Post(Book book)
    {
        await _service.CreateAsync(book);
        return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Put(string id, Book book)
    {
        var exists = await _service.GetAsync(id);
        if (exists is null) return NotFound();
        book.Id = id;
        await _service.UpdateAsync(id, book);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var exists = await _service.GetAsync(id);
        if (exists is null) return NotFound();
        await _service.RemoveAsync(id);
        return NoContent();
    }
}
