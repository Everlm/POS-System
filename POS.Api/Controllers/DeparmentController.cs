using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.Department.Request;
using POS.Application.Interfaces;
using POS.Infrastructure.Commons.Bases.Request;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeparmentController : ControllerBase
    {
        private readonly IDepartmentApplication _departmentApplication;

        public DeparmentController(IDepartmentApplication departmentApplication)
        {
            _departmentApplication = departmentApplication;
        }


        [HttpPost]
        public async Task<IActionResult> ListDepartments([FromBody] BaseFiltersRequest filters)
        {
            var response = await _departmentApplication.ListDepartment(filters);
            return Ok(response);
        }

        [HttpGet("{departmentId:int}")]
        public async Task<IActionResult> GetDepartmentById(int departmentId)
        {
            var response = await _departmentApplication.GetDepartmentById(departmentId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterDepartment([FromBody] DeparmentRequestDto requestDto)
        {
            var response = await _departmentApplication.RegisterDepartment(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{departmentId:int}")]
        public async Task<IActionResult> EditDepartment(int departmentId, [FromBody] DeparmentRequestDto requestDto)
        {
            var response = await _departmentApplication.EditDepartment(requestDto, departmentId);
            return Ok(response);
        }

        [HttpPut("Delete/{departmentId:int}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            var response = await _departmentApplication.DeleteDepartment(departmentId);
            return Ok(response);
        }
    }
}
