using FluentAssertions;
using OrderTest.Infrastructure;
using OrderTest.Interfaces;

namespace OrderTest.UnitTests
{
    public class OrderValidatorTests
    {
        private readonly IOrderValidator _validator = new OrderValidator();

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(999)]
        public void IsValidId_ShouldReturnTrue_ForPositiveIds(int id)
        {
            var result = _validator.IsValidId(id);
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void IsValidId_ShouldReturnFalse_ForZeroOrNegativeIds(int id)
        {
            var result = _validator.IsValidId(id);
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("Laptop")]
        [InlineData("Monitor 27 cali")]
        [InlineData("Zamówienie testowe")]
        public void IsValidDescription_ShouldReturnTrue_ForValidDescriptions(string description)
        {
            var result = _validator.IsValidDescription(description);
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void IsValidDescription_ShouldReturnFalse_ForNullOrWhitespace(string? description)
        {
            var result = _validator.IsValidDescription(description);
            result.Should().BeFalse();
        }

        [Fact]
        public void IsValidDescription_ShouldReturnFalse_WhenDescriptionExceeds200Characters()
        {
            var longDescription = new string('x', 201);
            var result = _validator.IsValidDescription(longDescription);
            result.Should().BeFalse();
        }

        [Fact]
        public void IsValidDescription_ShouldReturnTrue_WhenDescriptionIsExactly200Characters()
        {
            var exactDescription = new string('x', 200);
            var result = _validator.IsValidDescription(exactDescription);
            result.Should().BeTrue();
        }
    }
}