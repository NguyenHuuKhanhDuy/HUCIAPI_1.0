using ApplicationCore.Helper;
using ApplicationCore.ViewModels;
using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        public readonly IEmployeeServices _employeeServices;
        private readonly IConfiguration _configuration;

        public EmployeeController(IEmployeeServices employeeServices, IConfiguration configuration)
        {
            _employeeServices = employeeServices;
            _configuration = configuration;
        }

        
    }
}
