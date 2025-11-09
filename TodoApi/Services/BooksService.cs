using MongoDB.Driver;
using TodoApi.Models;

namespace TodoApi.Services;

public class BooksService
{
    private readonly IMongoCollection<Book> _books;

    public BooksService(IConfiguration config)
    {
        var s = config.GetSection("BookStoreDatabase").Get<BookStoreDatabaseSettings>()
                ?? throw new InvalidOperationException("Missing BookStoreDatabase settings.");
        var client = new MongoClient(s.ConnectionString);
        var db = client.GetDatabase(s.DatabaseName);
        _books = db.GetCollection<Book>(s.BooksCollectionName);
    }

    public async Task<List<Book>> GetAsync() =>
        await _books.Find(_ => true).ToListAsync();

    public async Task<Book?> GetAsync(string id) =>
        await _books.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Book newBook) =>
        await _books.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Book updatedBook) =>
        await _books.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _books.DeleteOneAsync(x => x.Id == id);
}
