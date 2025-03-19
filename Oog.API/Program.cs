using API.Common;
using API.Common.Databases;
using API.v1.Account.AuthenticateAccount;
using API.v1.Account.AuthenticateAccount.Interfaces;
using API.v1.Account.CreateAccount;
using API.v1.Account.CreateAccount.Interfaces;
using API.v1.Environment.AddAccountToEnvironment;
using API.v1.Environment.AddAccountToEnvironment.Interfaces;
using API.v1.Environment.AddAccountToEnvironment.Requests;
using API.v1.Environment.CreateEnvironment;
using API.v1.Environment.CreateEnvironment.Interfaces;
using API.v1.Environment.ReadEnvironment;
using API.v1.Environment.ReadEnvironment.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add database connections.
builder.Services.AddScoped<CoreDbConnection>();
builder.Services.AddScoped<LogDbConnection>();

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

builder.RegisterServices();

var app = builder.Build();

app.RegisterMiddlewares(builder.Configuration);

using var scope = app.Services.CreateScope();

// Get handlers from DI container.
var createAccountHandler = scope.ServiceProvider.GetRequiredService<ICreateAccountHandler>();
var authenticateAccountHandler = scope.ServiceProvider.GetRequiredService<IAuthenticateAccountHandler>();
var createEnvironmentHandler = scope.ServiceProvider.GetRequiredService<ICreateEnvironmentHandler>();
var addAccountToEnvHandler = scope.ServiceProvider.GetRequiredService<IAddAccountToEnvHandler>();

// Get repositories from DI container.
var readEnvironmentRepository = scope.ServiceProvider.GetRequiredService<IReadEnvironmentRepository>();

// Map endpoints and pass the handlers.
app.MapCreateAccountEndpoints(createAccountHandler);
app.MapAuthenticateAccountEndpoints(authenticateAccountHandler);
app.MapCreateEnvironmentEndpoints(createEnvironmentHandler);
app.MapReadEnvironmentEndpoints(readEnvironmentRepository);
app.MapAddAccountToEnvEndpoints(addAccountToEnvHandler);

app.Run();