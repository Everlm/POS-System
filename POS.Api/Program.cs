using POS.Api.Extensions;
using POS.Api.Hubs;
using POS.API.Extensions;
using POS.API.Middlewares;
using POS.Application.Extensions;
using POS.Infrastructure.Extensions;
using QuestPDF.Infrastructure;
using WatchDog;

#region Services

var builder = WebApplication.CreateBuilder(args);
var Cors = "Cors";
var Configuration = builder.Configuration;
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddInjectionInfrastructure(Configuration);
builder.Services.AddInjectionApplication(Configuration);
builder.Services.AddAuthentication(Configuration);
builder.Services.AddAppAuthorizationPolicies();
builder.Services.AddDefaultServices(Cors);
builder.Services.AddVersioningAPI();
builder.Services.AddSwagger();
builder.Services.AddAppScopedServices();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

#endregion

#region Middleware

var app = builder.Build();

// app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithVersioning();
    app.ApplyMigrations();
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
app.MapGet("/health", () => Results.Ok("OK"));

app.Run();

#endregion

public partial class Program { }