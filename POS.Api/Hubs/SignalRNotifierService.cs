using Microsoft.AspNetCore.SignalR;
using POS.Application.Interfaces;

namespace POS.Api.Hubs
{
    public class SignalRNotifierService : ISignalRNotifierService
    {
        private readonly IHubContext<AuthHub> _hubContext;
        private readonly ILogger<SignalRNotifierService> _logger;

        public SignalRNotifierService(IHubContext<AuthHub> hubContext, ILogger<SignalRNotifierService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyUserRolesChanged(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogWarning("userEmail es nulo o vacío");
                return;
            }

            try
            {
                var signal = "Esto es lo que viene del servidor: Roles actualizados";
                await _hubContext.Clients.User(userEmail).SendAsync("RolesUpdated", signal);
                _logger.LogInformation($"Notificación enviada a {userEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al notificar a {userEmail}: {ex.Message}");
                throw;
            }

        }
    }
}