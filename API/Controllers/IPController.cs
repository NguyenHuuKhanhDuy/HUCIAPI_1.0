using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPController : ControllerBase
    {
        private readonly IIPServices _ipServices;
        private readonly ILogger _logger;
        public IPController(IIPServices ipServices, ILogger<IIPServices> logger)
        {
            _ipServices = ipServices;
            _logger = logger;
        }
    }
}
