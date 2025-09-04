using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using POS.Application.Interfaces;

namespace POS.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;


    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public int? UserId =>
        IsAuthenticated && int.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
            ? userId
            : null;


    public string? Email =>
        IsAuthenticated ? User?.FindFirstValue(ClaimTypes.Email) : null;

    public string? UserName =>
        IsAuthenticated ? User?.FindFirstValue(ClaimTypes.Name) : null;

    public IReadOnlyList<string> Roles =>
        IsAuthenticated
            ? User?.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList() ?? new List<string>()
            : Array.Empty<string>();
}
