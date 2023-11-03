using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiMyBgList.Attributes;

namespace WebApiMyBgList.Swagger;

public class SortOrderFilter : IParameterFilter
{
    public void Apply(
        OpenApiParameter parameter,
        ParameterFilterContext context)
    {
        var attributes = context.ParameterInfo?
            .GetCustomAttributes(true)
            .Union(
                context.ParameterInfo.ParameterType.GetProperties()
                    .Where(p => p.Name == parameter.Name)
                    .SelectMany(p => p.GetCustomAttributes(true))
            )
            .OfType<SortOrderValidatorAttribute>();

        if (attributes == null) return;
        foreach (var attribute in attributes)
            parameter.Schema.Extensions.Add(
                "pattern",
                new OpenApiString(string.Join("|", attribute.AllowedValues.Select(v => $"^{v}$")))
            );
    }
}
