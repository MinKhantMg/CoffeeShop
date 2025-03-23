using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

public class AuthOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var controllerType = context.MethodInfo.DeclaringType;
        if (controllerType == null) return;

        // Check if the method or controller has the [Authorize] attribute
        var hasAuthorizeAttribute = controllerType.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Any() ||
            context.MethodInfo.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Any();

        // Check if the method has [AllowAnonymous] (which overrides [Authorize])
        var hasAllowAnonymous = context.MethodInfo.GetCustomAttributes(true)
            .OfType<AllowAnonymousAttribute>()
            .Any();

        // If the method has [AllowAnonymous], don't require authentication in Swagger
        if (hasAllowAnonymous)
        {
            return;
        }

        // If the controller or method has [Authorize], add security definition
        if (hasAuthorizeAttribute)
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                }
            };
        }
    }
}
