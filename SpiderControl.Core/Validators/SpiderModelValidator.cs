using FluentValidation;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Validators;

public class SpiderModelValidator : AbstractValidator<SpiderModel>
{
    private readonly WallModel _wall;

    public SpiderModelValidator(WallModel wall)
    {
        _wall = wall ?? throw new ArgumentNullException(nameof(wall));

        RuleFor(x => x.X)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(_wall.Width)
            .WithMessage("Wall X must be greater than or equal to 0");

        RuleFor(x => x.Y)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(_wall.Height)
            .WithMessage("Spider Y must be greater than or equal to 0");

        RuleFor(x => x.Orientation)
            .IsInEnum()
            .WithMessage("Invalid orientation value");
    }
}
