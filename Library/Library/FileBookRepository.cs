using System.Text.Json;

namespace Library
{
    public class FileBookRepository : IBookRepository
    {
        private const string FilePath = "books.json";

        public List<Book> GetAll()
        {
            var json = File.ReadAllText(FilePath);
            var books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
            return books;
        }

        public void Save(List<Book> books)
        {
            var json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
