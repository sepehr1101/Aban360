using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Contracts;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands
{
    [Route("v1/bank-file-structure")]
    public class BankFileStructureUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IBankFileStructureUpdateHandler _bankFileStructureUpdateHandler;
        public BankFileStructureUpdateController(
            IUnitOfWork uow,
            IBankFileStructureUpdateHandler bankFileStructureUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _bankFileStructureUpdateHandler = bankFileStructureUpdateHandler;
            _bankFileStructureUpdateHandler.NotNull(nameof(bankFileStructureUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankFileStructureUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] BankFileStructureUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _bankFileStructureUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
