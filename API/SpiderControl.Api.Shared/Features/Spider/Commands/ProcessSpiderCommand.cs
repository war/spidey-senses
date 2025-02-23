
using MediatR;
using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Models;

namespace SpiderControl.Api.Shared.Features.Spider.Commands;

public record ProcessSpiderCommand(string WallInput, string SpiderInput, string CommandInput)
    : IRequest<ProcessSpiderCommandResponse>;

public class ProcessSpiderCommandHandler : IRequestHandler<ProcessSpiderCommand, ProcessSpiderCommandResponse>
{
    private readonly ISpiderApplicationService _spiderService;

    public ProcessSpiderCommandHandler(ISpiderApplicationService spiderService)
    {
        _spiderService = spiderService;
    }

    public Task<ProcessSpiderCommandResponse> Handle(ProcessSpiderCommand command, CancellationToken ct)
    {
        var model = new ProcessCommandModel
        {
            WallInput = command.WallInput,
            SpiderInput = command.SpiderInput,
            CommandInput = command.CommandInput
        };

        var result = _spiderService.ProcessSpiderCommands(model);

        return Task.FromResult(new ProcessSpiderCommandResponse
        {
            FinalPosition = result
        });
    }
}
