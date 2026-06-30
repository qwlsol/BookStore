using System.ComponentModel.DataAnnotations;
using BookStore.Model;

namespace BookStore.Tests.Models;

public class BookTests
{
    [Fact]
    public void Book_TitleIsRequired_ReturnsValidationError()
    {
        var book = new Book { Title = "" };
        var context = new ValidationContext(book);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(book, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.ErrorMessage?.Contains("Требуется ввести название") == true);
    }

    [Fact]
    public void Book_PriceMustBePositive_ReturnsValidationError()
    {
        var book = new Book { Title = "Test Book", Price = 0 };
        var context = new ValidationContext(book);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(book, context, results, true);

        Assert.False(isValid);
    }

    [Fact]
    public void Book_ValidData_PassesValidation()
    {
        var book = new Book
        {
            Title = "Война и мир",
            Price = 500,
            Quantity = 10,
            AuthorID = 1
        };
        var context = new ValidationContext(book);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(book, context, results, true);

        Assert.True(isValid);
    }
}