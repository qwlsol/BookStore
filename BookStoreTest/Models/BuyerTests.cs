using System.ComponentModel.DataAnnotations;
using BookStore.Model;

namespace BookStore.Tests.Models;

public class BuyerTests
{
    [Fact]
    public void Buyer_LastNameIsRequired_ReturnsValidationError()
    {
        var buyer = new Buyer { LastName = "" }; 
        var context = new ValidationContext(buyer);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(buyer, context, results, true);

        Assert.False(isValid);
    }

    [Fact]
    public void Buyer_ValidEmail_PassesValidation()
    {
        var buyer = new Buyer { LastName = "Петров", Email = "petrov@example.com" };
        var context = new ValidationContext(buyer);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(buyer, context, results, true);

        Assert.True(isValid);
    }
}