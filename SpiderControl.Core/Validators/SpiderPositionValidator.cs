using FluentValidation;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Validators;

public class SpiderPositionValidator : AbstractValidator<SpiderModel>
{
    private readonly WallModel _wall;

    public SpiderPositionValidator(WallModel wall)
    {
        _wall = wall ?? throw new ArgumentNullException(nameof(wall));

        RuleFor(x => x.X)
            .LessThanOrEqualTo(_wall.Width)
            .WithMessage("Spider X must be greater than or equal to 0");

        RuleFor(x => x.Y)
            .LessThanOrEqualTo(_wall.Height)
            .WithMessage("Spider Y must be greater than or equal to 0");
    }
}
