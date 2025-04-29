using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Queries
{
    [Route("v1/bank-file-structure")]
    public class BankFileStructureGetAllController : BaseController
    {
        private readonly IBankFileStructureGetAllHandler _bankFileStructureGetAllHandler;
        public BankFileStructureGetAllController(IBankFileStructureGetAllHandler bankFileStructureGetAllHandler)
        {
            _bankFileStructureGetAllHandler = bankFileStructureGetAllHandler;
            _bankFileStructureGetAllHandler.NotNull(nameof(bankFileStructureGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<BankFileStructureGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var bankFileStructures = await _bankFileStructureGetAllHandler.Handle(cancellationToken);
            return Ok(bankFileStructures);
        }
    }
}
