namespace SpiderControl.Api.Shared.Features.Spider.Commands;

public record ProcessSpiderCommandResponse
{
    public required string FinalPosition { get; init; }
}