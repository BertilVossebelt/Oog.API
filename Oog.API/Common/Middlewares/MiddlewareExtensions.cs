namespace API.Common.Middlewares;

public static class MiddlewareExtensions
{
    public static void InvalidDataExceptionMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<InvalidDataExceptionMiddleware>();

    public static void HttpRequestExceptionMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<HttpRequestExceptionMiddleware>();

    public static void ExceptionMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<ExceptionMiddleware>();
}