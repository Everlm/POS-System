using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Commons.Bases.Request;
using POS.Application.Dtos.Provider.Request;
using POS.Application.Interfaces;
using POS.Application.Services;
using POS.Utilities.Static;

namespace POS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderApplication _providerApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public ProviderController(IProviderApplication providerApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _providerApplication = providerApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListProviders([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _providerApplication.ListProviders(filters);

            if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumsNames.GetColumnsProviders();
                var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpGet("Select")]
        public async Task<IActionResult> ListSelectProviders()
        {
            var response = await _providerApplication.GetAllProviders();
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
