using ApplicationCore.ViewModels.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SupplierController : BaseController
    {
        private readonly ISupplierServices _supplierServices;
        private ILogger _logger;
        public SupplierController(ISupplierServices services, ILogger<SupplierController> logger)
        {
            _supplierServices = services;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateSupplierAsync(SupplierVM supplierVM)
        {
            _logger.LogInformation("Start create supplier...");

            var supplier = await _supplierServices.CreateCustomerAsync(supplierVM);

            _logger.LogInformation("End create supplier...");

            return HandleResponseStatusOk(supplier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateSupplierAsync(SupplierUpdateVM supplierVM)
        {
            _logger.LogInformation("Start update supplier...");

            var supplier = await _supplierServices.UpdateCustomerAsync(supplierVM);

            _logger.LogInformation("End update supplier...");

            return HandleResponseStatusOk(supplier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteSupplierAsync(Guid supplierId)
        {
            _logger.LogInformation($"Start delete supplier id: {supplierId}");

            await _supplierServices.DeleteSupplierAsync(supplierId);

            _logger.LogInformation($"End delete supplier id: {supplierId}");

            return HandleResponseStatusOk(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllSupplierAsync()
        {
            _logger.LogInformation("Start get all supplier...");

            var suppliers = await _supplierServices.GetAllSupplierrAsync();

            _logger.LogInformation("End get all supplier...");

            return HandleResponseStatusOk(suppliers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetSupplierByIdAsync(Guid supplierId)
        {
            _logger.LogInformation($"Start get supplier by id: {supplierId}");

            var supplier = await _supplierServices.GetSupplierByIdAsync(supplierId);

            _logger.LogInformation($"End get supplier by id: {supplierId}");

            return HandleResponseStatusOk(supplier);
        }
    }
}
