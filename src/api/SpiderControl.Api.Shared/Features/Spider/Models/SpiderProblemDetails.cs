using Microsoft.AspNetCore.Mvc;

namespace SpiderControl.Api.Shared.Features.Spider.Models;

public class SpiderProblemDetails : ProblemDetails
{
    public SpiderProblemDetails(string title, string detail, int status)
    {
        Title = title;
        Detail = detail;
        Status = status;
        Type = $"https://spideysensesapi.com/errors/{status}";
        Instance = $"/errors/{Guid.NewGuid()}";
    }

    public List<SpiderError> Errors { get; set; } = new();
}
