// Program.cs
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Bind fixed URLs (dev)
builder.WebHost.UseUrls("http://localhost:7144", "https://localhost:7145");

// Controllers
builder.Services.AddControllers();

// EF Core InMemory (Task 1)
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

// MongoDB (Task 2)
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));
builder.Services.AddSingleton<BooksService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (allow all for local dev)
builder.Services.AddCors(o => o.AddPolicy("DevAll", p => p
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();

// Always enable Swagger for this tutorial
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("DevAll");
app.UseHttpsRedirection();

app.MapControllers();
app.Run();
