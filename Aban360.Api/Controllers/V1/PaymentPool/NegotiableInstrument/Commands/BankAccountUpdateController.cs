using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Contracts;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands
{
    [Route("v1/bank-account")]
    public class BankAccountUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IBankAccountUpdateHandler _bankAccountUpdateHandler;
        public BankAccountUpdateController(
            IUnitOfWork uow,
            IBankAccountUpdateHandler bankAccountUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _bankAccountUpdateHandler = bankAccountUpdateHandler;
            _bankAccountUpdateHandler.NotNull(nameof(bankAccountUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankAccountUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] BankAccountUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _bankAccountUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }

}
