using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Commons.Bases.Request;
using POS.Application.Dtos.Category.Request;
using POS.Application.Interfaces;
using POS.Utilities.Static;

namespace POS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //Aqui no funciona por la ruta [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
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

        /// <summary>
        /// Genera un documento PDF con la lista de categorías.
        /// </summary>
        /// <returns>Archivo PDF de categorías.</returns>
        /// <response code="200">Devuelve el archivo PDF generado.</response>
        [HttpGet("GenerateCategoriesPdfDocument")]
        public async Task<IActionResult> GenerateCategoriesPdfDocument()
        {
            var response = await _categoryApplication.GenerateCategoriesPdfDocument();

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return File(response.Data!, ContentType.ContentTypePdf, "CategoriesReport.pdf");
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

        // [HttpGet("sp/Select")]
        // public async Task<IActionResult> SPListSelectCategories()
        // {
        //     // var isAuthenticated = User.Identity?.IsAuthenticated;
        //     // var email1 = User.FindFirstValue(ClaimTypes.NameIdentifier); 
        //     // var userName1 = User.FindFirstValue(ClaimTypes.Surname);
        //     // var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        //     // var claims = User.Claims;

        //     var response = await _categoryApplication.SPListSelectCategories();
        //     return Ok(response);
        // }

        // [HttpGet("sp/{categoryId:int}")]
        // public async Task<IActionResult> SPGetCategoryById(int categoryId)
        // {
        //     var response = await _categoryApplication.SPGetCategoryById(categoryId);
        //     return Ok(response);
        // }

        // [HttpPost("sp/create")]
        // public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDto requestDto)
        // {
        //     var response = await _categoryApplication.SPCreateCategory(requestDto);
        //     return Ok(response);
        // }
        // [HttpPut("sp/Update/{categoryId:int}")]
        // public async Task<IActionResult> SPUpdateCategory(int categoryId, [FromBody] CategoryRequestDto requestDto)
        // {
        //     var response = await _categoryApplication.SPUpdateCategory(requestDto, categoryId);
        //     return Ok(response);
        // }

        // [HttpPut("sp/soft-delete/{categoryId:int}")]
        // public async Task<IActionResult> SPDeleteCategory(int categoryId)
        // {
        //     var response = await _categoryApplication.SPDeleteCategory(categoryId);
        //     return Ok(response);
        // }

        // [HttpDelete("sp/hard-delete/{categoryId:int}")]
        // public async Task<IActionResult> SPHardDeleteCategory(int categoryId)
        // {
        //     var response = await _categoryApplication.SPHardDeleteCategory(categoryId);
        //     return Ok(response);
        // }
    }
}
