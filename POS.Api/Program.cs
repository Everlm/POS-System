using Microsoft.AspNetCore.Authorization;
using POS.Api.Extensions;
using POS.API.Authentication;
using POS.API.Extensions;
using POS.API.Middlewares;
using POS.Application.Extensions;
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddHttpContextAccessor();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Cors,
    builder =>
    {
        builder.WithOrigins("*");
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors(Cors);

// app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSwagger();

//app.UseSwaggerUI();

app.UseWatchDogExceptionLogger();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseWatchDog(Configuration =>
{
    Configuration.WatchPageUsername = "EverDev";
    Configuration.WatchPagePassword = "123";

});

app.Run();

public partial class Program { }