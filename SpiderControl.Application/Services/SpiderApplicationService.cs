using SpiderControl.Application.Interfaces;
using SpiderControl.Core.Interfaces;
using SpiderControl.Core.Services;

namespace SpiderControl.Application.Services
{
    public class SpiderApplicationService : ISpiderApplicationService
    {
        private readonly ISpiderService _spiderService;
        private readonly IInputParser _inputParser;

        public SpiderApplicationService()
        {
            _spiderService = new SpiderService();
            _inputParser = new InputParser();
        }

        public string ProcessSpiderCommands(string wallInput, string spiderInput, string commandInput)
        {
            var parsedWall = _inputParser.ParseWallDimensions(wallInput);
            var parsedSpider = _inputParser.ParseSpiderPosition(spiderInput);
            var parsedCommands = _inputParser.ParseCommands(commandInput);

            var finalSpiderPosition = _spiderService.ProcessCommands(parsedSpider, parsedWall, parsedCommands);

            return finalSpiderPosition.ToString();
        }
    }
}
