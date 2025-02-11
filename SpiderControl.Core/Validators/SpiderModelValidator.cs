using FluentValidation;
using SpiderControl.Core.Models;

namespace SpiderControl.Core.Validators;

public class SpiderModelValidator : AbstractValidator<SpiderModel>
{
    public SpiderModelValidator()
    {
        RuleFor(x => x.X)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Wall X must be greater than or equal to 0");

        RuleFor(x => x.Y)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Spider Y must be greater than or equal to 0");

        RuleFor(x => x.Orientation)
            .IsInEnum()
            .WithMessage("Invalid orientation value");
    }
}
