using Microsoft.AspNetCore.Mvc;
using POS.Application.Commons.Bases.Request;
using POS.Application.Dtos.Category.Request;
using POS.Application.Dtos.Client.Request;
using POS.Application.Interfaces;
using POS.Application.Services;
using POS.Utilities.Static;

namespace POS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientApplication _clientApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;

    public ClientController(IClientApplication clientApplication, IGenerateExcelApplication generateExcelApplication)
    {
        _clientApplication = clientApplication;
        _generateExcelApplication = generateExcelApplication;
    }


    /// <summary>
    /// Obtiene un listado de clientes con opción de exportar a Excel.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListClients([FromQuery] BaseFiltersRequest filters)
    {
        var response = await _clientApplication.ListClient(filters);

        if ((bool)filters.Download!)
        {
            var columnNames = ExcelColumsNames.GetColumnsCategories();
            var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
            return File(fileBytes, ContentType.ContentTypeExcel);
        }

        return Ok(response);
    }

    [HttpGet("Select")]
    public async Task<IActionResult> ListSelectClients()
    {
        var response = await _clientApplication.ListSelectClient();
        return Ok(response);
    }

    [HttpGet("{clientId:int}")]
    public async Task<IActionResult> GetClientById(int clientId)
    {
        var response = await _clientApplication.GetClientById(clientId);
        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateCategory([FromBody] ClientRequestDto requestDto)
    {
        var response = await _clientApplication.CreateClient(requestDto);
        return Ok(response);
    }

    [HttpPut("Update/{clientId:int}")]
    public async Task<IActionResult> UpdateClient(int clientId, [FromBody] ClientRequestDto requestDto)
    {
        var response = await _clientApplication.UpdateClient(requestDto, clientId);
        return Ok(response);
    }

    [HttpPut("Delete/{clientId:int}")]
    public async Task<IActionResult> DeleteClient(int clientId)
    {
        var response = await _clientApplication.DeleteClient(clientId);
        return Ok(response);
    }

}
