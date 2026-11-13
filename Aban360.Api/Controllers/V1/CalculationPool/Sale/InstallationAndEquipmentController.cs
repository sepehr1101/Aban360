using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.CalculationPool.Sale
{
    public class InstallationAndEquipmentController : BaseController
    {
        private readonly IInstallationAndEquipmentCreateHadler _c0_createHadler;
        private readonly IInstallationAndEquipmentDeleteHadler _c1_deleteHadler;
        private readonly IInstallationAndEquipmentUpdateHadler _c2_updateHadler;
        private readonly IInstallationAndEquipmentGetAllHandler _c3_getHandler;
        private readonly IInstallationAndEquipmentGetHandler _c4_getHandler;
        public InstallationAndEquipmentController(IInstallationAndEquipmentCreateHadler c0_createHadler, IInstallationAndEquipmentDeleteHadler c1_deleteHadler, IInstallationAndEquipmentUpdateHadler c2_updateHadler, IInstallationAndEquipmentGetAllHandler c3_getHandler, IInstallationAndEquipmentGetHandler c4_getHandler)
        {
            _c0_createHadler = c0_createHadler;
            _c0_createHadler.NotNull(nameof(c0_createHadler));
            _c1_deleteHadler = c1_deleteHadler;
            _c1_deleteHadler.NotNull(nameof(c1_deleteHadler));
            _c2_updateHadler = c2_updateHadler;
            _c2_updateHadler.NotNull(nameof(c2_updateHadler));
            _c3_getHandler = c3_getHandler;
            _c3_getHandler.NotNull(nameof(c3_getHandler));
            _c4_getHandler = c4_getHandler;
            _c4_getHandler.NotNull(nameof(c4_getHandler));
        }

        [HttpPost]
        [Route("~/v1/installation-and-equipment/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InstallationAndEquipmentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] InstallationAndEquipmentCreateDto createDto, CancellationToken cancellationToken)
        {
            await _c0_createHadler.Handle(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("~/v1/installation-and-equipment/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SearchById input, CancellationToken cancellationToken)
        {
            await _c1_deleteHadler.Handle(input, CurrentUser, cancellationToken);
            return Ok(input);
        }

        [HttpPost]
        [Route("~/v1/installation-and-equipment/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<InstallationAndEquipmentUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] InstallationAndEquipmentUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _c2_updateHadler.Handle(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet]
        [Route("~/v1/installation-and-equipment/get-all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<InstallationAndEquipmentOutputDto> result = await _c3_getHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/installation-and-equipment/get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchById>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] SearchById inputDto, CancellationToken cancellationToken)
        {
            InstallationAndEquipmentOutputDto result = await _c4_getHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
