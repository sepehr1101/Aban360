using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Command
{
    [Route("v1/installation-and-equipment")]
    public class InstallationAndEquipmentUpdateController : BaseController
    {
        private readonly IInstallationAndEquipmentUpdateHadler _updateHadler;
        public InstallationAndEquipmentUpdateController(IInstallationAndEquipmentUpdateHadler updateHadler)
        {
            _updateHadler = updateHadler;
            _updateHadler.NotNull(nameof(updateHadler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InstallationAndEquipmentUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] InstallationAndEquipmentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _updateHadler.Handle(updateDto, CurrentUser, cancellationToken);

            return Ok(updateDto);
        }
    }
}
