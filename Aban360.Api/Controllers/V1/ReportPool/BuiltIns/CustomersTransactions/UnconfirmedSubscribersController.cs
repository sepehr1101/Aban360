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
        public UnconfirmedSubscribersController(IUnconfirmedSubscribersHandler unconfirmedSubscribersHandler)
        {
            _unconfirmedSubscribersHandler = unconfirmedSubscribersHandler;
            _unconfirmedSubscribersHandler.NotNull(nameof(unconfirmedSubscribersHandler));
        }

        [HttpPost, HttpGet]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnconfirmedSubscribersHeaderOutputDto, UnconfirmedSubscribersDataOutputDto>>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfo(UnconfirmedSubscribersInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<UnconfirmedSubscribersHeaderOutputDto, UnconfirmedSubscribersDataOutputDto> unconfirmedSubscribers = await _unconfirmedSubscribersHandler.Handle(input, cancellationToken);
            return Ok(unconfirmedSubscribers);
        }
    }
}
