using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if !DEBUG    
    [Authorize]
#endif
    public class LocationController : BaseController
    {
        private readonly ILocationServices _locationServices;
        private readonly ILogger _logger;
        public LocationController(ILocationServices locationServices, ILogger<LocationController> logger)
        {
            _locationServices = locationServices;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetLocationByIdParentAsync(int parentId)
        {
            _logger.LogInformation($"Start get location by parent id: {parentId}");

            var locations = await _locationServices.GetLocationByIdParentAsync(parentId);

            _logger.LogInformation($"End get location by parent id: {parentId}");

            return HandleResponseStatusOk(locations);
        }
    }
}
