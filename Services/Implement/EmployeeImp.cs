using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto;
using ApplicationCore.ModelsDto.Employee;
using ApplicationCore.ViewModels.Employee;
using AutoMapper;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Implement
{
    public class EmployeeImp : BaseServices, IEmployeeServices
    {
        private readonly HucidbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public EmployeeImp(HucidbContext dbContext, IMapper mapper, IConfiguration config)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _config = config;
        }
        public async Task<EmployeeDto> Login(UserVM userVM)
        {
            var employee =  _dbContext.Employees.Where(p => p.Username == userVM.Username).FirstOrDefault();

            if(employee == null || employee.IsDeleted)
            {
                throw new BusinessException(LoginConstants.USER_NOT_EXIST);
            }

            if (!CheckPassword(employee.Password, userVM.Password))
            {
                throw new BusinessException(LoginConstants.PASSWORD_INCORRECT);
            }

            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);

            var employeeCreate =  await _dbContext.Employees.FindAsync(employeeDto.CreateUserId);
            if(employeeCreate != null)
            {
                employeeDto.CreateUserName = employeeCreate.Name;
            }

            employeeDto.Token = CreateToken(employeeDto);

            return employeeDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passwordHash"></param>
        /// <param name="passwordInput"></param>
        /// <returns></returns>
        private bool CheckPassword(string passwordHash, string passwordInput)
        {
            return BCrypt.Net.BCrypt.Verify(passwordInput, passwordHash);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        private string CreateToken(EmployeeDto employee)
        {
            string subject = _config.GetSection("Jwt")["Subject"].ToString();
            string issuer = _config.GetSection("Jwt")["Issuer"].ToString();
            string audience = _config.GetSection("Jwt")["Audience"].ToString();
            string keyApp = _config.GetSection("Jwt")["Key"].ToString();

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", employee.Id.ToString()),
                        new Claim("DisplayName", employee.Name),
                        new Claim("Email", employee.Email)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyApp));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token).ToString();
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeVM employeeVM)
        {
            List<Employee> employees = await _dbContext.Employees.Where(x => !x.IsDeleted).ToListAsync();
            CheckEmployeeInformation(employeeVM, employees);

            Employee employee = _mapper.Map<Employee>(employeeVM);
            employee.Id = Guid.NewGuid();
            await GetNameFieldHaveId(employee);
            employee.IsDeleted = false;
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            EmployeeDto dto = _mapper.Map<EmployeeDto>(employee);
            return dto;
        }

        public async Task GetNameFieldHaveId(Employee employee)
        {
            employee.ProvinceName = await GetNameLocationById(employee.ProvinceId);
            employee.DistrictName = await GetNameLocationById(employee.DistrictId);
            employee.WardName = await GetNameLocationById(employee.WardId);
            employee.RuleName = await GetRuleNameById(employee.RuleId);
            employee.Address = $"{employee.Address}, {employee.WardName}, {employee.DistrictName}, {employee.ProvinceName}";
            employee.Password = HashPassword(employee.Password);
        }

        /// <summary>
        /// Get Location By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>name of location</returns>
        public async Task<string> GetNameLocationById(int id)
        {
            var location = await _dbContext.Locations.FindAsync(id);
            return location.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetRuleNameById(int id)
        {
            var rule = await _dbContext.Rules.FindAsync(id);
            return rule.Name;
        }

        /// <summary>
        /// Check Infor of employee
        /// </summary>
        /// <param name="employeeVM"></param>
        /// <exception cref="BusinessException"></exception>
        public void CheckEmployeeInformation(EmployeeVM employeeVM, List<Employee> employees)
        {
            var exist = employees.Where(x => x.Email == employeeVM.Email).FirstOrDefault();
            if(exist != null)
            {
                throw new BusinessException(EmployeeConstants.EXIST_EMAIL);
            }

            exist = employees.Where(x => x.Phone == employeeVM.Phone).FirstOrDefault();
            if(exist != null)
            {
                throw new BusinessException(EmployeeConstants.EXIST_PHONE);
            }

            exist = employees.Where(x => x.Username == employeeVM.Username).FirstOrDefault();
            if (exist != null)
            {
                throw new BusinessException(EmployeeConstants.EXIST_USERNAME);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<EmployeeDto>> GetAllEmployeeAsync()
        {
            var employees =  await _dbContext.Employees.ToListAsync();
            List<EmployeeDto> employeeDtos = new List<EmployeeDto>();
            foreach (var employee in employees)
            {
                if(employee.IsDeleted == false)
                {
                    var employeeDto = _mapper.Map<EmployeeDto>(employee);
                    employeeDto.CreateUserName = employees.Where(x => x.Id == employeeDto.CreateUserId).FirstOrDefault().Name;
                    employeeDtos.Add(employeeDto);
                }
            }

            return employeeDtos;
        }

        /// <summary>
        /// Get Employee By Id
        /// </summary>
        /// <param name="idEmployee"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId)
        {
            var employee = await _dbContext.Employees.FindAsync(employeeId);
            if(employee == null || employee.IsDeleted)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            employeeDto.CreateUserName = _dbContext.Employees.FirstOrDefault(x => x.Id == employeeDto.CreateUserId).Name;
            return employeeDto;
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(EmployeeUpdateVM employeeVM)
        {
            var employee = await _dbContext.Employees.FindAsync(employeeVM.Id);
            if(employee == null || employee.IsDeleted)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            List<Employee> employeesNotCurrentEmployee = await _dbContext.Employees.Where(x => !x.IsDeleted && x.Id != employeeVM.Id).ToListAsync();
            CheckEmployeeInformation(_mapper.Map<EmployeeVM>(employeeVM), employeesNotCurrentEmployee);

            employee.Name = employeeVM.Name;
            employee.Email = employeeVM.Email;
            employee.Phone = employeeVM.Phone;
            employee.Birthday = employeeVM.Birthday;
            employee.Gender = employeeVM.Gender.Value;
            employee.ProvinceId = employeeVM.ProvinceId.Value;
            employee.DistrictId = employeeVM.DistrictId.Value;
            employee.WardId = employeeVM.WardId.Value;
            employee.Notes = employeeVM.Notes;
            employee.Salary = employeeVM.Salary.Value;
            employee.SalaryTypeId = employeeVM.SalaryTypeId.Value;
            employee.RuleId = employeeVM.RuleId.Value;
            employee.Address = employeeVM.Address;
            employee.Username = employeeVM.Username;
            employee.Password = HashPassword(employeeVM.Password);

            await GetNameFieldHaveId(employee);

            await _dbContext.SaveChangesAsync();
            
            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);
            return employeeDto;
        }

        /// <summary>
        /// Delete Employee By Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task DeleteEmployeeByIdAsync(Guid employeeId)
        {
            var employee = await _dbContext.Employees.FindAsync(employeeId);
            if(employee == null || employee.IsDeleted)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            employee.IsDeleted = true;
            employee.Name = employee.Name + BaseConstants.DELETE;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<DataForCreateEmployeeDto> DataForCreateEmployeeAsync()
        {
            DataForCreateEmployeeDto data = new DataForCreateEmployeeDto();

            data.SalaryTypes = await _dbContext.SalaryTypes.Where(x => x.Id != 0).ToListAsync();
            data.Rules = await _dbContext.Rules.Where(x => x.Id != 0).ToListAsync();

            return data;
        }
    }
}
