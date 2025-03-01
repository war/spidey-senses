using FluentValidation.TestHelper;
using SpiderControl.Core.Models;
using SpiderControl.Core.Validators;

namespace SpiderControl.Core.Tests.Validators;

public class WallModelValidatorTests
{
    private readonly WallModelValidator _validator;

    public WallModelValidatorTests()
    {
        _validator = new WallModelValidator();
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(10, 10)]
    [InlineData(100, 100)]
    [InlineData(1, 100)]
    [InlineData(0, 100)]
    public void Validate_ValidDimensions_ShouldNotHaveValidationError(int width, int height)
    {
        // Arrange
        var wall = new WallModel(width, height);

        // Act
        var result = _validator.TestValidate(wall);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(-1, 1, "Width")]
    [InlineData(1, -1, "Height")]
    [InlineData(-1, -1, "Width")]
    public void Validate_InvalidDimensions_ShouldHaveValidationError(int width, int height, string propertyName)
    {
        // Arrange
        var wall = new WallModel(width, height);

        // Act
        var result = _validator.TestValidate(wall);

        // Assert
        result.ShouldHaveValidationErrorFor(propertyName);
    }
}
