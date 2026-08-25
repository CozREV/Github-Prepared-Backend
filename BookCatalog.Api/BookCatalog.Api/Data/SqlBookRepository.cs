using Dapper;
using Microsoft.Data.SqlClient;
using BookCatalog.Api.Models;

namespace BookCatalog.Api.Data;

public class SqlBookRepository : IBookRepository
{
    private readonly string _connectionString;

    public SqlBookRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BookCatalog")
            ?? throw new InvalidOperationException("Connection string mangler.");
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Book>("SELECT * FROM Books");
    }

    public async Task<Book?> FindAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "SELECT * FROM Books WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<Book>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Book>> FindByAuthorAsync(string author)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "SELECT * FROM Books WHERE Author = @Author";
        return await connection.QueryAsync<Book>(sql, new { Author = author });
    }
}