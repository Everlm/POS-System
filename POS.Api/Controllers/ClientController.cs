using Microsoft.AspNetCore.Mvc;
using POS.Application.Commons.Bases.Request;
using POS.Application.Interfaces;

namespace POS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientApplication _clientApplication;

    public ClientController(IClientApplication clientApplication)
    {
        _clientApplication = clientApplication;
    }


    [HttpGet]
    public async Task<IActionResult> ListClients([FromQuery] BaseFiltersRequest filters)
    {
        var response = await _clientApplication.ListClient(filters);

        // if ((bool)filters.Download!)
        // {
        //     var columnNames = ExcelColumsNames.GetColumnsCategories();
        //     var fileBytes = _generateExcelApplication.GenerateToExcel(response.Data!, columnNames);
        //     return File(fileBytes, ContentType.ContentTypeExcel);
        // }

        return Ok(response);
    }

}
