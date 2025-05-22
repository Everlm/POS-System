using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Interfaces;

namespace POS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VoucherDoumentTypeController : ControllerBase
{
    private readonly IVoucherDoumentTypeApplication _voucherDoumentTypeApplication;

    public VoucherDoumentTypeController(IVoucherDoumentTypeApplication voucherDoumentTypeApplication)
    {
        _voucherDoumentTypeApplication = voucherDoumentTypeApplication;
    }


    [HttpGet("Select")]
    public async Task<IActionResult> ListSelectVoucherDocumentTypes()
    {
        var response = await _voucherDoumentTypeApplication.GetAllVoucherDoumentType();
        return Ok(response);
    }
}
