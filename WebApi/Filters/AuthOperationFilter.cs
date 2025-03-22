using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AuthOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // List of controllers that require authentication
        var protectedControllers = new HashSet<string>
        {
            // Add new controllers here
            "CategoryController",
            "SubCategoryController",
            "TableController",
            
        };

        var controllerName = context.MethodInfo.DeclaringType.Name;

        if (protectedControllers.Contains(controllerName))
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
        else
        {
            operation.Security.Clear(); // Keep other controllers public
        }
    }
}
