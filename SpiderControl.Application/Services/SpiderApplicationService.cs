using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Models;
using SpiderControl.Core.Interfaces;

namespace SpiderControl.Application.Services
{
    public class SpiderApplicationService : ISpiderApplicationService
    {
        private readonly ISpiderService _spiderService;

        private readonly IWallInputParser _wallInputParser;
        private readonly ISpiderInputParser _spiderInputParser;
        private readonly ICommandInputParser _commandInputParser;

        public SpiderApplicationService(
            ISpiderService spiderService, 
            IWallInputParser wallInputParser, 
            ISpiderInputParser spiderInputParser, 
            ICommandInputParser commandInputParser)
        {
            _spiderService = spiderService ?? throw new ArgumentNullException(nameof(spiderService));

            _wallInputParser = wallInputParser ?? throw new ArgumentNullException(nameof(wallInputParser)); ;
            _spiderInputParser = spiderInputParser ?? throw new ArgumentNullException(nameof(spiderInputParser)); ;
            _commandInputParser = commandInputParser ?? throw new ArgumentNullException(nameof(commandInputParser)); ;
        }

        public string ProcessSpiderCommands(ProcessCommandModel model)
        {
            var parsedSpider = _spiderInputParser.ParseSpiderPosition(model.SpiderInput);
            var parsedWall = _wallInputParser.ParseWallDimensions(model.WallInput);
            var parsedCommands = _commandInputParser.ParseCommands(model.CommandInput);

            var finalSpiderPosition = _spiderService.ProcessCommands(parsedSpider, parsedWall, parsedCommands);

            return finalSpiderPosition.ToString();
        }
    }
}
