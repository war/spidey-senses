using SpiderControl.Application.Interfaces;
using SpiderControl.Application.Models;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Services;

namespace SpiderControl.Application.Services
{
    public class SpiderApplicationService : ISpiderApplicationService
    {
        private readonly ISpiderService _spiderService;
        private readonly IInputParser _inputParser;

        public SpiderApplicationService(ISpiderService spiderService, IInputParser inputParser)
        {
            _spiderService = spiderService ?? throw new ArgumentNullException(nameof(spiderService));
            _inputParser = inputParser ?? throw new ArgumentNullException(nameof(inputParser)); ;
        }

        public string ProcessSpiderCommands(ProcessCommandModel model)
        {
            var parsedSpider = _inputParser.ParseSpiderPosition(model.SpiderInput);
            var parsedWall = _inputParser.ParseWallDimensions(model.WallInput);
            var parsedCommands = _inputParser.ParseCommands(model.CommandInput);

            var finalSpiderPosition = _spiderService.ProcessCommands(parsedSpider, parsedWall, parsedCommands);

            return finalSpiderPosition.ToString();
        }
    }
}
