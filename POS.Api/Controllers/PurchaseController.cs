using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.Purchase.Request;
using POS.Application.Interfaces;
using POS.Infrastructure.Commons.Bases.Request;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseApplication _purchaseApplication;

        public PurchaseController(IPurchaseApplication purchaseApplication)
        {
            _purchaseApplication = purchaseApplication;
        }

        [HttpPost]
        public async Task<IActionResult> ListPurchases([FromBody] BaseFiltersRequest filters)
        {
            var response = await _purchaseApplication.ListPurchase(filters);
            return Ok(response);
        }

        [HttpGet("{purchaseId:int}")]
        public async Task<IActionResult> GetPurchaseById(int purchaseId)
        {
            var response = await _purchaseApplication.GetPurchaseById(purchaseId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterPurchase([FromBody] PurchaseRequestDto requestDto)
        {
            var response = await _purchaseApplication.RegisterPurchase(requestDto);
            return Ok(response);
        }
    }
}
