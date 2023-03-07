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
            try
            {
                if(employee == null)
                {
                    throw new ArgumentNullException(nameof(employee));
                }

                var employeeDto = await _employeeService.CreateEmployeeAsync(employee);
                return HandleResponse(employeeDto, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// get all employee
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllEmployee()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeeAsync();
                return HandleResponse(employees, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEmployeeById(Guid employeeId)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
                return HandleResponse(employee, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateEmpoyee([FromBody] EmployeeUpdateVM employeeVM)
        {
            try
            {
                var employee = await _employeeService.UpdateEmployeeAsync(employeeVM);
                return HandleResponse(employee, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteEmployeeById(Guid employeeId)
        {
            try
            {
                await _employeeService.DeleteEmployeeByIdAsync(employeeId);
                return HandleResponse(null, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDataForCreateEmployee()
        {
            try
            {
                var data = await _employeeService.DataForCreateEmployeeAsync();
                return HandleResponse(data, StatusCodeConstants.MESSAGE_SUCCESS, StatusCodeConstants.STATUS_SUCCESS);
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
