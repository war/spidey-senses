using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Interfaces;

public interface ISpiderInputParser
{
    Spider ParseSpiderPosition(string input);
}
