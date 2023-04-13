using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.Business.Request;
using POS.Application.Interfaces;
using POS.Infrastructure.Commons.Bases.Request;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessApplication _businessApplication;

        public BusinessController(IBusinessApplication businessApplication)
        {
            _businessApplication = businessApplication;
        }

        [HttpPost]
        public async Task<IActionResult> ListBusiness([FromBody] BaseFiltersRequest filters)
        {
            var response = await _businessApplication.ListBusiness(filters);
            return Ok(response);
        }

        [HttpGet("{businessId:int}")]
        public async Task<IActionResult> GetBusinessById(int businessId)
        {
            var response = await _businessApplication.GetBusinessById(businessId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterBusiness([FromBody] BusinessRequestDto requestDto)
        {
            var response = await _businessApplication.RegisterBusiness(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{businessId:int}")]
        public async Task<IActionResult> EditBusiness(int businessId, [FromBody] BusinessRequestDto requestDto)
        {
            var response = await _businessApplication.EditBusiness(requestDto, businessId);
            return Ok(response);
        }

        [HttpPut("Delete/{businessId:int}")]
        public async Task<IActionResult> DeleteBusiness(int businessId)
        {
            var response = await _businessApplication.DeleteBusiness(businessId);
            return Ok(response);
        }
    }
}
