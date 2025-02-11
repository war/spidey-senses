using FluentValidation.TestHelper;
using SpiderControl.Core.Models;
using SpiderControl.Core.Validators;

namespace SpiderControl.Core.Tests.Validators;

public class CommandValidatorTests
{
    private readonly CommandValidator _validator;

    public CommandValidatorTests()
    {
        _validator = new CommandValidator();
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
