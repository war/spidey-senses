
namespace SpiderControl.Api.Shared.Features.Api.Models;

public record ComponentStatus(string Name, string Status, string Message);
public record StatusResponse(string Status, ComponentStatus[] Components, string Uptime);
