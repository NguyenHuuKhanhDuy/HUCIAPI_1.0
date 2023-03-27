using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Supplier;
using ApplicationCore.ViewModels.Supplier;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class SupplierImp : BaseServices, ISupplierServices
    {
        private readonly HucidbContext _dbContext;
        public SupplierImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerVM"></param>
        /// <returns></returns>
        public async Task<SupplierDto> CreateCustomerAsync(SupplierVM SupplierVM)
        {
            List<Supplier> suppliers = await _dbContext.Suppliers.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();
            CheckSupplierInformation(SupplierVM.Email, SupplierVM.Phone, suppliers);
            await CheckUserCreate(SupplierVM.CreateUserId);

            Supplier supplier = new Supplier();
            MapFSupplierVMTSupplier(supplier, SupplierVM);

            supplier.Id = Guid.NewGuid();
            supplier.SupplierNumber = await GetNumberSupplier();
            supplier.IsDeleted = false;
            supplier.ProvinceName = await GetNameLocationById(supplier.ProvinceId);
            supplier.DistrictName = await GetNameLocationById(supplier.DistrictId);
            supplier.WardName = await GetNameLocationById(supplier.WardId);
            supplier.CreateDate = GetDateTimeNow();

            await _dbContext.Suppliers.AddAsync(supplier);
            await _dbContext.SaveChangesAsync();

            SupplierDto dto = MapFSupplierTSupplierDto(supplier);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeVM"></param>
        /// <param name="employees"></param>
        /// <exception cref="BusinessException"></exception>
        public void CheckSupplierInformation(string email, string phone, List<Supplier> suppliers)
        {
            var exist = suppliers.Where(x => x.Email == email).FirstOrDefault();
            if (exist != null)
            {
                throw new BusinessException(EmployeeConstants.EXIST_EMAIL);
            }

            exist = suppliers.Where(x => x.Phone == phone).FirstOrDefault();
            if (exist != null)
            {
                throw new BusinessException(EmployeeConstants.EXIST_PHONE);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNumberSupplier()
        {
            int number = await _dbContext.Suppliers.CountAsync() + 1;
            return SupplierConstants.PREFIX_SUPPLIER_NUMBER + number;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task CheckSupplierId(Guid customerId)
        {
            var exist = await _dbContext.Suppliers.FirstOrDefaultAsync(x => x.Id == customerId && !x.IsDeleted);
            if (exist == null)
            {
                throw new BusinessException(SupplierConstants.SUPPLIER_NOT_EXIST);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task DeleteSupplierAsync(Guid supplierId)
        {
            await CheckSupplierId(supplierId);
            Supplier supplier = await _dbContext.Suppliers.FindAsync(supplierId);

            supplier.IsDeleted = true;
            supplier.SupplierNumber += BaseConstants.DELETE;
            supplier.Name += BaseConstants.DELETE;

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerVM"></param>
        /// <returns></returns>
        public async Task<SupplierDto> UpdateCustomerAsync(SupplierUpdateVM supplierVM)
        {
            await CheckSupplierId(supplierVM.Id);

            List<Supplier> suppliers = await _dbContext.Suppliers.AsNoTracking().Where(x => x.Id != supplierVM.Id && !x.IsDeleted).ToListAsync();
            CheckSupplierInformation(supplierVM.Email, supplierVM.Phone, suppliers);

            Supplier supplier = await _dbContext.Suppliers.FindAsync(supplierVM.Id);

            MapFSupplierUpdateVMTSupplier(supplier, supplierVM);

            supplier.IsDeleted = false;
            supplier.ProvinceName = await GetNameLocationById(supplier.ProvinceId);
            supplier.DistrictName = await GetNameLocationById(supplier.DistrictId);
            supplier.WardName = await GetNameLocationById(supplier.WardId);

            await _dbContext.SaveChangesAsync();

            SupplierDto dto = MapFSupplierTSupplierDto(supplier);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<SupplierDto> GetSupplierByIdAsync(Guid supplierId)
        {
            await CheckSupplierId(supplierId);
            Supplier supplier = await _dbContext.Suppliers.FindAsync(supplierId);

            SupplierDto dto = MapFSupplierTSupplierDto(supplier);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<SupplierDto>> GetAllSupplierrAsync()
        {
            var suppliers = await _dbContext.Suppliers.Where(x => !x.IsDeleted).ToListAsync();

            List<SupplierDto> dtos = new List<SupplierDto>();
            foreach (Supplier supplier in suppliers)
            {
                dtos.Add(MapFSupplierTSupplierDto(supplier));
            }
            return dtos;
        }
    }
}
