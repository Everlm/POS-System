using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.Sale.Request;
using POS.Application.Interfaces;
using POS.Infrastructure.Commons.Bases.Request;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleApplication _saleApplication;

        public SaleController(ISaleApplication saleApplication)
        {
            _saleApplication = saleApplication;
        }


        [HttpPost]
        public async Task<IActionResult> ListSales([FromBody] BaseFiltersRequest filters)
        {
            var response = await _saleApplication.ListSales(filters);
            return Ok(response);
        }

        [HttpGet("{saletId:int}")]
        public async Task<IActionResult> GetSaleById(int saletId)
        {
            var response = await _saleApplication.GetSaleById(saletId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterSale([FromBody] SaleRequestDto requestDto)
        {
            var response = await _saleApplication.RegisterSale(requestDto);
            return Ok(response);
        }
    }
}
