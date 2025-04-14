using API.Common;
using API.Common.Databases;
using API.v1.api.Account.CreateAccount;
using API.v1.api.Account.CreateAccount.Interfaces;
using API.v1.api.Application.CreateApplication;
using API.v1.api.Application.CreateApplication.Interfaces;
using API.v1.api.Authentication.AuthenticateAccount;
using API.v1.api.Authentication.AuthenticateAccount.Interfaces;
using API.v1.api.Authentication.AuthenticateApplication;
using API.v1.api.Authentication.AuthenticateApplication.Interfaces;
using API.v1.api.Environment.AddAccountToEnvironment;
using API.v1.api.Environment.AddAccountToEnvironment.Interfaces;
using API.v1.api.Environment.CreateEnvironment;
using API.v1.api.Environment.CreateEnvironment.Interfaces;
using API.v1.api.Environment.GetAccountsFromEnvironment;
using API.v1.api.Environment.GetAccountsFromEnvironment.Interfaces;
using API.v1.api.Environment.ReadEnvironment;
using API.v1.api.Environment.ReadEnvironment.Interfaces;
using API.v1.api.Log.CreateLog;
using API.v1.api.Log.CreateLog.Interfaces;
using API.v1.api.Role.CreateRole;
using API.v1.api.Role.CreateRole.Interfaces;
using API.v1.rtes;
using API.v1.rtes.Connection;
using API.v1.rtes.Connection.Interfaces;
using API.v1.rtes.Hubs.Log;

var builder = WebApplication.CreateBuilder(args);

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Add database connections.
builder.Services.AddScoped<CoreDbConnection>();
builder.Services.AddScoped<LogDbConnection>();

// SignalR connection management.
builder.Services.AddSingleton<IClientConnectionHandler, ClientConnectionHandler>();
builder.Services.AddSingleton<IClientConnectionRepository, ClientConnectionRepository>();

// Register SignalR hubs.
builder.Services.AddScoped<LogHub>();

// Register repositories and handlers.
builder.Services.AddScoped<ICreateAccountHandler, CreateAccountHandler>();
builder.Services.AddScoped<ICreateAccountRepository, CreateAccountRepository>();

builder.Services.AddScoped<IAuthenticateAccountHandler, AuthenticateAccountHandler>();
builder.Services.AddScoped<IAuthenticateAccountRepository, AuthenticateAccountRepository>();

builder.Services.AddScoped<ICreateEnvironmentHandler, CreateEnvironmentHandler>();
builder.Services.AddScoped<ICreateEnvironmentRepository, CreateEnvironmentRepository>();

builder.Services.AddScoped<IReadEnvironmentRepository, ReadEnvironmentRepository>();

builder.Services.AddScoped<IAddAccountToEnvHandler, AddAccountToEnvHandler>();
builder.Services.AddScoped<IAddAccountToEnvRepository, AddAccountToEnvRepository>();

builder.Services.AddScoped<IGetAccountsFromEnvHandler, GetAccountsFromEnvHandler>();
builder.Services.AddScoped<IGetAccountsFromEnvRepository, GetAccountsFromEnvRepository>();

builder.Services.AddScoped<ICreateAppHandler, CreateAppHandler>();
builder.Services.AddScoped<ICreateAppRepository, CreateAppRepository>();

builder.Services.AddScoped<IAuthenticateAppHandler, AuthenticateAppHandler>();
builder.Services.AddScoped<IAuthenticateAppRepository, AuthenticateAppRepository>();

builder.Services.AddScoped<ICreateLogHandler, CreateLogHandler>();
builder.Services.AddScoped<ICreateLogRepository, CreateLogRepository>();

builder.Services.AddScoped<ICreateRoleHandler, CreateRoleHandler>();
builder.Services.AddScoped<ICreateRoleRepository, CreateRoleRepository>();

builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares();

using var scope = app.Services.CreateScope();

// Get handlers from DI container.
var createAccountHandler = scope.ServiceProvider.GetRequiredService<ICreateAccountHandler>();
var authenticateAccountHandler = scope.ServiceProvider.GetRequiredService<IAuthenticateAccountHandler>();
var authenticateAppHandler = scope.ServiceProvider.GetRequiredService<IAuthenticateAppHandler>();
var createEnvironmentHandler = scope.ServiceProvider.GetRequiredService<ICreateEnvironmentHandler>();
var addAccountToEnvHandler = scope.ServiceProvider.GetRequiredService<IAddAccountToEnvHandler>();
var getAccountsFromEnvHandler = scope.ServiceProvider.GetRequiredService<IGetAccountsFromEnvHandler>();
var createAppHandler = scope.ServiceProvider.GetRequiredService<ICreateAppHandler>();
var createLogHandler = scope.ServiceProvider.GetRequiredService<ICreateLogHandler>();
var createRoleHandler = scope.ServiceProvider.GetRequiredService<ICreateRoleHandler>();

// Get repositories from DI container.
var readEnvironmentRepository = scope.ServiceProvider.GetRequiredService<IReadEnvironmentRepository>();

// Map endpoints and pass the handlers.
app.MapCreateAccountEndpoints(createAccountHandler);
app.MapAuthenticateAccountEndpoints(authenticateAccountHandler);
app.MapCreateEnvironmentEndpoints(createEnvironmentHandler);
app.MapReadEnvironmentEndpoints(readEnvironmentRepository);
app.MapAddAccountToEnvEndpoints(addAccountToEnvHandler);
app.MapGetAccountsFromEnvController(getAccountsFromEnvHandler);
app.MapCreateAppEndpoints(createAppHandler);
app.MapAuthenticateAppEndpoints(authenticateAppHandler);
app.MapCreateLogEndpoints(createLogHandler);
app.MapCreateRoleEndpoints(createRoleHandler);

// Map SignalR hubs.
app.MapHub<LogHub>("/rtes/v1/log");

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    await next();
});

app.Run();
