using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto;
using ApplicationCore.ViewModels;
using AutoMapper;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Implement
{
    public class EmployeeImp : IEmployeeServices
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

            if(employee == null)
            {
                throw new BusinessException(LoginConstants.USER_NOT_EXIST);
            }

            if(!CheckPassword(employee.Password, userVM.Password))
            {
                throw new BusinessException(LoginConstants.PASSWORD_INCORRECT);
            }

            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(employee);

            var employeeCreate = await _dbContext.Employees.FindAsync(employeeDto.CreateUserId);
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
    }
}
