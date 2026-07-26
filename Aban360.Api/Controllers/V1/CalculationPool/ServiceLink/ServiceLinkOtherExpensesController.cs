using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.ServiceLink
{
    [Route("v1/service-link-other-expenses")]
    public class ServiceLinkOtherExpensesController : BaseController
    {
        private readonly IOtherExpensesInsertHandler _otherExpensesInsertHandler;
        public ServiceLinkOtherExpensesController(IOtherExpensesInsertHandler otherExpensesInsertHandler)
        {
            _otherExpensesInsertHandler = otherExpensesInsertHandler;
            _otherExpensesInsertHandler.NotNull(nameof(otherExpensesInsertHandler));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] OtherExpensesInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 2540;
            ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto> result= await _otherExpensesInsertHandler.Handle(inputDto, CurrentUser, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
