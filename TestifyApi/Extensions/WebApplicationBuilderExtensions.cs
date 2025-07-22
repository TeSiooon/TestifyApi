using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.RateLimiting;
using Testify.API.Converters;
using Testify.API.Middlewares;

namespace Testify.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        builder.Services.AddControllers()
            .AddJsonOptions(options => 
            {
                options.JsonSerializerOptions.Converters.Add(new TimeSpanJsonConverter());
            });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c => 
        {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}
                    },
                    []
                }

            });

            c.SwaggerDoc("Quizzes", new OpenApiInfo { Title = "Quiz API", Version = "v1" });
            c.SwaggerDoc("Identity", new OpenApiInfo { Title = "Identity API", Version = "v1" });
            c.SwaggerDoc("Questions", new OpenApiInfo { Title = "Question API", Version = "v1" });
            c.SwaggerDoc("QuizAttempts", new OpenApiInfo { Title = "QuizAttempt API", Version = "v1" });
            c.SwaggerDoc("UserProfile", new OpenApiInfo { Title = "UserProfile API", Version = "v1" });
            c.SwaggerDoc("Enums", new OpenApiInfo { Title = "Enum API", Version = "v1" });

            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (apiDesc.GroupName == null)
                    return docName == "Identity";
                return apiDesc.GroupName == docName;
            });

            c.SchemaFilter<TimeSpanSchemaFilter>();
            c.OperationFilter<AddEndpointUrlOperationFilter>();

        });

        builder.Services.AddScoped<ErrorHandlingMiddleware>();

        builder.Services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: ip,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromSeconds(30),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync(
                    "{\"error\":\"Too many requests. Please try again later.\"}", token);
            };
        });
    }
}


// for tests maybe save for later
public class AddEndpointUrlOperationFilter : IOperationFilter
{
    private readonly string _swaggerBasePath;

    public AddEndpointUrlOperationFilter(string swaggerBasePath = "")
    {
        _swaggerBasePath = swaggerBasePath;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Pobieramy ścieżkę (np. /api/Questions/{id})
        var route = context.ApiDescription.RelativePath;

        // Budujemy pełny URL — tu możesz dopasować swój swaggerBasePath (np. /swagger/v1)
        var fullUrl = $"{_swaggerBasePath}/{route}";

        var linkMarkdown = $"**Endpoint URL:** [{fullUrl}]({fullUrl})";

        if (operation.Description == null)
            operation.Description = linkMarkdown;
        else
            operation.Description += "\n\n" + linkMarkdown;
    }
}