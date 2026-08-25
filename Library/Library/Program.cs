using Library;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBookRepository, FileBookRepository>();
builder.Services.AddScoped<BookLoanService>();

var app = builder.Build();

app.MapPost("/loans", (
    BorrowBookDto request,
    BookLoanService service) =>
{
    try
    {
        service.BorrowBook(request.bookId, request.userName);
        return Results.Ok();
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
});

app.Run();

public class BorrowBookDto
{
    public int bookId { get; set; }
    public string userName { get; set; } = "";
}

