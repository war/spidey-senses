using Microsoft.Extensions.Logging;
using Moq;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Models;
using SpiderControl.Application.Services;
using SpiderControl.Core.Common;
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
    private readonly Mock<ILogger<ISpiderApplicationService>> _logger;

    public SpiderApplicationServiceTests()
    {
        _spiderServiceMock = new Mock<ISpiderService>();
        _spiderInputParserMock = new Mock<ISpiderInputParser>();
        _wallInputParserMock = new Mock<IWallInputParser>();
        _commandInputParserMock = new Mock<ICommandInputParser>();
        _logger = new Mock<ILogger<ISpiderApplicationService>>();

        _spiderApplicationService = new SpiderApplicationService(
            _spiderServiceMock.Object,
            _wallInputParserMock.Object,
            _spiderInputParserMock.Object,
            _commandInputParserMock.Object,
            _logger.Object
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

        _spiderInputParserMock.Setup(x => x.ParseSpiderPosition(spiderInput))
            .Returns(Result<Spider>.Success(spider));

        _wallInputParserMock.Setup(x => x.ParseWallDimensions(wallInput))
            .Returns(Result<WallModel>.Success(wall));

        _commandInputParserMock.Setup(x => x.ParseCommands(commandInput))
            .Returns(Result<IEnumerable<ICommand>>.Success(commands));

        _spiderServiceMock.Setup(x => x.ProcessCommands(
            It.IsAny<Spider>(),
            It.IsAny<WallModel>(),
            It.IsAny<IEnumerable<ICommand>>()
        )).Returns(Result<Spider>.Success(resultSpider));

        var processCommandModel = new ProcessCommandModel
        {
            SpiderInput = spiderInput,
            WallInput = wallInput,
            CommandInput = commandInput,
        };

        // Act
        var result = _spiderApplicationService.ProcessSpiderCommands(processCommandModel);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedOutput, result.Value);

        _spiderInputParserMock.Verify(x => x.ParseSpiderPosition(spiderInput), Times.Once());
        _wallInputParserMock.Verify(x => x.ParseWallDimensions(wallInput), Times.Once());
        _commandInputParserMock.Verify(x => x.ParseCommands(commandInput), Times.Once());
        _spiderServiceMock.Verify(x => x.ProcessCommands(spider, wall, commands), Times.Once());
    }

    [Theory]
    [InlineData("7 10", "2 4 Left", "FLFLFRFFLF", "3 1 Right")]
    [InlineData("5 5", "3 3 Up", "FLFLFRFFLFRR", "0 2 Up")]
    [InlineData("3 3", "0 0 Right", "FLFLFRFFLLL", "0 3 Right")]
    public void ProcessSpiderCommand_ValidInputs_ReturnsCorrectOutput(
        string wallInput, string spiderInput, string commandInput, string expectedOutput)
    {
        // Arrange
        var expectedResult = CreateSpider(expectedOutput);

        _spiderInputParserMock.Setup(x => x.ParseSpiderPosition(spiderInput))
            .Returns(Result<Spider>.Success(new Spider(0, 0, Orientation.Up))); // Actual values don't matter

        _wallInputParserMock.Setup(x => x.ParseWallDimensions(wallInput))
            .Returns(Result<WallModel>.Success(new WallModel(0, 0))); // Actual values don't matter

        _commandInputParserMock.Setup(x => x.ParseCommands(commandInput))
            .Returns(Result<IEnumerable<ICommand>>.Success(new List<ICommand>()));

        _spiderServiceMock.Setup(x => x.ProcessCommands(
            It.IsAny<Spider>(),
            It.IsAny<WallModel>(),
            It.IsAny<IEnumerable<ICommand>>()
        )).Returns(Result<Spider>.Success(expectedResult));

        var processCommandModel = new ProcessCommandModel
        {
            SpiderInput = spiderInput,
            WallInput = wallInput,
            CommandInput = commandInput,
        };

        // Act
        var result = _spiderApplicationService.ProcessSpiderCommands(processCommandModel);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedOutput, result.Value);
    }

    [Fact]
    public void ProcessSpiderCommand_SpiderInputParserFailure_ReturnsFailure()
    {
        // Arrange
        var wallInput = "10 12";
        var spiderInput = "invalid input";
        var commandInput = "FLFLFLRRFF";

        var expectedError = "Invalid spider position format";

        _spiderInputParserMock.Setup(x => x.ParseSpiderPosition(spiderInput))
            .Returns(Result<Spider>.Failure(expectedError));

        var processCommandModel = new ProcessCommandModel
        {
            SpiderInput = spiderInput,
            WallInput = wallInput,
            CommandInput = commandInput,
        };

        // Act
        var result = _spiderApplicationService.ProcessSpiderCommands(processCommandModel);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(expectedError, result.Error);

        _spiderInputParserMock.Verify(x => x.ParseSpiderPosition(spiderInput), Times.Once());
        _wallInputParserMock.Verify(x => x.ParseWallDimensions(It.IsAny<string>()), Times.Never());
        _commandInputParserMock.Verify(x => x.ParseCommands(It.IsAny<string>()), Times.Never());
        _spiderServiceMock.Verify(x => x.ProcessCommands(
            It.IsAny<Spider>(),
            It.IsAny<WallModel>(),
            It.IsAny<IEnumerable<ICommand>>()),
            Times.Never());
    }

    [Fact]
    public void Constructor_NullSpiderApplcationService_ThrowsException()
    {
        // Arrange
        ISpiderService spiderService = null!;

        var result = () =>
            new SpiderApplicationService(
                spiderService,
                _wallInputParserMock.Object,
                _spiderInputParserMock.Object,
                _commandInputParserMock.Object,
                _logger.Object
            );

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(result);

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
