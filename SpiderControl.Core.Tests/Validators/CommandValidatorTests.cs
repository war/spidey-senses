using FluentValidation.TestHelper;
using Microsoft.Extensions.Options;
using Moq;
using SpiderControl.Core.Configuration;
using SpiderControl.Core.Models;
using SpiderControl.Core.Validators;

namespace SpiderControl.Core.Tests.Validators;

public class CommandValidatorTests
{
    private readonly CommandValidator _validator;
    private readonly SpiderControlConfig _defaultConfig;
    private readonly Mock<IOptions<SpiderControlConfig>> _configMock;

    public CommandValidatorTests()
    {
        _defaultConfig = new SpiderControlConfig
        {
            ValidCommands = new[] { 'F', 'L', 'R' }
        };

        _configMock = new Mock<IOptions<SpiderControlConfig>>();
        _configMock.Setup(x => x.Value).Returns(_defaultConfig);
        _validator = new CommandValidator(_configMock.Object);
    }

    [Theory]
    [InlineData('F')]
    [InlineData('L')]
    [InlineData('R')]
    [InlineData('f')]
    [InlineData('l')]
    [InlineData('r')]
    public void Validate_ValidDimensions_ShouldNotHaveValidationError(char command)
    {
        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData('X')]
    [InlineData('5')]
    [InlineData(' ')]
    [InlineData('\r')]
    public void Validate_InvalidDimensions_ShouldHaveValidationError(char commmand)
    {
        // Act
        var result = _validator.TestValidate(commmand);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x);
    }
}
