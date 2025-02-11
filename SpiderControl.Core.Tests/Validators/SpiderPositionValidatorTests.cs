using FluentValidation.TestHelper;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Models;
using SpiderControl.Core.Validators;

namespace SpiderControl.Core.Tests.Validators;

public class SpiderPositionValidatorTests
{
    private readonly WallModel _wallModel;
    private readonly SpiderPositionValidator _spiderPositionValidator;

    public SpiderPositionValidatorTests()
    {
        _wallModel = new WallModel(10, 10);
        _spiderPositionValidator = new SpiderPositionValidator(_wallModel);
    }

    [Theory]
    [InlineData(0, 0, Orientation.Up)]
    [InlineData(10, 10, Orientation.Right)]
    [InlineData(5, 5, Orientation.Down )]
    [InlineData(1, 1, Orientation.Left)]
    public void Validate_ValidDimensions_ShouldNotHaveValidationError(int x, int y, Orientation orientation)
    {
        // Arrange
        var spider = new SpiderModel(x, y, orientation);

        // Act
        var result = _spiderPositionValidator.TestValidate(spider);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(11, 5, Orientation.Down, "X")]      // X > wall width
    [InlineData(5, 11, Orientation.Down, "Y")]      // Y > wall height
    public void Validate_InvalidDimensions_ShouldHaveValidationError(int x, int y, Orientation orientation, string propertyName)
    {
        // Arrange
        var spider = new SpiderModel(x, y, orientation);

        // Act
        var result = _spiderPositionValidator.TestValidate(spider);

        // Assert
        result.ShouldHaveValidationErrorFor(propertyName);
    }
}
