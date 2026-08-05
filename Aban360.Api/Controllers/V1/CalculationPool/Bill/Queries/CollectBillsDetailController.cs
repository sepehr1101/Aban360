using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/collect-bills-detail")]
    public class CollectBillsDetailController : BaseController
    {
        private readonly ICollectBillsDetailGetAllHandler _collectBillsDetailGetAllHandler;
        public CollectBillsDetailController(ICollectBillsDetailGetAllHandler collectBillsDetailGetAllHandler)
        {
            _collectBillsDetailGetAllHandler = collectBillsDetailGetAllHandler;
            _collectBillsDetailGetAllHandler.NotNull(nameof(collectBillsDetailGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<CollectBillsDetailGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<CollectBillsDetailGetDto> result = await _collectBillsDetailGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
