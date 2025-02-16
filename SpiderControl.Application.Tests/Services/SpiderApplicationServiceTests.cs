using Moq;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Models;
using SpiderControl.Application.Services;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Tests.Services;

public class SpiderApplicationServiceTests
{
    private readonly Mock<ISpiderService> _spiderServiceMock;

    private readonly Mock<ISpiderInputParser> _spiderInputParserMock;
    private readonly Mock<IWallInputParser> _wallInputParserMock;
    private readonly Mock<ICommandInputParser> _commandInputParserMock;

    private readonly ISpiderApplicationService _spiderApplicationService;

    public SpiderApplicationServiceTests()
    {
        _spiderServiceMock = new Mock<ISpiderService>();

        _spiderInputParserMock = new Mock<ISpiderInputParser>();
        _wallInputParserMock = new Mock<IWallInputParser>();
        _commandInputParserMock = new Mock<ICommandInputParser>();

        _spiderApplicationService = new SpiderApplicationService(
            _spiderServiceMock.Object, 
            _wallInputParserMock.Object, 
            _spiderInputParserMock.Object, 
            _commandInputParserMock.Object
        );
    }

    [Fact]
    public void ProcessSpiderCommand_ValidInput_ReturnsCorrectOutput()
    {
        // Arrange
        var wallInput = "10 12";
        var spiderInput = "6 8 Right";
        var commandInput = "FLFLFLRRFF";

        var wall = new WallModel(10, 12);
        var spider = new Spider(6, 8, Orientation.Right);
        var commands = new List<ICommand>();

        var expectedOutput = "6 11 Up";
        var resultSpider = CreateSpider(expectedOutput);

        _spiderInputParserMock.Setup(x => x.ParseSpiderPosition(spiderInput)).Returns(spider);
        _wallInputParserMock.Setup(x => x.ParseWallDimensions(wallInput)).Returns(wall);
        _commandInputParserMock.Setup(x => x.ParseCommands(commandInput)).Returns(commands);
        _spiderServiceMock.Setup(x => x.ProcessCommands(
            It.IsAny<Spider>(),
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

        _spiderInputParserMock.Verify(x => x.ParseSpiderPosition(spiderInput), Times.Once());
        _wallInputParserMock.Verify(x => x.ParseWallDimensions(wallInput), Times.Once());
        _commandInputParserMock.Verify(x => x.ParseCommands(commandInput), Times.Once());
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

        _spiderInputParserMock.Setup(x => x.ParseSpiderPosition(spiderInput)).Returns(It.IsAny<Spider>());
        _wallInputParserMock.Setup(x => x.ParseWallDimensions(wallInput)).Returns(It.IsAny<WallModel>());
        _commandInputParserMock.Setup(x => x.ParseCommands(commandInput)).Returns(It.IsAny<IEnumerable<ICommand>>());

        _spiderServiceMock.Setup(x => x.ProcessCommands(
            It.IsAny<Spider>(),
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
        var spiderApplicationService = () => new SpiderApplicationService(
            null!, 
            _wallInputParserMock.Object, 
            _spiderInputParserMock.Object, 
            _commandInputParserMock.Object
        );

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(spiderApplicationService);
        Assert.Equal(nameof(spiderService), exception.ParamName);
    }

    private static Spider CreateSpider(string position)
    {
        var positionArray = position.Split(' ');
        return new Spider(
            int.Parse(positionArray[0]),
            int.Parse(positionArray[1]),
            Enum.Parse<Orientation>(positionArray[2])
        );
    }
}
