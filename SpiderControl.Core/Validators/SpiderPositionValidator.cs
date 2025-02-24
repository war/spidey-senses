using FluentValidation;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Validators;

public class SpiderPositionValidator : AbstractValidator<Spider>
{
    private readonly WallModel _wall;

    public SpiderPositionValidator(WallModel wall)
    {
        _wall = wall ?? throw new ArgumentNullException(nameof(wall));

        RuleFor(x => x.X)
            .LessThanOrEqualTo(_wall.Width)
            .WithMessage("Spider X must be less than or equal to wall width");

        RuleFor(x => x.Y)
            .LessThanOrEqualTo(_wall.Height)
            .WithMessage("Spider Y must be less than or equal to wall height");
    }
}
