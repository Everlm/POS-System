using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.API.CustomAttribute;
using POS.Application.Commons.Bases.Request;
using POS.Application.Dtos.Product.Request;
using POS.Application.Dtos.Purcharse.Request;
using POS.Application.Interfaces;
using POS.Application.Services;
using POS.Utilities.Static;

namespace POS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurcharseController : ControllerBase
    {
        private readonly IPurcharseApplication _purchaseApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public PurcharseController(IPurcharseApplication purchaseApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _purchaseApplication = purchaseApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        //[ApiKey] Ejemplo apikey
        //[Authorize(Policy = "ApiKeyPolicy")] Ejemplo de autorizacion por policy
        public async Task<IActionResult> ListPurchases([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _purchaseApplication.ListPurcharses(filters);

            if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumsNames.GetColumnsPurchases();
                var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpGet("{purcharseId:int}")]
        public async Task<IActionResult> PurcharseById(int purcharseId)
        {
            var response = await _purchaseApplication.PurcharseById(purcharseId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterPurcharse([FromBody] PurcharseRequestDto requestDto)
        {
            var response = await _purchaseApplication.CreatePurcharse(requestDto);
            return Ok(response);
        }

        [HttpPut("Cancel/{purcharseId:int}")]
        public async Task<IActionResult> RegisterPurcharse(int purcharseId)
        {
            var response = await _purchaseApplication.CancelPurcharse(purcharseId);
            return Ok(response);
        }
    }
}
