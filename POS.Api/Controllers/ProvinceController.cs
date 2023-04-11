using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.Department.Request;
using POS.Application.Dtos.Province.Request;
using POS.Application.Interfaces;
using POS.Infrastructure.Commons.Bases.Request;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private IProvinceApplication _ProvinceApplication;

        public ProvinceController(IProvinceApplication provinceApplication)
        {
            _ProvinceApplication = provinceApplication;
        }

        [HttpPost]
        public async Task<IActionResult> ListProvinces([FromBody] BaseFiltersRequest filters)
        {
            var response = await _ProvinceApplication.ListProvinces(filters);
            return Ok(response);
        }

        [HttpGet("{provinceId:int}")]
        public async Task<IActionResult> GetProvinceById(int provinceId)
        {
            var response = await _ProvinceApplication.GetProvinceById(provinceId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterProvince([FromBody] ProvinceRequestDto requestDto)
        {
            var response = await _ProvinceApplication.Registerprovince(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{provinceId:int}")]
        public async Task<IActionResult> EditProvince(int provinceId, [FromBody] ProvinceRequestDto requestDto)
        {
            var response = await _ProvinceApplication.EditProvince(requestDto, provinceId);
            return Ok(response);
        }

        [HttpPut("Delete/{provinceId:int}")]
        public async Task<IActionResult> DeleteProvince(int provinceId)
        {
            var response = await _ProvinceApplication.DeleteProvince(provinceId);
            return Ok(response);
        }
    }
}
