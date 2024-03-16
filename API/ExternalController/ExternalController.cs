using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.ExternalController;

public class ExternalController : BaseController
{
    private readonly IExternal _externalServices;
    private readonly ILogger<ExternalController> _logger;
    public ExternalController(IExternal external, ILogger<ExternalController> logger)
    {
        _externalServices = external;
        _logger = logger;
    }

    [HttpGet]
    [Route("customer")]
    public async Task<IActionResult> GetPointCustomer(string phoneNumber)
    {
        var response = await _externalServices.GetCustomer(phoneNumber);
        return HandleResponseStatusOk(response);
    }
}