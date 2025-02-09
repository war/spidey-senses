using SpiderControl.Console.IO;

namespace SpiderControl.Console.Tests;

public class ConsoleInputReaderTests
{
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

        var inputReader = new ConsoleInputReader();

        // Act
        var result = inputReader.ReadInputs();

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

        var inputReader = new ConsoleInputReader();

        // Act
        var result = () => inputReader.ReadInputs();

        // Assert
        Assert.Throws<InvalidOperationException>(result);
    }
}
