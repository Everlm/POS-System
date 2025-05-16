using Microsoft.AspNetCore.Mvc;
using POS.Application.Commons.Bases.Request;
using POS.Application.Dtos.Sale.Request;
using POS.Application.Interfaces;
using POS.Utilities.Static;

namespace POS.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleApplication _saleApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public SaleController(ISaleApplication saleApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _saleApplication = saleApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListSale([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _saleApplication.ListSale(filters);

            if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumsNames.GetColumnsSale();
                var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpGet("ProductStockByWarehouse")]
        public async Task<IActionResult> GetProductStockByWarehouseId([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _saleApplication.GetProductStockByWarehouse(filters);
            return Ok(response);
        }

        [HttpGet("{saleId:int}")]
        public async Task<IActionResult> SaleById(int saleId)
        {
            var response = await _saleApplication.GetSaleById(saleId);
            return Ok(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateSale([FromBody] SaleRequestDto requestDto)
        {
            var response = await _saleApplication.CreateSale(requestDto);
            return Ok(response);
        }

        [HttpPut("Cancel/{saleId:int}")]
        public async Task<IActionResult> CancelPurcharse(int saleId)
        {
            var response = await _saleApplication.CancelSale(saleId);
            return Ok(response);
        }


    }
}
