using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiMyBgList.Attributes;
using ICustomAttributeProvider = System.Reflection.ICustomAttributeProvider;

namespace WebApiMyBgList.Swagger;

public class CustomKeyValueFilter : ISchemaFilter
{
    public void Apply(
        OpenApiSchema schema,
        SchemaFilterContext context)
    {
        var caProvider = context.MemberInfo
                         ?? context.ParameterInfo
                             as ICustomAttributeProvider;

        var attributes = caProvider?
            .GetCustomAttributes(true)
            .OfType<CustomKeyValueAttribute>();

        if (attributes == null) return;
        foreach (var attribute in attributes)
            schema.Extensions.Add(
                attribute.Key ?? string.Empty,
                new OpenApiString(attribute.Value)
            );
    }
}
