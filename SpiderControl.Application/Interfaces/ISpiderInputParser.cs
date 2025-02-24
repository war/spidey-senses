using SpiderControl.Core.Common;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Interfaces;

public interface ISpiderInputParser
{
    Result<Spider> ParseSpiderPosition(string input);
}
