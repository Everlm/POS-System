using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.Category.Request;
using POS.Application.Dtos.Provider.Request;
using POS.Application.Interfaces;
using POS.Application.Services;
using POS.Infrastructure.Commons.Bases.Request;

namespace POS.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderApplication _providerApplication;

        public ProviderController(IProviderApplication providerApplication)
        {
            _providerApplication = providerApplication;
        }

        [HttpPost]
        public async Task<IActionResult> ListProviders([FromBody] BaseFiltersRequest filters)
        {
            var response = await _providerApplication.ListProviders(filters);
            return Ok(response);
        }

        [HttpGet("{providerId:int}")]
        public async Task<IActionResult> GetProviderById(int providerId)
        {
            var response = await _providerApplication.GetProviderById(providerId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterProvider([FromBody] ProviderRequestDto requestDto)
        {
            var response = await _providerApplication.RegisterProvider(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{providerId:int}")]
        public async Task<IActionResult> EditProvider([FromBody] ProviderRequestDto requestDto, int providerId)
        {
            var response = await _providerApplication.EditProvider(requestDto, providerId);
            return Ok(response);
        }

        [HttpPut("Delete/{providerId:int}")]
        public async Task<IActionResult> DeleteProvider(int providerId)
        {
            var response = await _providerApplication.DeleteProvider(providerId);
            return Ok(response);
        }
    }
}
