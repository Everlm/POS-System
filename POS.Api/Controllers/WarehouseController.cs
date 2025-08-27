using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Commons.Bases.Request;
using POS.Application.Dtos.Warehouse.Request;
using POS.Application.Interfaces;
using POS.Utilities.Static;

namespace POS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseApplication _warehouseApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public WarehouseController(IWarehouseApplication warehouseApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _warehouseApplication = warehouseApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListWarehouses([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _warehouseApplication.ListWarehouses(filters);

            if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumsNames.GetColumnsWarehouses();
                var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpGet("Select")]
        public async Task<IActionResult> ListSelectWarehouse()
        {
            var response = await _warehouseApplication.GetSelectWarehouse();
            return Ok(response);
        }

        [HttpGet("{warehouseId:int}")]
        public async Task<IActionResult> GetWarehouseById(int warehouseId)
        {
            var response = await _warehouseApplication.GetWarehouseById(warehouseId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterWarehouse([FromBody] WarehouseRequestDto requestDto)
        {
            var response = await _warehouseApplication.RegisterWarehouse(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{warehouseId:int}")]
        public async Task<IActionResult> EditWarehouse([FromBody] WarehouseRequestDto requestDto, int warehouseId)
        {
            var response = await _warehouseApplication.EditWarehouse(requestDto, warehouseId);
            return Ok(response);
        }

        [HttpPut("Remove/{warehouseId:int}")]
        public async Task<IActionResult> RemoveWarehouse(int warehouseId)
        {
            var response = await _warehouseApplication.RemoveWarehouse(warehouseId);
            return Ok(response);
        }
    }
}
