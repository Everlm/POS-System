using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.District.Request;
using POS.Application.Dtos.Province.Request;
using POS.Application.Interfaces;
using POS.Application.Services;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictApplication _districtApplication;

        public DistrictController(IDistrictApplication districtApplication)
        {
            _districtApplication = districtApplication;
        }

        [HttpPost]
        public async Task<IActionResult> ListDistricts([FromBody] BaseFiltersRequest filters)
        {
            var response = await _districtApplication.ListDistricts(filters);
            return Ok(response);
        }

        [HttpGet("{districtId:int}")]
        public async Task<IActionResult> GetDistrictById(int districtId)
        {
            var response = await _districtApplication.GetDistrictById(districtId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterDistrict([FromBody] DistrictRequestDto requestDto)
        {
            var response = await _districtApplication.RegisterDistrict(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{districtId:int}")]
        public async Task<IActionResult> EditDistrict(int districtId, [FromBody] DistrictRequestDto requestDto)
        {
            var response = await _districtApplication.EditDistrict(requestDto, districtId);
            return Ok(response);
        }

        [HttpPut("Delete/{districtId:int}")]
        public async Task<IActionResult> DeleteDistrict(int districtId)
        {
            var response = await _districtApplication.DeleteDistrict(districtId);
            return Ok(response);
        }
    }
}
