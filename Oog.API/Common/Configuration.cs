using API.Common.Middlewares;

namespace API.Common;

public static class Configuration
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddCors(options =>
                options.AddPolicy("_myAllowSpecificOrigins",
                    corsPolicyBuilder =>
                    {
                        corsPolicyBuilder.WithOrigins("https://localhost:5050")
                            .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    }
                )
            )
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }

    public static void RegisterMiddlewares(this WebApplication app, IConfiguration configuration)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger().UseSwaggerUI();
        }

        // app.UseHttpsRedirection();
        app.UseCors("_myAllowSpecificOrigins");

        var jwtSecret = configuration.GetSection("JwtSettings:Secret").Value;

        // Register middlewares that obfuscate exceptions and errors for production only.
        if (!app.Environment.IsDevelopment())
        {
            app.ExceptionMiddleware();
            app.HttpRequestExceptionMiddleware();
            app.InvalidDataExceptionMiddleware();
        }

        app.UseWhen(context =>
                // Register routes that do not require authorization.
                !context.Request.Path.StartsWithSegments("/") &&
                !context.Request.Path.StartsWithSegments("/swagger") &&
                !context.Request.Path.StartsWithSegments("/api/v1/account/create") &&
                !context.Request.Path.StartsWithSegments("/api/v1/account/authenticate"),

                // Require authentication for everything else.
            appBuilder => { appBuilder.UseMiddleware<AuthorizationMiddleware>(); }
        );
        
        // Register all other middlewares.
        
    }
}