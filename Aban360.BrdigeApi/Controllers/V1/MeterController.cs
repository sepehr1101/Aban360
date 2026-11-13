using Aban360.CalculationPool.Domain.Features.MeterChange.Dto;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations;
using Aban360.BrdigeApi.Controllers.V1.CalculationPool;
using Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo;

namespace Aban360.BrdigeApi.Controllers.V1
{
    public class MeterController : BaseController
    {
        private readonly IChangeMeterCauseGetAllHandler _c1_changeMeterCause;
        public MeterController(IChangeMeterCauseGetAllHandler c1_changeMeterCause)
        {
            _c1_changeMeterCause = c1_changeMeterCause;
            _c1_changeMeterCause.NotNull(nameof(_c1_changeMeterCause));
        }

        [HttpPost]
        [Route("~/v1/meter/set-change")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterChangeSetInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetChange(MeterChangeSetInputDto input, CancellationToken cancellationToken)
        {
            //Set MeterChange
            return Ok(input);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/meter/change-cuase")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeCuase(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _c1_changeMeterCause.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/meter/change-cause")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeCause(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _c1_changeMeterCause.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
