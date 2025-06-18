
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using POS.Application.Interfaces;

namespace POS.Api.Hubs
{
    [Authorize]
    public class AuthHub : Hub
    {
        private readonly ILogger<AuthHub> _logger;

        public AuthHub(ILogger<AuthHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            string? userEmail = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userEmail))
            {
                _logger.LogInformation($"Cliente conectado a AuthHub: {Context.ConnectionId} con email {userEmail}");
                await Groups.AddToGroupAsync(Context.ConnectionId, userEmail);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Cliente desconectado de AuthHub: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}