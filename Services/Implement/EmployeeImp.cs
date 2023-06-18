using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto;
using ApplicationCore.ModelsDto.Employee;
using ApplicationCore.ViewModels.Employee;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Services.Implement
{
    public class EmployeeImp : BaseServices, IEmployeeServices
    {
        private readonly HucidbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IOrderServices _orderServices;

        public EmployeeImp(HucidbContext dbContext, IMapper mapper, IConfiguration config, IOrderServices orderServices) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _config = config;
            _orderServices = orderServices;
        }

        public async Task<EmployeeDto> Login(UserVM userVM, string ip)
        {
            if (userVM.Username.ToLower() != EmployeeConstants.AdminName.ToLower())
            {
                await CheckIpAsync(ip);
            }

            var employee = _dbContext.Employees.Where(p => p.Username.ToLower() == userVM.Username.ToLower()).FirstOrDefault();

            if (employee == null || employee.IsDeleted)
            {
                throw new BusinessException(LoginConstants.USER_NOT_EXIST);
            }

            if (!CheckPassword(employee.Password, userVM.Password))
            {
                throw new BusinessException(LoginConstants.PASSWORD_INCORRECT);
            }

            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);

            var employeeCreate = await _dbContext.Employees.FindAsync(employeeDto.CreateUserId);
            if (employeeCreate != null)
            {
                employeeDto.CreateUserName = employeeCreate.Name;
            }

            employeeDto.Token = CreateToken(employeeDto);

            return employeeDto;
        }

        private async Task CheckIpAsync(string ipAddress)
        {
            var ip = await _dbContext.Ips.AsNoTracking().FirstOrDefaultAsync(x => x.Ipv4 == ipAddress && !x.IsDeleted);

            if(ip == null)
            {
                throw new BusinessException($"Your IP address: {ipAddress} is unable to log in to this page.");
            }
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
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token).ToString();
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeVM employeeVM)
        {
            List<Employee> employees = await _dbContext.Employees.Where(x => !x.IsDeleted).ToListAsync();
            CheckEmployeeInformation(employeeVM, employees);

            var salaryType = await _dbContext.SalaryTypes.FindAsync(employeeVM.SalaryTypeId);
            if (salaryType == null)
            {
                throw new BusinessException(EmployeeConstants.SALARY_TYPE_NOTE_EXIST);
            }

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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetRuleNameById(int id)
        {
            var rule = await _dbContext.Rules.FindAsync(id);

            if (rule == null)
            {
                throw new BusinessException(EmployeeConstants.RULE_NOTE_EXIST);
            }

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
            if (exist != null)
            {
                throw new BusinessException(EmployeeConstants.EXIST_EMAIL);
            }

            exist = employees.Where(x => x.Phone == employeeVM.Phone).FirstOrDefault();
            if (exist != null)
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
            var allEmployee = await _dbContext.Employees.ToListAsync();
            var employees = allEmployee.Where(x => !x.IsDeleted).ToList();
            List<EmployeeDto> employeeDtos = new List<EmployeeDto>();
            foreach (var employee in employees)
            {
                if (employee.IsDeleted == false)
                {
                    var employeeDto = _mapper.Map<EmployeeDto>(employee);
                    employeeDto.CreateUserName = allEmployee.Where(x => x.Id == employeeDto.CreateUserId).FirstOrDefault().Name;
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
            if (employee == null || employee.IsDeleted)
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
            if (employee == null || employee.IsDeleted)
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

            if (employee.Password != employeeVM.Password)
            {
                employee.Password = HashPassword(employeeVM.Password);
            }

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

            if (employee == null || employee.IsDeleted)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            if (employee.Name == EmployeeConstants.AdminName || employee.Name == BaseConstants.NameDefault)
            {
                throw new BusinessException(EmployeeConstants.CanNotRemoveAdmin);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEmployee"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<SalaryEmployeeDto> SalaryEmployeeByIdAsync(Guid idEmployee, DateTime startDate, DateTime endDate)
        {
            var employee = await _dbContext.Employees.FindAsync(idEmployee);
            if (employee == null || employee.IsDeleted)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            var orderCommission = await _dbContext.OrderCommissions.Where(x => x.CreateDate.Date >= startDate.Date && x.CreateDate.Date <= endDate.Date && x.EmployeeId == idEmployee).ToListAsync();

            var salaryEmployeeDto = new SalaryEmployeeDto();
            salaryEmployeeDto.SalaryEmployee = employee.Salary;

            foreach (var item in orderCommission)
            {
                salaryEmployeeDto.OrderCommissions.Add(MapFOrderCommissionTOrderCommissionDto(item));
            }

            salaryEmployeeDto.TotalCommission = orderCommission.Sum(x => x.OrderCommission1);

            return salaryEmployeeDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string GetPassworkEncrypt(string password)
        {
            return HashPassword(password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<BenefitDto> CalculateBenefitWithDateAsync(DateTime startDate, DateTime endDate)
        {
            var commissions = await _dbContext.OrderCommissions.AsNoTracking().Where(x => x.CreateDate.Date >= startDate.Date && x.CreateDate.Date <= endDate.Date).ToListAsync();

            var orderIds = commissions.Select(x => x.OrderId);
            var orders = await _dbContext.Orders.AsNoTracking()
                .Where(x => orderIds.Contains(x.Id))
                .ToListAsync();
            var employee = await _dbContext.Employees.Where(x => !x.IsDeleted).ToListAsync();
            var otherCosts = await _dbContext.OtherCosts.AsNoTracking()
                .Where(x => x.CreateDate.Date >= startDate.Date
                && x.CreateDate.Date <= endDate.Date
                && !x.IsDeleted)
                .ToListAsync();

            var benefitDto = new BenefitDto();
            benefitDto.TotalSalary = employee.Sum(x => x.Salary);
            benefitDto.TotalCommission = commissions.Sum(x => x.OrderCommission1);
            benefitDto.TotalOtherCost = otherCosts.Sum(x => x.Price);
            benefitDto.TotalOrderBenefit = orders.Sum(x => x.BenefitOrder);

            return benefitDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monthsAgo"></param>
        /// <returns></returns>
        public async Task<List<ReportMonthDto>> ReportMonthsAgo(int monthsAgo)
        {
            var reportDto = new List<ReportMonthDto>();
            var monthsAgoDateTime = DateTime.Today.AddMonths(-monthsAgo);
            for(int i = 0; i <monthsAgo;i++)
            {
                var firstDayOfMonth = new DateTime(monthsAgoDateTime.Year, monthsAgoDateTime.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var totalOrder = await _orderServices.GetOrderByDateAsync(firstDayOfMonth, lastDayOfMonth);
                var benefit = await CalculateBenefitWithDateAsync(firstDayOfMonth, lastDayOfMonth);
                //do something
                var report = new ReportMonthDto
                {
                    TotalOrder = totalOrder.Count,
                    Benefit = benefit,
                    MonthAgo = monthsAgo - i
                };

                reportDto.Add(report);
                monthsAgoDateTime = monthsAgoDateTime.AddMonths(1);
            }

            return reportDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var rolesDto = new List<RoleDto>();
            var roles = await _dbContext.Rules.AsNoTracking().ToListAsync();

            foreach (var item in roles)
            {
                rolesDto.Add(new RoleDto
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            return rolesDto;
        }
    }
}
