using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.ViewModels.Import;
using Services.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#if !DEBUG    
    [Authorize]
#endif
    public class ImportController : BaseController
    {
        private readonly IImportServices _services;
        private readonly ILogger _logger;
        public ImportController(IImportServices services, ILogger<ImportController> logger)
        {
            _services = services;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateImportAsync(ImportVM vm)
        {
            _logger.LogInformation($"Start create import... {GetStringFromJson(vm)}");

            var import = await _services.CreateImportAsync(vm);

            _logger.LogInformation($"End create import... {GetStringFromJson(import)}");

            return HandleResponseStatusOk(import);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateImportAsync(ImportUpdateVM vm)
        {
            _logger.LogInformation($"Start update import... {GetStringFromJson(vm)}");

            var import = await _services.UpdateImportAsync(vm);

            _logger.LogInformation($"End update import... {GetStringFromJson(import)}");

            return HandleResponseStatusOk(import);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteImportAsync(Guid Id)
        {
            _logger.LogInformation($"Start delete import... {Id}");

            await _services.DeleteImportAsync(Id);

            _logger.LogInformation($"End delete import... {Id}");

            return HandleResponseStatusOk();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetImportByIdAsync(Guid Id)
        {
            _logger.LogInformation($"Start get import... {Id}");

            var import = await _services.GetImportByIdAsync(Id);

            _logger.LogInformation($"End get import... {import}");

            return HandleResponseStatusOk(import);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllImportAsync()
        {
            _logger.LogInformation($"Start get all import...");

            var imports = await _services.GetAllImportAsync();

            _logger.LogInformation($"End get import... {imports}");

            return HandleResponseStatusOk(imports);
        }
    }
}
