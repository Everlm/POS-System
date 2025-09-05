using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.Category.Request;
using POS.Application.Interfaces;

namespace POS.API.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    // [Authorize]
    // [AllowAnonymous]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryApplication _categoryApplication;

        public CategoryController(ICategoryApplication categoryApplication)
        {
            _categoryApplication = categoryApplication;

        }

        [HttpGet("sp/Select")]
        public async Task<IActionResult> SPListSelectCategories()
        {
            // var isAuthenticated = User.Identity?.IsAuthenticated;
            // var email1 = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            // var userName1 = User.FindFirstValue(ClaimTypes.Surname);
            // var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            // var claims = User.Claims;

            var response = await _categoryApplication.SPListSelectCategories();
            return Ok(response);
        }

        [HttpGet("sp/{categoryId:int}")]
        public async Task<IActionResult> SPGetCategoryById(int categoryId)
        {
            var response = await _categoryApplication.SPGetCategoryById(categoryId);
            return Ok(response);
        }

        [HttpPost("sp/create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDto requestDto)
        {
            var response = await _categoryApplication.SPCreateCategory(requestDto);
            return Ok(response);
        }
        [HttpPut("sp/Update/{categoryId:int}")]
        public async Task<IActionResult> SPUpdateCategory(int categoryId, [FromBody] CategoryRequestDto requestDto)
        {
            var response = await _categoryApplication.SPUpdateCategory(requestDto, categoryId);
            return Ok(response);
        }

        [HttpPut("sp/soft-delete/{categoryId:int}")]
        public async Task<IActionResult> SPDeleteCategory(int categoryId)
        {
            var response = await _categoryApplication.SPDeleteCategory(categoryId);
            return Ok(response);
        }

        [HttpDelete("sp/hard-delete/{categoryId:int}")]
        public async Task<IActionResult> SPHardDeleteCategory(int categoryId)
        {
            var response = await _categoryApplication.SPHardDeleteCategory(categoryId);
            return Ok(response);
        }
    }
}
