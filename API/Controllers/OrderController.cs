using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly ILogger _logger;
        public OrderController(IOrderServices orderServices, ILogger logger)
        {
            _orderServices = orderServices;
            _logger = logger;
        }
    }
}
