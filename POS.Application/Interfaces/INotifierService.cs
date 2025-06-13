
namespace POS.Application.Interfaces
{
    public interface INotifierService
    {
        Task NotifyUserRolesChanged(string userEmail, List<string> roles);
    }
}