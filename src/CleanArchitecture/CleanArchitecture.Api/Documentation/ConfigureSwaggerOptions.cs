using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.Api.Documentation;

public sealed class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach(var documentacion in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(documentacion.GroupName, CreateDocumentation(documentacion));
        }
    }

    private static OpenApiInfo CreateDocumentation(ApiVersionDescription apiVersionDescription)
    {
        var info = new OpenApiInfo
        {
            Title = $"CleanArchitecture.Api v{ apiVersionDescription.ApiVersion}",
            Version = apiVersionDescription.ApiVersion.ToString(),
            Description = "Clean Architecture API",
            Contact = new OpenApiContact
            {
                Name = "Clean Architecture",
                Email = "susio24@yahoo.es"
            }
        };

        if(apiVersionDescription.IsDeprecated)
        {
            info.Description += " - This API version has been deprecated.";
        }

        return info;
    }
}

    