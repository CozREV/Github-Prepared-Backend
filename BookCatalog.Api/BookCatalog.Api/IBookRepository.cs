using BookCatalog.Api.Models;

namespace BookCatalog.Api.Data;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();

    Task<Book?> FindAsync(int id);

    Task<IEnumerable<Book>> FindByAuthorAsync(
        string author);
}