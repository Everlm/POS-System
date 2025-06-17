using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace POS.Api.Hubs
{
    [Authorize]
    public class RoleUpdateHub : Hub
    {
        public async Task SendTestMessage(string message)
        {
            Console.WriteLine($"Mensaje de prueba recibido en el Hub: {message}");
            await Clients.All.SendAsync("ReceiveTestMessage", "Server", $"Echo: {message}");
        }

        public override async Task OnConnectedAsync()
        {
            // Opcional: Para propósitos de depuración o para rastrear usuarios conectados
            Console.WriteLine($"Cliente conectado a RoleUpdateHub: {Context.ConnectionId}");

            // Si tienes autenticación y el user ID está en los claims (NameIdentifier)
            // puedes añadir el usuario a un grupo específico del usuario para enviarle mensajes directamente
            string? userEmail = Context.UserIdentifier; // Obtiene el ID del usuario autenticado
            if (!string.IsNullOrEmpty(userEmail))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userEmail);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Cliente desconectado de RoleUpdateHub: {Context.ConnectionId}");
            Debug.WriteLine("Desconexión del cliente: " + Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}