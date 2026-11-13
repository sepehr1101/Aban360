// Frozen original consumer; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Contracts;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Update.Contracts;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Queries;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers20.Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument
{
    [Route("v1/bank-account")]
    public class BankAccountController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IBankAccountCreateHandler _bankAccountCreateHandler;
        private readonly IBankAccountUpdateHandler _bankAccountUpdateHandler;
        private readonly IBankAccountDeleteHandler _bankAccountDeleteHandler;
        private readonly IBankAccountGetSingleHandler _bankAccountGetSingleHandler;
        private readonly IBankAccountGetAllHandler _bankAccountGetAllHandler;

        public BankAccountController(
            IUnitOfWork uow,
            IBankAccountCreateHandler bankAccountCreateHandler,
            IBankAccountUpdateHandler bankAccountUpdateHandler,
            IBankAccountDeleteHandler bankAccountDeleteHandler,
            IBankAccountGetSingleHandler bankAccountGetSingleHandler,
            IBankAccountGetAllHandler bankAccountGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _bankAccountCreateHandler = bankAccountCreateHandler;
            _bankAccountCreateHandler.NotNull(nameof(bankAccountCreateHandler));

            _bankAccountUpdateHandler = bankAccountUpdateHandler;
            _bankAccountUpdateHandler.NotNull(nameof(bankAccountUpdateHandler));

            _bankAccountDeleteHandler = bankAccountDeleteHandler;
            _bankAccountDeleteHandler.NotNull(nameof(bankAccountDeleteHandler));

            _bankAccountGetSingleHandler = bankAccountGetSingleHandler;
            _bankAccountGetSingleHandler.NotNull(nameof(bankAccountGetSingleHandler));

            _bankAccountGetAllHandler = bankAccountGetAllHandler;
            _bankAccountGetAllHandler.NotNull(nameof(bankAccountGetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankAccountUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] BankAccountUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _bankAccountUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
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

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankAccountGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var bankAccounts = await _bankAccountGetSingleHandler.Handle(id, cancellationToken);
            return Ok(bankAccounts);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<BankAccountGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var bankAccounts = await _bankAccountGetAllHandler.Handle(cancellationToken);
            return Ok(bankAccounts);
        }
    }
}
