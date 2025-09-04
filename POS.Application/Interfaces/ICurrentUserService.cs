namespace POS.Application.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    int? UserId { get; }
    string? Email { get; }
    string? UserName { get; }
    IReadOnlyList<string> Roles { get; }
}
