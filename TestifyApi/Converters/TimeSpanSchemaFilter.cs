using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Testify.API.Converters;

public class TimeSpanSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(TimeSpan) || context.Type == typeof(TimeSpan?))
        {
            schema.Type = "string";
            schema.Format = "time-span";
            schema.Example = new Microsoft.OpenApi.Any.OpenApiString("00:00:00");
            schema.Properties?.Clear();
        }
    }
}
