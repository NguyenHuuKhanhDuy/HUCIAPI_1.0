using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.Import;
using ApplicationCore.ViewModels.Import;
using ApplicationCore.ViewModels.Product;
using Common.Constants;
using Common.Enums;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class ImportImp : BaseServices, IImportServices
    {
        private readonly HucidbContext _dbContext;

        public ImportImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<ImportDto> CreateImportAsync(ImportVM vm)
        {
            var productIds = vm.Products.Select(x => x.ProductId).ToList();
            var employees = await _dbContext.Employees.AsNoTracking().Where(x => x.Id == vm.UserCreateId && !x.IsDeleted).FirstOrDefaultAsync();
            var suppliers = await _dbContext.Suppliers.AsNoTracking().Where(x => x.Id == vm.SupplierId && !x.IsDeleted).FirstOrDefaultAsync();
            var products = await _dbContext.Products.Where(x => productIds.Contains(x.Id) && !x.IsDeleted).ToListAsync();
            var statusImports = await _dbContext.StatusImports.AsNoTracking().ToListAsync();
            CheckInfoImport(vm, employees, suppliers, products);
            var imports = await _dbContext.Imports.AsNoTracking().ToListAsync();
            var import = MapFImportVMTImport(vm);
            import.ImportNumber = GetImportNumber(imports);
            import.StatusImportName = statusImports.FirstOrDefault(x => x.Id == import.StatusImportId)!.Name;

            var importDetails = await GetImportDetailAsync(import, products, vm.Products);

            await _dbContext.Imports.AddAsync(import);
            await _dbContext.ImportDetails.AddRangeAsync(importDetails);
            await _dbContext.SaveChangesAsync();

            import.Total = await CalculateTotalImport(import.Id);
            await _dbContext.SaveChangesAsync();
            var importDto = MapFImportTImportDto(import);

            return importDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="employees"></param>
        /// <param name="suppliers"></param>
        /// <param name="products"></param>
        /// <exception cref="BusinessException"></exception>
        private void CheckInfoImport(ImportVM vm, Employee? employee, Supplier? supplier, List<Product> products)
        {
            if (employee == null)
            {
                throw new BusinessException(EmployeeConstants.EMPLOYEE_NOT_EXIST);
            }

            if (supplier == null)
            {
                throw new BusinessException(SupplierConstants.SUPPLIER_NOT_EXIST);
            }

            foreach (var item in vm?.Products)
            {
                var product = products.FirstOrDefault(x => x.Id == item.ProductId);

                if (product == null)
                {
                    throw new BusinessException($"Product with Id: {item.ProductId} not exist");
                }

                if (product.ProductTypeId == ProductConstants.PRODUCT_TYPE_COMBO)
                {
                    throw new BusinessException(ImportConstants.CanNotImportCombo);
                }
            }
        }

        private string GetImportNumber(List<Import> suppliers)
        {
            return ImportConstants.PrefixImportNumber + $"{suppliers.Count + 1}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="import"></param>
        /// <param name="products"></param>
        /// <param name="productInsides"></param>
        /// <param name="comboDetails"></param>
        /// <returns></returns>
        private async Task<List<ImportDetail>> GetImportDetailAsync(Import import, List<Product> products, List<ProductInsideComboVM> productInsides)
        {
            var importDetails = new List<ImportDetail>();
            var idImport = import.Id;
            var importDetailsFromDB = await _dbContext.ImportDetails.Where(x => x.ImportId == idImport).ToListAsync();
            var idProductsInside = productInsides.Select(x => x.ProductId).ToList();
            var importDetailsNeedToDelete = importDetailsFromDB.Where(x => !idProductsInside.Contains(x.ProductId)).ToList();
            bool isUpdateProduct = import.StatusImportId == 0 ? false : true;

            foreach (var item in productInsides)
            {
                var product = products.Where(x => x.Id == item.ProductId).FirstOrDefault();
                var importDetailFromDB = importDetailsFromDB.Where(x => x.ProductId == item.ProductId).FirstOrDefault();

                if (importDetailFromDB == null)
                {
                    var importDetail = new ImportDetail
                    {
                        Id = Guid.NewGuid(),
                        ImportId = idImport,
                        ProductId = item.ProductId,
                        ProductName = product!.Name,
                        ProductNumber = product!.ProductNumber,
                        ProductPriceImport = product!.OriginalPrice,
                        Quantity = item.Quantity,
                        IsDeleted = BaseConstants.IsDeletedDefault
                    };
                    importDetails.Add(importDetail);
                }
                else
                {
                    importDetailFromDB.IsDeleted = BaseConstants.IsDeletedDefault;
                    importDetailFromDB.Quantity = item.Quantity;
                }

                if (isUpdateProduct)
                {
                    product.OnHand += item.Quantity;
                }

            }

            importDetailsNeedToDelete.ForEach(x =>
            {
                x.IsDeleted = true;
            });

            return importDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<ImportDto> UpdateImportAsync(ImportUpdateVM vm)
        {
            var import = await _dbContext.Imports.Where(x => x.Id == vm.Id && !x.IsDelete).FirstOrDefaultAsync();
            if (import == null)
            {
                throw new BusinessException($"{ImportConstants.ImportNotExistWithId}: {vm.Id} ");
            }

            if (import.StatusImportId == (int)ImportEnum.Done)
            {
                throw new BusinessException(ImportConstants.ImportIsCompleted);
            }

            var productIds = vm.Products.Select(x => x.ProductId).ToList();
            var employee = await _dbContext.Employees.AsNoTracking().Where(x => x.Id == vm.UserCreateId && !x.IsDeleted).FirstOrDefaultAsync();
            var supplier = await _dbContext.Suppliers.AsNoTracking().Where(x => x.Id == vm.SupplierId && !x.IsDeleted).FirstOrDefaultAsync();
            var products = await _dbContext.Products.Where(x => productIds.Contains(x.Id) && !x.IsDeleted).ToListAsync();
            var statusImports = await _dbContext.StatusImports.AsNoTracking().ToListAsync();

            CheckInfoImport(vm, employee, supplier, products);
            MapFImportUpdateVMTImport(import, vm);
            import.StatusImportName = statusImports.FirstOrDefault(x => x.Id == import.StatusImportId)!.Name;

            var importDetails = await GetImportDetailAsync(import, products, vm.Products);

            await _dbContext.ImportDetails.AddRangeAsync(importDetails);
            await _dbContext.SaveChangesAsync();

            import.Total = await CalculateTotalImport(import.Id);
            await _dbContext.SaveChangesAsync();
            var importDto = MapFImportTImportDto(import);

            return importDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private async Task<int> CalculateTotalImport(Guid Id)
        {
            int total = 0;
            var importDetails = await _dbContext.ImportDetails.Where(x => x.Id != Id && !x.IsDeleted).ToListAsync();
            if(importDetails != null && importDetails.Any())
            {
                foreach (var item in importDetails)
                {
                    total += item.Quantity * item.ProductPriceImport;
                }
            }

            return total;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteImportAsync(Guid Id)
        {
            var import = await _dbContext.Imports.Where(x => x.Id == Id && !x.IsDelete).FirstOrDefaultAsync();
            
            if (import == null)
            {
                throw new BusinessException($"{ImportConstants.ImportNotExistWithId}: {Id} ");
            }

            import.IsDelete = true;

            bool isImported = import.StatusImportId == 0 ? false : true;
            var products = await _dbContext.Products.Where(x => !x.IsDeleted).ToListAsync();
            var importDetails = await _dbContext.ImportDetails.Where(x => x.ImportId == Id).ToListAsync();

            importDetails.ForEach(x =>
            {
                x.IsDeleted = true;
            });

            if (isImported)
            {
                foreach (var item in importDetails)
                {
                    var product = products.Where(x => x.Id == item.ProductId).FirstOrDefault();

                    if(product != null)
                    {
                        product.OnHand -= item.Quantity;
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<ImportDto> GetImportByIdAsync(Guid Id)
        {
            var import = await _dbContext.Imports.Where(x => x.Id == Id && !x.IsDelete).FirstOrDefaultAsync();

            if (import == null)
            {
                throw new BusinessException($"{ImportConstants.ImportNotExistWithId}: {Id}");
            }

            var importDto = MapFImportTImportDto(import);
            var importDetails = await _dbContext.ImportDetails.Where(x => x.ImportId == import.Id).ToListAsync();

            if( importDetails.Any() )
            {
                importDetails.ForEach(x =>
                {
                    importDto.Details.Add(MapFImportDetailTDto(x));
                });
            }

            return importDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ImportDto>> GetAllImportAsync()
        {
            var imports = await _dbContext.Imports.Where(x => !x.IsDelete).OrderByDescending(x => x.CreateDate).ToListAsync();
            var employeeIds = imports.Select(x => x.UserCreateId).ToList();
            var shupplierIds = imports.Select(x => x.SupplierId).ToList();
            var employees = await _dbContext.Employees.AsNoTracking().Where(x => employeeIds.Contains(x.Id)).ToListAsync();
            var suppliers = await _dbContext.Suppliers.AsNoTracking().Where(x => shupplierIds.Contains(x.Id)).ToListAsync();
            var importDtos = new List<ImportDto>();

            if(imports.Any())
            {
                imports.ForEach(import =>
                {
                    var supplier = suppliers.FirstOrDefault(x => x.Id == import.SupplierId);
                    var employee = employees.FirstOrDefault(x => x.Id == import.UserCreateId);

                    importDtos.Add(MapFImportTImportDto(import, employee?.Name, supplier?.Name));
                });
            }

            return importDtos;
        }
    }
}
