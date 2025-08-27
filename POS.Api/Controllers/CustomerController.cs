using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Commons.Bases.Request;
using POS.Application.Dtos.Client.Request;
using POS.Application.Interfaces;
using POS.Utilities.Static;

namespace POS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IClientApplication _clientApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;

    public CustomerController(IClientApplication clientApplication, IGenerateExcelApplication generateExcelApplication)
    {
        _clientApplication = clientApplication;
        _generateExcelApplication = generateExcelApplication;
    }


    /// <summary>
    /// Obtiene un listado de clientes con opci�n de exportar a Excel.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListCustomers([FromQuery] BaseFiltersRequest filters)
    {
        var response = await _clientApplication.ListClient(filters);
        bool isAdmin = User.IsInRole("Admin");

        if ((bool)filters.Download!)
        {
            var columnNames = ExcelColumsNames.GetColumnsClients();
            var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
            return File(fileBytes, ContentType.ContentTypeExcel);
        }

        return Ok(response);
    }

    [HttpGet("Select")]
    public async Task<IActionResult> ListSelectCustomer()
    {
        var response = await _clientApplication.ListSelectClient();
        return Ok(response);
    }

    [HttpGet("{customerId:int}")]
    public async Task<IActionResult> GetCustomerById(int customerId)
    {
        var response = await _clientApplication.GetClientById(customerId);
        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateCustomer([FromBody] ClientRequestDto requestDto)
    {
        var response = await _clientApplication.CreateClient(requestDto);
        return Ok(response);
    }

    [HttpPut("Update/{customerId:int}")]
    public async Task<IActionResult> UpdateCustomer(int customerId, [FromBody] ClientRequestDto requestDto)
    {
        var response = await _clientApplication.UpdateClient(requestDto, customerId);
        return Ok(response);
    }

    [HttpPut("Delete/{customerId:int}")]
    public async Task<IActionResult> DeleteCustomer(int customerId)
    {
        var response = await _clientApplication.DeleteClient(customerId);
        return Ok(response);
    }

}
