using BookStore.Pages.Books;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
    public void OnPost_WithValidBook_AddsBookToDatabase()
    {
        var context = GetDbContext();
        var model = new CreateModel(context);

        var author = new Author { Name = "Тестовый автор" };
        context.Authors.Add(author);
        context.SaveChanges();

        model.Book = new Book
        {
            Title = "Новая книга",
            AuthorID = author.Id,
            Price = 300,
            Quantity = 7
        };
        var result = model.OnPost();

        Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal(1, context.Books.Count());
        Assert.Equal("Новая книга", context.Books.First().Title);
    }

    [Fact]
    public void OnPost_WithInvalidBook_ReturnsPage()
    {
        var context = GetDbContext();
        var model = new CreateModel(context);

        model.Book = new Book { Title = "", Price = 300, Quantity = 7 };
        model.ModelState.AddModelError("Book.Title", "Название обязательно");

        var result = model.OnPost();

        Assert.IsType<PageResult>(result);
        Assert.Equal(0, context.Books.Count());
    }
}