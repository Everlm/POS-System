using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.Client.Request;
using POS.Application.Dtos.Provider.Request;
using POS.Application.Interfaces;
using POS.Application.Services;
using POS.Infrastructure.Commons.Bases.Request;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientApplication _clientApplication;

        public ClientController(IClientApplication clientApplication)
        {
            _clientApplication = clientApplication;
        }

        [HttpPost]
        public async Task<IActionResult> ListClients([FromBody] BaseFiltersRequest filters)
        {
            var response = await _clientApplication.ListClients(filters);
            return Ok(response);
        }

        [HttpGet("{clientId:int}")]
        public async Task<IActionResult> GetClientById(int clientId)
        {
            var response = await _clientApplication.GetClientById(clientId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterClient([FromBody] ClientRequestDto requestDto)
        {
            var response = await _clientApplication.Registerclient(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{clientId:int}")]
        public async Task<IActionResult> EditClient([FromBody] ClientRequestDto requestDto, int clientId)
        {
            var response = await _clientApplication.EditClient(requestDto, clientId);
            return Ok(response);
        }

        [HttpPut("Delete/{clientId:int}")]
        public async Task<IActionResult> DeleteClient(int clientId)
        {
            var response = await _clientApplication.DeleteClient(clientId);
            return Ok(response);
        }
    }
}
