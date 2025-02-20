using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/meter-use-type")]
    public class MeterUseTypeUpdateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterUseTypeUpdateHandler _meterUseTypeHandler;
        public MeterUseTypeUpdateController(
            IUnitOfWork uow,
            IMeterUseTypeUpdateHandler meterUseTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterUseTypeHandler = meterUseTypeHandler;
            _meterUseTypeHandler.NotNull(nameof(meterUseTypeHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterUseTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] MeterUseTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _meterUseTypeHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
