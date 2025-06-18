
namespace POS.Application.Interfaces
{
    public interface ISignalRNotifierService
    {
        Task NotifyUserRolesChanged(string userEmail);
    }
}