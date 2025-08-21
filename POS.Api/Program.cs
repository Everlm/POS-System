using Microsoft.AspNetCore.Authorization;
using POS.Api.Extensions;
using POS.Api.Hubs;
using POS.API.Authentication;
using POS.API.Extensions;
using POS.API.Middlewares;
using POS.Application.Extensions;
using POS.Application.Interfaces;
using POS.Infrastructure.Extensions;
using WatchDog;


var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
var Cors = "Cors";

builder.Services.AddInjectionInfrastructure(Configuration);
builder.Services.AddInjectionApplication(Configuration);
builder.Services.AddAuthentication(Configuration);
builder.Services.AddAppAuthorizationPolicies();

builder.Services.AddScoped<IAuthorizationHandler, ApiKeyHandler>();
builder.Services.AddScoped<ISignalRNotifierService, SignalRNotifierService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Cors,
    builder =>
    {
        builder.WithOrigins("http://localhost:4200", "https://tuproduccion.com", "http://localhost:8080")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});



var app = builder.Build();

// app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();


if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWatchDogExceptionLogger();

app.UseHttpsRedirection();

app.UseCors(Cors);

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseWatchDog(Configuration =>
{
    Configuration.WatchPageUsername = "EverDev";
    Configuration.WatchPagePassword = "123";

});


app.MapHub<AuthHub>("/authHub");

app.Run();

public partial class Program { }