using FluentValidation.TestHelper;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Models;
using SpiderControl.Core.Validators;

namespace SpiderControl.Core.Tests.Validators;

public class SpiderModelValidatorTests
{
    private readonly WallModel _wallModel;
    private readonly SpiderModelValidator _spiderValidator;

    public SpiderModelValidatorTests()
    {
        _wallModel = new WallModel(10, 10);
        _spiderValidator = new SpiderModelValidator();
    }

    [Theory]
    [InlineData(0, 0, Orientation.Up)]
    [InlineData(10, 10, Orientation.Right)]
    [InlineData(5, 5, Orientation.Down )]
    [InlineData(1, 1, Orientation.Left)]
    public void Validate_ValidDimensions_ShouldNotHaveValidationError(int x, int y, Orientation orientation)
    {
        // Arrange
        var spider = new Spider(x, y, orientation);

        // Act
        var result = _spiderValidator.TestValidate(spider);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(-1, 5, Orientation.Up, "X")]
    [InlineData(5, -1, Orientation.Right, "Y")]
    public void Validate_InvalidDimensions_ShouldHaveValidationError(int x, int y, Orientation orientation, string propertyName)
    {
        // Arrange
        var spider = new Spider(x, y, orientation);

        // Act
        var result = _spiderValidator.TestValidate(spider);

        // Assert
        result.ShouldHaveValidationErrorFor(propertyName);
    }
}
