using System.ComponentModel.DataAnnotations;

namespace API.Common.Middlewares;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<T>().FirstOrDefault();
        if (request == null) return TypedResults.BadRequest(new { message = "Invalid request body" });

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            return await next(context);
        }
        
        var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
        return TypedResults.BadRequest(new { message = "Validation errors", errors });
    }
}