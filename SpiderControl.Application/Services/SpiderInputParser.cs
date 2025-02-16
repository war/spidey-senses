using SpiderControl.Application.Interfaces;
using SpiderControl.Core.Enums;
using SpiderControl.Core.Exceptions;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Services;

public class SpiderInputParser : ISpiderInputParser
{
    private readonly IValidatorService _validatorService;

    public SpiderInputParser(IValidatorService validatorService)
    {
        _validatorService = validatorService ?? throw new ArgumentNullException();
    }

    public Spider ParseSpiderPosition(string input)
    {
        try
        {
            var args = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != 3 ||
                !int.TryParse(args[0], out int x) ||
                !int.TryParse(args[1], out int y) ||
                !Enum.TryParse<Orientation>(args[2], true, out var orientation) || (int)orientation > 3)
            {
                throw new InputParseException("Invalid spider position format. Expected: 'x y orientation' where x and y are integers and orientation is Up/Down/Left/Right");
            }

            var spider = new Spider(x, y, orientation);
            var validationResult = _validatorService.ValidateSpider(spider);

            if (!validationResult.IsValid)
            {
                throw new ModelValidationException(validationResult);
            }

            return new Spider(x, y, orientation);
        }
        catch (Exception ex) when (ex is not InputParseException and not ModelValidationException)
        {
            throw new InputParseException($"Failed to parse spider position: {ex.Message}");
        }
    }
}
