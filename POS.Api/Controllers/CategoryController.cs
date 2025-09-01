using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Commons.Bases.Request;
using POS.Application.Dtos.Category.Request;
using POS.Application.Interfaces;
using POS.Utilities.Static;

namespace POS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    // [AllowAnonymous]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryApplication _categoryApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public CategoryController(ICategoryApplication categoryApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _categoryApplication = categoryApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet("GenerateCategoriesPdfDocument")]
        public async Task<IActionResult> GenerateCategoriesPdfDocument()
        {
            var response = await _categoryApplication.GenerateCategoriesPdfDocument();

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return File(response.Data!, "application/pdf", "CategoriesReport.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> ListCategories([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _categoryApplication.ListCategories(filters);

            if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumsNames.GetColumnsCategories();
                var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpGet("Select")]
        //[ApiKey]
        //[Authorize(Policy = "ApiKeyPolicy")] Ejemplo de autorizacion por policy
        // [Authorize(Roles = AppRoles.Admin)]
        // [Authorize(Policy = AppPolicies.RequireAdminRole)]
        public async Task<IActionResult> ListSelectCategories()
        {
            var response = await _categoryApplication.ListSelectCategories();
            return Ok(response);
        }


        [HttpGet("{categoryId:int}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var response = await _categoryApplication.GetCategoryById(categoryId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterCategory([FromBody] CategoryRequestDto requestDto)
        {
            var response = await _categoryApplication.RegisterCategory(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{categoryId:int}")]
        public async Task<IActionResult> EditCategory(int categoryId, [FromBody] CategoryRequestDto requestDto)
        {
            var response = await _categoryApplication.EditCategory(requestDto, categoryId);
            return Ok(response);
        }

        [HttpPut("Delete/{categoryId:int}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var response = await _categoryApplication.DeleteCategory(categoryId);
            return Ok(response);
        }
    }
}
