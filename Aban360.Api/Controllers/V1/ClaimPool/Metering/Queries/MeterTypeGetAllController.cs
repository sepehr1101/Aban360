using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/meter-type")]
    public class MeterTypeGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterTypeGetAllHandler _meterTypeHandler;
        public MeterTypeGetAllController(
            IUnitOfWork uow,
            IMeterTypeGetAllHandler meterTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterTypeHandler = meterTypeHandler;
            _meterTypeHandler.NotNull(nameof(meterTypeHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<MeterTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<MeterTypeGetDto> meterTypes = await _meterTypeHandler.Handle(cancellationToken);
            return Ok(meterTypes);
        }
    }
}
