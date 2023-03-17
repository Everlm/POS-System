using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.Product.Request;
using POS.Application.Interfaces;
using POS.Infrastructure.Commons.Bases.Request;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductApplication _productApplication;

        public ProductController(IProductApplication productApplication)
        {
            _productApplication = productApplication;
        }

        [HttpPost]
        public async Task<IActionResult> ListProducts([FromBody] BaseFiltersRequest filters)
        {
            var response = await _productApplication.ListProducts(filters);
            return Ok(response);
        }

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var response = await _productApplication.GetProductById(productId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterProduct([FromBody] ProductRequestDto requestDto)
        {
            var response = await _productApplication.RegisterProduct(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{productId:int}")]
        public async Task<IActionResult> EditProduct([FromBody] ProductRequestDto requestDto, int productId)
        {
            var response = await _productApplication.EditProduct(requestDto, productId);
            return Ok(response);
        }

        [HttpPut("Delete/{productId:int}")]
        public async Task<IActionResult> DeleteProvider(int productId)
        {
            var response = await _productApplication.DeleteProduct(productId);
            return Ok(response);
        }
    }
}
