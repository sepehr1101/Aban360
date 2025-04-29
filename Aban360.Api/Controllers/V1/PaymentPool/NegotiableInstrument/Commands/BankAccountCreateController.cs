using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands
{
    [Route("v1/bank-account")]
    public class BankAccountCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IBankAccountCreateHandler _bankAccountCreateHandler;
        public BankAccountCreateController(
            IUnitOfWork uow,
            IBankAccountCreateHandler bankAccountCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _bankAccountCreateHandler = bankAccountCreateHandler;
            _bankAccountCreateHandler.NotNull(nameof(bankAccountCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankAccountCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] BankAccountCreateDto createDto, CancellationToken cancellationToken)
        {
            await _bankAccountCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }

}
