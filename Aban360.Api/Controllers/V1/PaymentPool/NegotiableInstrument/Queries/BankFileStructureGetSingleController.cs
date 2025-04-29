using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Queries
{
    [Route("v1/bank-file-structure")]
    public class BankFileStructureGetSingleController : BaseController
    {
        private readonly IBankFileStructureGetSingleHandler _bankFileStructureGetSingleHandler;
        public BankFileStructureGetSingleController(IBankFileStructureGetSingleHandler bankFileStructureGetSingleHandler)
        {
            _bankFileStructureGetSingleHandler = bankFileStructureGetSingleHandler;
            _bankFileStructureGetSingleHandler.NotNull(nameof(bankFileStructureGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankFileStructureGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var bankFileStructures = await _bankFileStructureGetSingleHandler.Handle(id, cancellationToken);
            return Ok(bankFileStructures);
        }
    }
}
