using ApplicationCore.ViewModels.Employee;
using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeServices _employeeService;
        public EmployeeController(IEmployeeServices employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Create Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateEmployee([FromForm] EmployeeVM employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            var employeeDto = await _employeeService.CreateEmployeeAsync(employee);
            return HandleResponse(employeeDto, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// get all employee
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var employees = await _employeeService.GetAllEmployeeAsync();
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
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
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
            var employee = await _employeeService.UpdateEmployeeAsync(employeeVM);
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
            await _employeeService.DeleteEmployeeByIdAsync(employeeId);
            return HandleResponse(null, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetDataForCreateEmployee()
        {
            var data = await _employeeService.DataForCreateEmployeeAsync();
            return HandleResponse(data, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
        }
    }
}
