using BookStore.Pages.Books;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Model;

namespace BookStore.Tests.Pages;

public class BooksIndexTests
{
    [Fact]
    public void IndexModel_OnGet_LoadsBooksFromDatabase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var author = new Author { Name = "Тестовый автор" };
        context.Authors.Add(author);
        context.SaveChanges();

        context.Books.Add(new Book
        {
            Title = "Книга 1",
            AuthorID = author.Id,
            Price = 100,
            Quantity = 5
        });

        context.Books.Add(new Book
        {
            Title = "Книга 2",
            AuthorID = author.Id,
            Price = 200,
            Quantity = 3
        });

        context.SaveChanges();

        var model = new IndexModel(context);
        model.OnGet();

        Assert.Equal(2, model.Books.Count);
    }

    [Fact]
    public void IndexModel_Exists_AndHasBooksProperty()
    {
        var model = new IndexModel(null!);

        Assert.NotNull(model);
        Assert.IsType<IndexModel>(model);
    }
}