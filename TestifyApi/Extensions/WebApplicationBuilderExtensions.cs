using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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

            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (apiDesc.GroupName == null)
                    return docName == "Identity";
                return apiDesc.GroupName == docName;
            });

            c.SchemaFilter<TimeSpanSchemaFilter>();

        });

        builder.Services.AddScoped<ErrorHandlingMiddleware>();
    }
}
