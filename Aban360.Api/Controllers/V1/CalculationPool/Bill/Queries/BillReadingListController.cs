using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/bill-reading-list")]
    public class BillReadingListController : BaseController
    {
        private readonly IBillReadingListGetHandler _billReadingListGetHandler;
        public BillReadingListController(IBillReadingListGetHandler billReadingListGetHandler)
        {
            _billReadingListGetHandler = billReadingListGetHandler;
            _billReadingListGetHandler.NotNull(nameof(billReadingListGetHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillReadingListHeaderOutputDto, BillReadingListDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromBody] BillReadingListInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BillReadingListHeaderOutputDto, BillReadingListDataOutputDto> companyServiceTypes = await _billReadingListGetHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(companyServiceTypes);
        }
    }
}
