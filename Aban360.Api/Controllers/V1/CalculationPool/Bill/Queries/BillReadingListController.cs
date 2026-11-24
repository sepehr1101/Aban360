using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Bill.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/bill-reading-list")]
    public class BillReadingListController : BaseController
    {
        private readonly IBillListsService _billReadingListGetHandler;
        public BillReadingListController(IBillListsService billReadingListGetHandler)
        {
            _billReadingListGetHandler = billReadingListGetHandler;
            _billReadingListGetHandler.NotNull(nameof(billReadingListGetHandler));
        }

        [HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<BillReadingListHeaderOutputDto, BillReadingListDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromBody] BillReadingListInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<BillReadingListHeaderOutputDto, BillReadingListDataOutputDto> companyServiceTypes = await _billReadingListGetHandler.BillReadingListGetAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(companyServiceTypes);
        }
    }
}
