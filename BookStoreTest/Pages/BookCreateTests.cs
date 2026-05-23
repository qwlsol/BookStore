using BookStore.Data;
using BookStore.Hubs;
using BookStore.Model;
using BookStore.Pages.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BookStore.Tests.Pages;

public class BooksCreateTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public void CreateModel_Exists()
    {
        var model = new CreateModel(null!);
        Assert.NotNull(model);
    }

    [Fact]
    public async Task OnPost_WithValidBook_AddsBookToDatabase()
    {
        var context = GetDbContext();

        // Создаем Mock для IHubContext<BookHub>
        var mockHubContext = new Mock<IHubContext<BookHub>>();
        var mockClients = new Mock<IHubClients>();
        var mockClientProxy = new Mock<IClientProxy>();

        mockHubContext.Setup(x => x.Clients).Returns(mockClients.Object);
        mockClients.Setup(x => x.All).Returns(mockClientProxy.Object);

        var model = new CreateModel(context, mockHubContext.Object);

        var author = new Author
        {
            Name = "Тестовый автор"
        };
        context.Authors.Add(author);
        context.SaveChanges();

        model.Book = new Book
        {
            Title = "Новая книга",
            AuthorID = author.Id,
            Price = 300,
            Quantity = 7,
            Author = null
        };
        var result = await model.OnPostAsync();

        Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal(1, context.Books.Count());

        var savedBook = context.Books.First();
        Assert.Equal("Новая книга", savedBook.Title);
        Assert.Equal(author.Id, savedBook.AuthorID);
    }

    [Fact]
    public async Task OnPost_WithInvalidBook_ReturnsPage()
    {
        var context = GetDbContext();
        var model = new CreateModel(context);

        model.Book = new Book { Title = "", Price = 300, Quantity = 7 };
        model.ModelState.AddModelError("Book.Title", "Название обязательно");

        var result = await model.OnPostAsync();

        Assert.IsType<PageResult>(result);
        Assert.Equal(0, context.Books.Count());

    } 
}
