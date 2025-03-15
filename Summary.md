# Summary
|||
|:---|:---|
| Generated on: | 03/15/2025 - 09:59:34 |
| Coverage date: | 03/15/2025 - 09:59:23 - 03/15/2025 - 09:59:32 |
| Parser: | MultiReport (4x Cobertura) |
| Assemblies: | 6 |
| Classes: | 49 |
| Files: | 46 |
| **Line coverage:** | 83.8% (680 of 811) |
| Covered lines: | 680 |
| Uncovered lines: | 131 |
| Coverable lines: | 811 |
| Total lines: | 1530 |
| **Branch coverage:** | 77.4% (127 of 164) |
| Covered branches: | 127 |
| Total branches: | 164 |
| **Method coverage:** | [Feature is only available for sponsors](https://reportgenerator.io/pro) |

|**Name**|**Covered**|**Uncovered**|**Coverable**|**Total**|**Line coverage**|**Covered**|**Total**|**Branch coverage**|
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
|**SpiderControl.Api.Shared**|**51**|**2**|**53**|**186**|**96.2%**|**2**|**2**|**100%**|
|SpiderControl.Api.Shared.Features.Api.Models.ComponentStatus|1|0|1|5|100%|0|0||
|SpiderControl.Api.Shared.Features.Api.Models.ConfigResponse|2|0|2|5|100%|0|0||
|SpiderControl.Api.Shared.Features.Api.Models.ErrorCatalogResponse|1|0|1|5|100%|0|0||
|SpiderControl.Api.Shared.Features.Api.Models.ErrorInfo|1|0|1|5|100%|0|0||
|SpiderControl.Api.Shared.Features.Api.Models.MetricsResponse|2|0|2|5|100%|0|0||
|SpiderControl.Api.Shared.Features.Api.Models.StatusResponse|1|0|1|5|100%|0|0||
|SpiderControl.Api.Shared.Features.Api.Models.VersionResponse|1|0|1|4|100%|0|0||
|SpiderControl.Api.Shared.Features.Spider.Commands.ProcessSpiderCommand|3|0|3|56|100%|0|0||
|SpiderControl.Api.Shared.Features.Spider.Commands.ProcessSpiderCommandHandler|26|0|26|56|100%|2|2|100%|
|SpiderControl.Api.Shared.Features.Spider.Commands.ProcessSpiderCommandRequest|3|0|3|10|100%|0|0||
|SpiderControl.Api.Shared.Features.Spider.Commands.ProcessSpiderCommandResponse|1|0|1|6|100%|0|0||
|SpiderControl.Api.Shared.Features.Spider.Models.SpiderError|0|2|2|7|0%|0|0||
|SpiderControl.Api.Shared.Features.Spider.Models.SpiderProblemDetails|9|0|9|17|100%|0|0||
|**SpiderControl.Api.Shared.Testing**|**5**|**6**|**11**|**24**|**45.4%**|**0**|**0**|****|
|SpiderControl.Api.Shared.Testing.Infrastructure.TestBase`1|5|6|11|24|45.4%|0|0||
|**SpiderControl.Application**|**154**|**14**|**168**|**316**|**91.6%**|**44**|**54**|**81.4%**|
|SpiderControl.Application.Models.ProcessCommandModel|3|0|3|8|100%|0|0||
|SpiderControl.Application.ServiceCollectionExtensions|11|0|11|27|100%|0|0||
|SpiderControl.Application.Services.CommandInputParser|21|0|21|41|100%|8|8|100%|
|SpiderControl.Application.Services.SpiderApplicationService|59|5|64|111|92.1%|17|24|70.8%|
|SpiderControl.Application.Services.SpiderInputParser|34|6|40|74|85%|12|14|85.7%|
|SpiderControl.Application.Services.WallInputParser|26|3|29|55|89.6%|7|8|87.5%|
|**SpiderControl.Console**|**36**|**41**|**77**|**147**|**46.7%**|**4**|**6**|**66.6%**|
|SpiderControl.Console.IO.ConsoleInputReader|27|3|30|59|90%|4|6|66.6%|
|SpiderControl.Console.Models.InputModel|9|0|9|15|100%|0|0||
|SpiderControl.Console.Program|0|38|38|73|0%|0|0||
|**SpiderControl.Core**|**227**|**29**|**256**|**495**|**88.6%**|**57**|**68**|**83.8%**|
|SpiderControl.Core.Commands.ForwardCommand|9|1|10|25|90%|9|10|90%|
|SpiderControl.Core.Commands.RotateLeftCommand|3|0|3|13|100%|0|0||
|SpiderControl.Core.Commands.RotateRightCommand|3|0|3|13|100%|0|0||
|SpiderControl.Core.Common.Error|0|1|1|3|0%|0|0||
|SpiderControl.Core.Common.Result`1|11|0|11|18|100%|0|0||
|SpiderControl.Core.Common.Unit|1|0|1|3|100%|0|0||
|SpiderControl.Core.Configuration.SpiderControlConfig|1|0|1|6|100%|0|0||
|SpiderControl.Core.Exceptions.InputParseException|0|2|2|11|0%|0|0||
|SpiderControl.Core.Exceptions.InvalidOrientationException|0|2|2|9|0%|0|0||
|SpiderControl.Core.Exceptions.InvalidSpiderToStringException|0|2|2|9|0%|0|0||
|SpiderControl.Core.Exceptions.ModelValidationException|0|11|11|22|0%|0|0||
|SpiderControl.Core.Factories.CommandFactory|9|0|9|19|100%|6|6|100%|
|SpiderControl.Core.Models.Spider|58|5|63|90|92%|17|22|77.2%|
|SpiderControl.Core.Models.WallModel|7|0|7|13|100%|0|0||
|SpiderControl.Core.Services.SpiderService|30|3|33|60|90.9%|8|10|80%|
|SpiderControl.Core.Services.ValidatorService|56|2|58|98|96.5%|16|18|88.8%|
|SpiderControl.Core.Validators.CommandValidator|8|0|8|21|100%|0|0||
|SpiderControl.Core.Validators.SpiderModelValidator|12|0|12|22|100%|0|0||
|SpiderControl.Core.Validators.SpiderPositionValidator|10|0|10|22|100%|1|2|50%|
|SpiderControl.Core.Validators.WallModelValidator|9|0|9|18|100%|0|0||
|**SpiderControl.WebApiV2**|**207**|**39**|**246**|**428**|**84.1%**|**20**|**34**|**58.8%**|
|SpiderControl.WebApiV2.Features.Api.ApiController|64|0|64|118|100%|2|4|50%|
|SpiderControl.WebApiV2.Features.Health.HealthController|23|0|23|55|100%|2|4|50%|
|SpiderControl.WebApiV2.Features.Spider.SpiderController|41|14|55|101|74.5%|14|18|77.7%|
|SpiderControl.WebApiV2.Middleware.ErrorResponse|1|0|1|3|100%|0|0||
|SpiderControl.WebApiV2.Middleware.ExceptionHandlingMiddleware|10|22|32|50|31.2%|0|6|0%|
|SpiderControl.WebApiV2.Program|68|3|71|101|95.7%|2|2|100%|
