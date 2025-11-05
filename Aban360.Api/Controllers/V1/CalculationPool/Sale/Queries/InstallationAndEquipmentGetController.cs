using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.InstallationAndEquipment.Queries
{
    [Route("v1/installation-and-equipment")]
    public class InstallationAndEquipmentGetController : BaseController
    {
        private readonly IInstallationAndEquipmentGetHandler _getHandler;
        public InstallationAndEquipmentGetController(IInstallationAndEquipmentGetHandler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchById inputDto, CancellationToken cancellationToken)
        {
            InstallationAndEquipmentOutputDto result = await _getHandler.Handle(inputDto, cancellationToken);

            return Ok(result);
        }
    }
}
