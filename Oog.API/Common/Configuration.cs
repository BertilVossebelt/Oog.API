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
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .AddSignalR();
    }

    public static void RegisterMiddlewares(this WebApplication app)
    {
        // Register middlewares for development only.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger().UseSwaggerUI();
        }
        
        // Register middlewares for production only.
        if (app.Environment.IsProduction())
        {            
            app.UseHttpsRedirection();
            app.UseHsts();
            app.ExceptionMiddleware();
            app.HttpRequestExceptionMiddleware();
            app.InvalidDataExceptionMiddleware();
        }

        // Register middlewares that are used in both development and production.
        app.UseCors("_myAllowSpecificOrigins");
        
        app.UseWhen(context =>
                // Register routes that do not require user authorization.
                !context.Request.Path.StartsWithSegments("/") &&
                !context.Request.Path.StartsWithSegments("/swagger") &&
                !context.Request.Path.StartsWithSegments("/api/v1/log/create") &&
                !context.Request.Path.StartsWithSegments("/api/v1/account/create") &&
                !context.Request.Path.StartsWithSegments("/api/v1/account/authenticate") &&
                !context.Request.Path.StartsWithSegments("/api/v1/application/authenticate"),
            
            // Require authentication for everything else.
            appBuilder => { appBuilder.UseMiddleware<AccountAuthorizationMiddleware>(); }
        );
        
        app.UseWhen(context =>
                // Register routes that require application authorization.
                context.Request.Path.StartsWithSegments("/api/v1/log/create"),

                // Require authentication for everything else.
            appBuilder => { appBuilder.UseMiddleware<AppAuthorizationMiddleware>(); }
        );
    }
}