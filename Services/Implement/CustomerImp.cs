using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Customer;
using ApplicationCore.ViewModels.Customer;
using Azure;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
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
            CheckCustomerInformation(customerVM.Email, customerVM.Phone, customers);
            await CheckUserCreate(customerVM.CreateUserId);

            Customer customer = new Customer();
            MapFCustomerVMTCustomer(customer, customerVM);

            customer.Id = Guid.NewGuid();
            customer.CustomerNumber = await GetNumberCustomer();
            customer.IsDeleted = false;
            customer.ProvinceName = await GetNameLocationById(customer.ProvinceId);
            customer.DistrictName = await GetNameLocationById(customer.DistrictId);
            customer.WardName = await GetNameLocationById(customer.WardId);
            customer.OrderCount = 0;
            customer.CreateDate = GetDateTimeNow();

            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();

            CustomerDto dto = MapFCustomerTCustomerDto(customer);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNumberCustomer()
        {
            int number = _dbContext.Customers.Count() + 1;
            return CustomerConstants.PREFIX_CUSTOMER_NUMBER + number;
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
        public void CheckCustomerInformation(string email, string phone, List<Customer> customers)
        {
            var exist = customers.Where(x => x.Phone == phone).FirstOrDefault();
            if (exist != null)
            {
                throw new BusinessException(EmployeeConstants.EXIST_PHONE);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task DeleteCustomerAsync(Guid customerId)
        {
            await CheckCustomerId(customerId);
            Customer customer = await _dbContext.Customers.FindAsync(customerId);

            customer.IsDeleted = true;
            customer.CustomerNumber += BaseConstants.DELETE;
            customer.Name += BaseConstants.DELETE;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<CustomerDto> UpdateCustomerAsync(CustomerUpdateVM customerVM)
        {
            await CheckCustomerId(customerVM.Id);

            List<Customer> customers = await _dbContext.Customers.AsNoTracking().Where(x => x.Id != customerVM.Id && !x.IsDeleted).ToListAsync();
            CheckCustomerInformation(customerVM.Email, customerVM.Phone, customers);

            Customer customer = await _dbContext.Customers.FindAsync(customerVM.Id);

            MapFCustomerUpdateVMTCustomer(customer, customerVM);

            customer.IsDeleted = false;
            customer.ProvinceName = await GetNameLocationById(customer.ProvinceId);
            customer.DistrictName = await GetNameLocationById(customer.DistrictId);
            customer.WardName = await GetNameLocationById(customer.WardId);

            await _dbContext.SaveChangesAsync();

            CustomerDto dto = MapFCustomerTCustomerDto(customer);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CustomerDto> GetCustomerByIdAsync(Guid customerId)
        {
            await CheckCustomerId(customerId);
            Customer customer = await _dbContext.Customers.FindAsync(customerId);

            CustomerDto dto = MapFCustomerTCustomerDto(customer);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<CustomerPaginationDto> GetCustomerForPagination(int page, int pageSize)
        {
            var customerPagination = new CustomerPaginationDto();

            var customers = _dbContext.Customers.AsNoTracking().Where(x => !x.IsDeleted).OrderByDescending(x => x.CreateDate);
            var customerPerPage = await customers.ToListPagedAsync(page, pageSize, customerPagination);
            customerPagination.customer = DataMapper.MapList<Customer, CustomerDto>(customerPerPage);

            return customerPagination;
        }
    }
}
