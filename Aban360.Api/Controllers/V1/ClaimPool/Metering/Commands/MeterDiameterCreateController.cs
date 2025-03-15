using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/meter-diameter")]
    public class MeterDiameterCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterDiameterCreateHandler _meterDiameterHandler;
        public MeterDiameterCreateController(
            IUnitOfWork uow,
            IMeterDiameterCreateHandler meterDiameterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterDiameterHandler = meterDiameterHandler;
            _meterDiameterHandler.NotNull(nameof(meterDiameterHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterDiameterCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MeterDiameterCreateDto createDto, CancellationToken cancellationToken)
        {
            await _meterDiameterHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
