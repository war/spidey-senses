using MediatR;

namespace SpiderControl.Api.Shared.Features.Spider.Commands;

public record ProcessSpiderCommandRequest : IRequest<ProcessSpiderCommandResponse>
{
    public required string WallInput { get; init; }
    public required string SpiderInput { get; init; }
    public required string CommandInput { get; init; }
}
