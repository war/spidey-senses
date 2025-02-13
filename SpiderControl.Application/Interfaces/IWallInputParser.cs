using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Interfaces;

public interface IWallInputParser
{
    WallModel ParseWallDimensions(string input);
}
