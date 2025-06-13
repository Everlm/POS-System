
using Microsoft.AspNetCore.SignalR;
using POS.Application.Interfaces;

namespace POS.Api.Hubs
{
    public class SignalRNotifierService : INotifierService
    {
        private readonly IHubContext<RoleUpdateHub> _hubContext;

        public SignalRNotifierService(IHubContext<RoleUpdateHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyUserRolesChanged(string userEmail, List<string> roles)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                Console.WriteLine("Advertencia: userEmail es nulo o vacío, no se puede enviar notificación específica.");
                return;
            }

            // Envía la señal al cliente específico.
            // "ReceiveRoleUpdate" es el nombre del método que el cliente Angular escuchará.
            await _hubContext.Clients.User(userEmail).SendAsync("ReceiveRoleUpdate", roles);

            Console.WriteLine($"SignalR: Notificación de roles enviada a usuario con email {userEmail} con roles: {string.Join(", ", roles)}");

        }
    }
}