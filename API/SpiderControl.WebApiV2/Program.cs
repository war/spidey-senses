using Asp.Versioning;
using SpiderControl.Api.Shared.Features.Spider.Commands;
using SpiderControl.Application;
using SpiderControl.Core.Configuration;

namespace SpiderControl.WebApiV2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = ConfigureServices(builder);

        Configure(app);

        app.Run();
    }

    public static WebApplication ConfigureServices(WebApplicationBuilder builder)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        builder.Services.Configure<SpiderControlConfig>(
            configuration.GetSection("SpiderControl")
        );

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ProcessSpiderCommand).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        builder.Services.AddOpenApiDocument(options =>
        {
            options.Title = "Spider Control API";
            options.Version = "v1";
            options.Description = "An API for controlling robotic spiders";
        });

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;

            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new QueryStringApiVersionReader("api-version")
            );
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddSpiderControlServices();

        return builder.Build();
    }

    public static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}
