using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Customer;
using ApplicationCore.ViewModels.Customer;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class CustomerImp : BaseServices, ICustomerServices
    {
        private readonly HucidbContext _dbContext;

        public CustomerImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerVM customerVM)
        {
            List<Customer> customers = await _dbContext.Customers.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();
            CheckCustomerInformation(customerVM, customers);
            await CheckUserCreate(customerVM.CreateUserId);

            Customer customer = new Customer();
            MapFCustomerVMTCustomer(customer, customerVM);

            customer.Id = Guid.NewGuid();
            customer.IsDeleted = false;
            customer.ProvinceName = await GetNameLocationById(customer.ProvinceId);
            customer.DistrictName = await GetNameLocationById(customer.DistrictId);
            customer.WardName = await GetNameLocationById(customer.WardId);
            customer.Address = $"{customer.Address}, {customer.WardName}, {customer.DistrictName}, {customer.ProvinceName}";
            customer.OrderCount = 0;
            customer.CreateDate = GetDateTimeNow();

            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();

            CustomerDto dto = MapFCustomerTCustomerDto(customer);
            return dto;
        }

        public async Task CheckCustomerId(Guid customerId)
        {
            var exist = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId && !x.IsDeleted);
            if (exist == null)
            {
                throw new BusinessException(CustomerConstants.CUSTOMER_NOT_EXIST);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeVM"></param>
        /// <param name="employees"></param>
        /// <exception cref="BusinessException"></exception>
        public void CheckCustomerInformation(CustomerVM customerVM, List<Customer> customers)
        {
            var exist = customers.Where(x => x.Email == customerVM.Email).FirstOrDefault();
            if (exist != null)
            {
                throw new BusinessException(EmployeeConstants.EXIST_EMAIL);
            }

            exist = customers.Where(x => x.Phone == customerVM.Phone).FirstOrDefault();
            if (exist != null)
            {
                throw new BusinessException(EmployeeConstants.EXIST_PHONE);
            }
        }

        public Task DeleteCustomerAsync(Guid customerId)
        {
            throw new NotImplementedException();
        }

        //public async Task<CustomerDto> UpdateCustomerAsync(CustomerUpdateVM customerVM)
        //{
        //    await CheckCustomerId(customerVM.Id);
        //    Customer customer = await _dbContext.Customers.FindAsync(customerVM.Id);

        //    Map
        //}
    }
}
