using SpiderControl.Core.Common;
using SpiderControl.Core.Models;

namespace SpiderControl.Application.Interfaces;

public interface IWallInputParser
{
    Result<WallModel> ParseWallDimensions(string input);
}
