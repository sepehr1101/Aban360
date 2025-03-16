using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/meter-producer")]
    public class MeterProducerGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterProducerGetSingleHandler _meterProducerHandler;
        public MeterProducerGetSingleController(
            IUnitOfWork uow,
            IMeterProducerGetSingleHandler meterProducerHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterProducerHandler = meterProducerHandler;
            _meterProducerHandler.NotNull(nameof(meterProducerHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterProducerGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            MeterProducerGetDto meterProducer = await _meterProducerHandler.Handle(id, cancellationToken);
            return Ok(meterProducer);
        }
    }
}
