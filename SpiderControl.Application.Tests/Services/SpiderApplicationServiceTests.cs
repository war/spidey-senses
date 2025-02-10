using Moq;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Models;
using SpiderControl.Application.Services;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;
using SpiderControl.Core.Services;

namespace SpiderControl.Application.Tests.Services;

public class SpiderApplicationServiceTests
{
    private readonly Mock<ISpiderService> _spiderServiceMock;
    private readonly Mock<IInputParser> _inputParserMock;

    private readonly ISpiderApplicationService _spiderApplicationService;

    public SpiderApplicationServiceTests()
    {
        _spiderServiceMock = new Mock<ISpiderService>();
        _inputParserMock = new Mock<IInputParser>();

        _spiderApplicationService = new SpiderApplicationService(_spiderServiceMock.Object, _inputParserMock.Object);
    }

    [Fact]
    public void ProcessSpiderCommand_ValidInput_ReturnsCorrectOutput()
    {
        // Arrange
        var wallInput = "10 12";
        var spiderInput = "6 8 Right";
        var commandInput = "FLFLFLRRFF";

        var wall = new WallModel(10, 12);
        var spider = new SpiderModel(6, 8, Orientation.Right);
        var commands = new List<ICommand>();

        var expectedOutput = "6 11 Up";
        var resultSpider = CreateSpider(expectedOutput);

        _inputParserMock.Setup(x => x.ParseSpiderPosition(spiderInput)).Returns(spider);
        _inputParserMock.Setup(x => x.ParseWallDimensions(wallInput)).Returns(wall);
        _inputParserMock.Setup(x => x.ParseCommands(commandInput)).Returns(commands);
        _spiderServiceMock.Setup(x => x.ProcessCommands(
            It.IsAny<SpiderModel>(),
            It.IsAny<WallModel>(),
            It.IsAny<IEnumerable<ICommand>>()
        )).Returns(resultSpider);

        var processCommandModel = new ProcessCommandModel
        {
            SpiderInput = spiderInput,
            WallInput = wallInput,
            CommandInput = commandInput,
        };

        // Act
        var result = _spiderApplicationService.ProcessSpiderCommands(processCommandModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedOutput, result);

        _inputParserMock.Verify(x => x.ParseSpiderPosition(spiderInput), Times.Once());
        _inputParserMock.Verify(x => x.ParseWallDimensions(wallInput), Times.Once());
        _inputParserMock.Verify(x => x.ParseCommands(commandInput), Times.Once());
        _spiderServiceMock.Verify(x => x.ProcessCommands(spider, wall, commands), Times.Once());
    }

    [Theory]
    [InlineData("7 10", "2 4 Left", "FLFLFRFFLF", "3 1 Right")]
    [InlineData("5 5", "3 3 Up", "FLFLFRFFLFRR", "0 2 Up")]
    [InlineData("3 3", "0 0 Right", "FLFLFRFFLLL", "0 3 Right")]
    public void ProcessSpiderCommand_ValidInputs_ReturnsCorrectOutput(string wallInput, string spiderInput, string commandInput, string expectedOutput)
    {
        // Arrange
        var expectedResult = CreateSpider(expectedOutput);

        _inputParserMock.Setup(x => x.ParseSpiderPosition(spiderInput)).Returns(It.IsAny<SpiderModel>());
        _inputParserMock.Setup(x => x.ParseWallDimensions(wallInput)).Returns(It.IsAny<WallModel>());
        _inputParserMock.Setup(x => x.ParseCommands(commandInput)).Returns(It.IsAny<IEnumerable<ICommand>>());

        _spiderServiceMock.Setup(x => x.ProcessCommands(
            It.IsAny<SpiderModel>(),
            It.IsAny<WallModel>(),
            It.IsAny<IEnumerable<ICommand>>()
        )).Returns(expectedResult);

        var processCommandModel = new ProcessCommandModel
        {
            SpiderInput = spiderInput,
            WallInput = wallInput,
            CommandInput = commandInput,
        };

        // Act
        var result = _spiderApplicationService.ProcessSpiderCommands(processCommandModel);

        // Assert
        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public void Constructor_NullSpiderApplcationService_ThrowsException()
    {
        // Arrange
        // declaring ISpiderService for the use of nameof(spiderService) in the assert
        // not perfect, but better than strings
        ISpiderService spiderService;
        var spiderApplicationService = () => new SpiderApplicationService(null!, _inputParserMock.Object);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(spiderApplicationService);
        Assert.Equal(nameof(spiderService), exception.ParamName);
    }

    [Fact]
    public void Constructor_NullInputParser_ThrowsException()
    {
        // Arrange
        IInputParser inputParser;
        var spiderApplicationService = () => new SpiderApplicationService(_spiderServiceMock.Object, null!);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(spiderApplicationService);
        Assert.Equal(nameof(inputParser), exception.ParamName);
    }

    private static SpiderModel CreateSpider(string position)
    {
        var positionArray = position.Split(' ');
        return new SpiderModel(
            int.Parse(positionArray[0]),
            int.Parse(positionArray[1]),
            Enum.Parse<Orientation>(positionArray[2])
        );
    }
}
