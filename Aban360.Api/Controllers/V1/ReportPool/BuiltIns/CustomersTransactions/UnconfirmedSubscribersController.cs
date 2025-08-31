using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/unconfirmed-subscribers")]
    public class UnconfirmedSubscribersController : BaseController
    {
        private readonly IUnconfirmedSubscribersHandler _unconfirmedSubscribersHandler;
        private readonly IReportGenerator _reportGenerator;
        public UnconfirmedSubscribersController(
            IUnconfirmedSubscribersHandler unconfirmedSubscribersHandler,
            IReportGenerator reportGenerator)
        {
            _unconfirmedSubscribersHandler = unconfirmedSubscribersHandler;
            _unconfirmedSubscribersHandler.NotNull(nameof(unconfirmedSubscribersHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnconfirmedSubscribersHeaderOutputDto, UnconfirmedSubscribersDataOutputDto>>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UnconfirmedSubscribersInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<UnconfirmedSubscribersHeaderOutputDto, UnconfirmedSubscribersDataOutputDto> unconfirmedSubscribers = await _unconfirmedSubscribersHandler.Handle(input, cancellationToken);
            return Ok(unconfirmedSubscribers);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UnconfirmedSubscribersInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _unconfirmedSubscribersHandler.Handle, CurrentUser, ReportLiterals.UnconfirmedSubscribers, connectionId);
            return Ok(inputDto);
        }
    }
}
