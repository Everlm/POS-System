using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.BranchOffice.Request;
using POS.Application.Dtos.Business.Request;
using POS.Application.Interfaces;
using POS.Application.Services;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchOfficeController : ControllerBase
    {
        private readonly IBranchOfficeApplication _branchOfficeApplication;

        public BranchOfficeController(IBranchOfficeApplication branchOfficeApplication)
        {
            _branchOfficeApplication = branchOfficeApplication;
        }

        [HttpPost]
        public async Task<IActionResult> ListBranchOffice([FromBody] BaseFiltersRequest filters)
        {
            var response = await _branchOfficeApplication.ListBranchOffices(filters);
            return Ok(response);
        }

        [HttpGet("{BranchOfficeId:int}")]
        public async Task<IActionResult> GetBranchOfficeById(int BranchOfficeId)
        {
            var response = await _branchOfficeApplication.GetBranchOfficeById(BranchOfficeId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterBranchOffice([FromBody] BranchOfficeRequestDto requestDto)
        {
            var response = await _branchOfficeApplication.RegisterBranchOffice(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{BranchOfficeId:int}")]
        public async Task<IActionResult> EditBranchOffice([FromBody] BranchOfficeRequestDto requestDto, int BranchOfficeId)
        {
            var response = await _branchOfficeApplication.EditBranchOffice(requestDto, BranchOfficeId);
            return Ok(response);
        }

        [HttpPut("Delete/{BranchOfficeId:int}")]
        public async Task<IActionResult> DeleteBranchOffice(int BranchOfficeId)
        {
            var response = await _branchOfficeApplication.DeleteBranchOffice(BranchOfficeId);
            return Ok(response);
        }
    }
}
