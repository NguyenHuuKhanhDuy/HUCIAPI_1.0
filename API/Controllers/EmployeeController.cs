using ApplicationCore.ViewModels.Employee;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

#if !DEBUG
    [Authorize]
#endif

    public class EmployeeController : BaseController
    {
        private readonly IEmployeeServices _employeeService;
        private readonly ILogger _logger;
        public EmployeeController(IEmployeeServices employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Create Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeVM employee)
        {

            _logger.LogInformation($"Start create employee... {employee}");

            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            var employeeDto = await _employeeService.CreateEmployeeAsync(employee);

            _logger.LogInformation("End create employee...");
            return HandleResponse(employeeDto, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// get all employee
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllEmployee()
        {
            _logger.LogInformation("Start get all employee...");

            var employees = await _employeeService.GetAllEmployeeAsync();

            _logger.LogInformation("End get all employee...");

            return HandleResponse(employees, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetEmployeeById(Guid employeeId)
        {
            _logger.LogInformation($"Start get employee by id: {employeeId}");

            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);

            _logger.LogInformation($"End get employee by id: {employeeId}");

            return HandleResponse(employee, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateEmpoyee([FromBody] EmployeeUpdateVM employeeVM)
        {
            _logger.LogInformation($"Start update employee... {employeeVM}");

            var employee = await _employeeService.UpdateEmployeeAsync(employeeVM);

            _logger.LogInformation($"End update employee... {employeeVM}");

            return HandleResponse(employee, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteEmployeeById(Guid employeeId)
        {
            _logger.LogInformation($"Start delete employee by id: {employeeId}");

            await _employeeService.DeleteEmployeeByIdAsync(employeeId);

            _logger.LogInformation($"End delete employee by id: {employeeId}");

            return HandleResponse(null, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetDataForCreateEmployee()
        {
            _logger.LogInformation("Start get data for create employee...");

            var data = await _employeeService.DataForCreateEmployeeAsync();

            _logger.LogInformation("End get data for create employee...");

            return HandleResponse(data, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetSalaryEmployeeByIdAsync(Guid employeeId, DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation($"Start get salary for employee by id: {employeeId}");

            var data = await _employeeService.SalaryEmployeeByIdAsync(employeeId, startDate, endDate);

            _logger.LogInformation($"End get salary for employee by id: {employeeId}");

            return HandleResponse(data, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCalculateBenefitAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation($"Start calculate benefit from {startDate} to {endDate}");

            var benefit = await _employeeService.CalculateBenefitWithDateAsync(startDate, endDate);

            _logger.LogInformation($"Start calculate benefit from {startDate} to {endDate}. {GetStringFromJson(benefit)}");

            return HandleResponseStatusOk(benefit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetReportMonthsAgo(int monthsAgo)
        {
            _logger.LogInformation($"Start get report for {monthsAgo} months ago...");

            var report = await _employeeService.ReportMonthsAgo(monthsAgo);

            _logger.LogInformation($"Start get report for {{monthsAgo}} months ago... {GetStringFromJson(report)}");

            return HandleResponseStatusOk(report);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            _logger.LogInformation("Start get all role...");

            var roles = await _employeeService.GetAllRolesAsync();

            _logger.LogInformation($"End get all role...{GetStringFromJson(roles)}");

            return HandleResponseStatusOk(roles);
        }
    }
}
