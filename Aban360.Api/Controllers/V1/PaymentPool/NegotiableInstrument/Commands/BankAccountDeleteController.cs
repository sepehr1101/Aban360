using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Contracts;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands
{
    [Route("v1/bank-account")]
    public class BankAccountDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IBankAccountDeleteHandler _bankAccountDeleteHandler;
        public BankAccountDeleteController(
            IUnitOfWork uow,
            IBankAccountDeleteHandler bankAccountDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _bankAccountDeleteHandler = bankAccountDeleteHandler;
            _bankAccountDeleteHandler.NotNull(nameof(bankAccountDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankAccountDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] BankAccountDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _bankAccountDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }

}
