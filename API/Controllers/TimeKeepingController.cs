using ApplicationCore.ViewModels.TimeKeeping;
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
    public class TimeKeepingController : BaseController
    {
        private readonly ITimeKeepingServices _timekeepingServices;
        private readonly ILogger _logger;
        public TimeKeepingController(ITimeKeepingServices timekeepingServices, ILogger<TimeKeepingController> logger)
        {
            _timekeepingServices = timekeepingServices;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateTimeKeepingAsync(TimeKeepingVM vm)
        {
            _logger.LogInformation($"Start create time keeping... {GetStringFromJson(vm)}");

            var timekeeping = await _timekeepingServices.CreateTimeKeepingAsync(vm);

            _logger.LogInformation($"End create time keeping... {GetStringFromJson(timekeeping)}");

            return HandleResponseStatusOk(timekeeping);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteTimeKeepingAsync(Guid timeKeepingId)
        {
            _logger.LogInformation($"Start delete time keeping with Id: {timeKeepingId}");

            await _timekeepingServices.DeleteTimeKeepingAsync(timeKeepingId);

            _logger.LogInformation($"End create time keeping with Id: {timeKeepingId}");

            return HandleResponseStatusOk();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllTimeKeepingAsync()
        {
            _logger.LogInformation($"Start get all time keeping...");

            var timekeepings = await _timekeepingServices.GetAllTimeKeepingAsync();

            _logger.LogInformation($"End get all time keeping... {GetStringFromJson(timekeepings)}");

            return HandleResponseStatusOk(timekeepings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllTimeKeepingByDateAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation($"Start get all time keeping from {startDate} to {endDate}...");

            var timekeepings = await _timekeepingServices.GetAllTimeKeepingByDateAsync(startDate, endDate);

            _logger.LogInformation($"End get all time keeping... {GetStringFromJson(timekeepings)}");

            return HandleResponseStatusOk(timekeepings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllTimeKeepingByEmployeeIdAsync(Guid employeeId)
        {
            _logger.LogInformation($"Start get all time keeping with Id: {employeeId}");

            var timekeepings = await _timekeepingServices.GetAllTimeKeepingByEmployeeIdAsync(employeeId);

            _logger.LogInformation($"End get all time keeping... {GetStringFromJson(timekeepings)}");

            return HandleResponseStatusOk(timekeepings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllTimeKeepingByEmployeeIdAndDateAsync(Guid employeeId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation($"Start get all time keeping with Id: {employeeId} from {startDate} to {endDate}");

            var timekeepings = await _timekeepingServices.GetAllTimeKeepingByEmployeeIdAndDateAsync(employeeId, startDate, endDate);

            _logger.LogInformation($"End get all time keeping... {GetStringFromJson(timekeepings)}");

            return HandleResponseStatusOk(timekeepings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> IsTimeKeepingToday(Guid employeeId)
        {
            _logger.LogInformation($"Start check time keeping today...");

            var timekeepings = await _timekeepingServices.IsTimeKeepingTodayAsync(employeeId);

            _logger.LogInformation($"End check time keeping today...");

            return HandleResponseStatusOk(timekeepings);
        }
    }
}
