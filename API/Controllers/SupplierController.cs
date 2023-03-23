using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierServices _supplierServices;
        private ILogger _logger;
        public SupplierController(ISupplierServices services, ILogger<SupplierController> logger)
        {
            _supplierServices = services;
            _logger = logger;
        }
    }
}
