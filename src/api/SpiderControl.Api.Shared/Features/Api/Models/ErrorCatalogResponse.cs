
namespace SpiderControl.Api.Shared.Features.Api.Models;

public record ErrorInfo(string Code, string Message, int StatusCode);
public record ErrorCatalogResponse(ErrorInfo[] Errors);
