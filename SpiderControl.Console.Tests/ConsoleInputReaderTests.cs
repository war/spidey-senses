using Microsoft.Extensions.Logging;
using Moq;
using SpiderControl.Console.Interfaces;
using SpiderControl.Console.IO;

namespace SpiderControl.Console.Tests;

public class ConsoleInputReaderTests
{
    private readonly Mock<ILogger<IConsoleInputReader>> _logger;
    private readonly IConsoleInputReader _consoleInputReader;

    public ConsoleInputReaderTests()
    {
        _logger = new Mock<ILogger<IConsoleInputReader>>();
        _consoleInputReader = new ConsoleInputReader(_logger.Object);
    }

    [Fact]
    public void ReadInputs_ValidInput_ReturnsExpectedInput()
    {
        // Arrange
        var input = new StringReader(
            "7 15\n" +
            "1 5 Right\n" +
            "FLFLFLFRFF\n"
        );

        System.Console.SetIn(input);

        // Act
        var result = _consoleInputReader.ReadInputs();

        // Assert
        Assert.Equal("7 15", result.WallDimensions);
        Assert.Equal("1 5 Right", result.SpiderPosition);
        Assert.Equal("FLFLFLFRFF", result.Commands);
    }

    [Fact]
    public void ReadInputs_EmptyInput_ThrowsError()
    {
        // Arrange
        var input = new StringReader("\n\n\n");

        System.Console.SetIn(input);

        // Act
        var result = () => _consoleInputReader.ReadInputs();

        // Assert
        Assert.Throws<InvalidOperationException>(result);
    }
}
