using Library;

public class BookLoanService
{

    private readonly IBookRepository _bookRepository;

    public BookLoanService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public void BorrowBook(
        int bookId,
        string userName)
    {

        var books = _bookRepository.GetAll();

        var book =
            books.FirstOrDefault(
                book => book.Id == bookId);

        if (book == null)
        {
            throw new Exception(
                "Boka finnes ikke.");
        }

        if (book.BorrowedBy != null)
        {
            throw new Exception(
                "Boka er allerede utlånt.");
        }

        book.BorrowedBy = userName;

        _bookRepository.Save(books);
    }
}
