
namespace SpiderControl.Api.Shared.Features.Api.Models;

public record ConfigResponse(int ApiRateLimit, int MaxCommandLength,
    char[] ValidCommands, object MaxWallDimensions);
