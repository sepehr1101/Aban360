// Frozen consumer before service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
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

namespace BeforeServiceConsumers15.Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument
{
    [Route("v1/bank-file-structure")]
    public class BankFileStructureController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IBankFileStructureCreateHandler _bankFileStructureCreateHandler;
        private readonly IBankFileStructureUpdateHandler _bankFileStructureUpdateHandler;
        private readonly IBankFileStructureDeleteHandler _bankFileStructureDeleteHandler;
        private readonly IBankFileStructureGetSingleHandler _bankFileStructureGetSingleHandler;
        private readonly IBankFileStructureGetAllHandler _bankFileStructureGetAllHandler;

        public BankFileStructureController(
            IUnitOfWork uow,
            IBankFileStructureCreateHandler bankFileStructureCreateHandler,
            IBankFileStructureUpdateHandler bankFileStructureUpdateHandler,
            IBankFileStructureDeleteHandler bankFileStructureDeleteHandler,
            IBankFileStructureGetSingleHandler bankFileStructureGetSingleHandler,
            IBankFileStructureGetAllHandler bankFileStructureGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _bankFileStructureCreateHandler = bankFileStructureCreateHandler;
            _bankFileStructureCreateHandler.NotNull(nameof(bankFileStructureCreateHandler));

            _bankFileStructureUpdateHandler = bankFileStructureUpdateHandler;
            _bankFileStructureUpdateHandler.NotNull(nameof(bankFileStructureUpdateHandler));

            _bankFileStructureDeleteHandler = bankFileStructureDeleteHandler;
            _bankFileStructureDeleteHandler.NotNull(nameof(bankFileStructureDeleteHandler));

            _bankFileStructureGetSingleHandler = bankFileStructureGetSingleHandler;
            _bankFileStructureGetSingleHandler.NotNull(nameof(bankFileStructureGetSingleHandler));

            _bankFileStructureGetAllHandler = bankFileStructureGetAllHandler;
            _bankFileStructureGetAllHandler.NotNull(nameof(bankFileStructureGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankFileStructureCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] BankFileStructureCreateDto createDto, CancellationToken cancellationToken)
        {
            await _bankFileStructureCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
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

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankFileStructureDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] BankFileStructureDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _bankFileStructureDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BankFileStructureGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var bankFileStructures = await _bankFileStructureGetSingleHandler.Handle(id, cancellationToken);
            return Ok(bankFileStructures);
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
