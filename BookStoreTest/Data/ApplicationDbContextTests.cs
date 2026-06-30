using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Model;

namespace BookStore.Tests.Data;

public class ApplicationDbContextTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public void CanAddBookToDatabase()
    {
        var context = GetDbContext();
        var book = new Book { Title = "Тестовая книга", Price = 100, Quantity = 5 };

        context.Books.Add(book);
        context.SaveChanges();

        Assert.Equal(1, context.Books.Count());
    }

    [Fact]
    public void CanAddBuyerToDatabase()
    {
        var context = GetDbContext();
        var buyer = new Buyer { LastName = "Тестовый", Email = "test@test.com" };

        context.Buyers.Add(buyer);
        context.SaveChanges();

        Assert.Equal(1, context.Buyers.Count());
    }

    [Fact]
    public void CanAddAuthorToDatabase()
    {
        var context = GetDbContext();
        var author = new Author { Name = "Толстой" };

        context.Authors.Add(author);
        context.SaveChanges();

        Assert.Equal(1, context.Authors.Count());
    }
}