
namespace SpiderControl.Api.Shared.Features.Api.Models;

public record MetricsResponse(string Uptime, string MemoryUsage, double CpuUsage,
    int ThreadCount, int RequestCount);
