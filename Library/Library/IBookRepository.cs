namespace Library
{
    public interface IBookRepository
    {
        List<Book> GetAll();
        void Save(List<Book> books);
    }
}
