using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale.Command
{
    [Route("v1/installation-and-equipment")]
    public class InstallationAndEquipmentCreateController : BaseController
    {
        private readonly IInstallationAndEquipmentCreateHadler _createHadler;
        public InstallationAndEquipmentCreateController(IInstallationAndEquipmentCreateHadler createHadler)
        {
            _createHadler = createHadler;
            _createHadler.NotNull(nameof(createHadler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InstallationAndEquipmentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] InstallationAndEquipmentCreateDto createDto, CancellationToken cancellationToken)
        {
            await _createHadler.Handle(createDto, CurrentUser, cancellationToken);

            return Ok(createDto);
        }
    }
}
