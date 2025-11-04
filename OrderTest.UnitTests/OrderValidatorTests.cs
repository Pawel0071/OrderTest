using FluentAssertions;
using OrderTest.Infrastructure;
using OrderTest.Interfaces;

namespace OrderTest.UnitTests;

public class OrderValidatorTests
{
    private readonly IOrderValidator _validator = new OrderValidator();

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(999)]
    public void IsValidId_ShouldReturnTrue_ForPositiveIds(int id)
    {
        // Arrange done in constructor

        // Act
        var result = _validator.IsValidId(id);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void IsValidId_ShouldReturnFalse_ForZeroOrNegativeIds(int id)
    {
        // Act
        var result = _validator.IsValidId(id);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("Laptop")]
    [InlineData("Monitor 27 cali")]
    [InlineData("Zamówienie testowe")]
    public void IsValidDescription_ShouldReturnTrue_ForValidDescriptions(string description)
    {
        // Act
        var result = _validator.IsValidDescription(description);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void IsValidDescription_ShouldReturnFalse_ForNullOrWhitespace(string? description)
    {
        // Act
        var result = _validator.IsValidDescription(description);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidDescription_ShouldReturnFalse_WhenDescriptionExceeds200Characters()
    {
        // Arrange
        var longDescription = new string('x', 201);

        // Act
        var result = _validator.IsValidDescription(longDescription);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValidDescription_ShouldReturnTrue_WhenDescriptionIsExactly200Characters()
    {
        // Arrange
        var exactDescription = new string('x', 200);

        // Act
        var result = _validator.IsValidDescription(exactDescription);

        // Assert
        result.Should().BeTrue();
    }
}