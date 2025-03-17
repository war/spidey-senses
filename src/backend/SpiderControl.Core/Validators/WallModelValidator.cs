using FluentValidation;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Validators;

public class WallModelValidator : AbstractValidator<WallModel>
{
    public WallModelValidator()
    {
        RuleFor(x => x.Width)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Wall width must be greater than or equal to 0");

        RuleFor(x => x.Height)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Wall height must be greater than or equal to 0");
    }
}
