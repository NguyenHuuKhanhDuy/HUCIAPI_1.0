using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeService;
        public EmployeeController(IEmployeeServices employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateEmployee([FromForm] EmployeeVM employee)
        {
            return Ok();
        }
    }
}
