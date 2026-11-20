using Aban360.Api.Filters;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Application.Features.Sale.Services;
using Aban360.Api.Controllers.V1.CalculationPool.Sale;
using IInstallationAndEquipmentService = global::Aban360.CalculationPool.Application.Features.Sale.Services.IInstallationAndEquipmentService;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale
{
    [Route("v1/installation-and-equipment")]
    public class InstallationAndEquipmentController : BaseController
    {
        private readonly IInstallationAndEquipmentService _installationAndEquipmentService;
        public InstallationAndEquipmentController(IInstallationAndEquipmentService installationAndEquipmentService)
        {
            _installationAndEquipmentService = installationAndEquipmentService;
            _installationAndEquipmentService.NotNull(nameof(installationAndEquipmentService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InstallationAndEquipmentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] InstallationAndEquipmentCreateDto createDto, CancellationToken cancellationToken)
        {
            await _installationAndEquipmentService.InstallationAndEquipmentCreateAsync(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SearchById input, CancellationToken cancellationToken)
        {
            await _installationAndEquipmentService.InstallationAndEquipmentDeleteAsync(input, CurrentUser, cancellationToken);
            return Ok(input);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InstallationAndEquipmentUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] InstallationAndEquipmentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _installationAndEquipmentService.InstallationAndEquipmentUpdateAsync(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<InstallationAndEquipmentOutputDto> result = await _installationAndEquipmentService.InstallationAndEquipmentGetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchById inputDto, CancellationToken cancellationToken)
        {
            InstallationAndEquipmentOutputDto result = await _installationAndEquipmentService.InstallationAndEquipmentGetAsync(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
