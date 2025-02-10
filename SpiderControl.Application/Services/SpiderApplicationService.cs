using SpiderControl.Application.Interfaces;
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

        public string ProcessSpiderCommands(string spiderInput, string wallInput, string commandInput)
        {
            var parsedSpider = _inputParser.ParseSpiderPosition(spiderInput);
            var parsedWall = _inputParser.ParseWallDimensions(wallInput);
            var parsedCommands = _inputParser.ParseCommands(commandInput);

            var finalSpiderPosition = _spiderService.ProcessCommands(parsedSpider, parsedWall, parsedCommands);

            return finalSpiderPosition.ToString();
        }
    }
}
