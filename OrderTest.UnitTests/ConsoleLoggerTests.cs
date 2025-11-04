using FluentAssertions;
using OrderTest.Infrastructure;

namespace OrderTest.UnitTests;

public class ConsoleLoggerTests
{
    [Theory]
    [InlineData("Information", "[INFO]")]
    [InlineData("Error", "[ERROR]")]
    public void LogInfo_ShouldWriteToConsole_WhenLevelIsEnabled(string configuredLevel, string expectedPrefix)
    {
        // Arrange
        var logger = new ConsoleLogger(configuredLevel);
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        logger.LogInfo("Test info message");

        // Assert
        var consoleOutput = output.ToString();
        if (configuredLevel == "Information")
        {
            consoleOutput.Should().Contain(expectedPrefix);
            consoleOutput.Should().Contain("Test info message");
        }
        else
        {
            consoleOutput.Should().BeEmpty();
        }
    }

    [Theory]
    [InlineData("Error", "[ERROR]")]
    [InlineData("Critical", "")] // Should not log unless level is Error or lower
    public void LogError_ShouldWriteToConsole_WhenLevelIsEnabled(string configuredLevel, string expectedPrefix)
    {
        // Arrange
        var logger = new ConsoleLogger(configuredLevel);
        var output = new StringWriter();
        Console.SetOut(output);

        // Act
        logger.LogError("Test error message");

        // Assert
        var consoleOutput = output.ToString();
        if (configuredLevel == "Error")
        {
            consoleOutput.Should().Contain(expectedPrefix);
            consoleOutput.Should().Contain("Test error message");
        }
        else
        {
            consoleOutput.Should().BeEmpty();
        }
    }

    [Fact]
    public void LogError_ShouldIncludeExceptionDetails_WhenExceptionProvided()
    {
        // Arrange
        var logger = new ConsoleLogger("Error");
        var output = new StringWriter();
        Console.SetOut(output);

        var ex = new InvalidOperationException("Something went wrong");

        // Act
        logger.LogError("Error with exception", ex);

        // Assert
        var consoleOutput = output.ToString();
        consoleOutput.Should().Contain("Error with exception");
        consoleOutput.Should().Contain("InvalidOperationException");
        consoleOutput.Should().Contain("Something went wrong");
    }
}
