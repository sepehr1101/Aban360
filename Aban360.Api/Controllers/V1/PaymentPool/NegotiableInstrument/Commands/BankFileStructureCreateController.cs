using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands
{
    [Route("v1/bank-file-structure")]
    public class BankFileStructureCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IBankFileStructureCreateHandler _bankFileStructureCreateHandler;
        public BankFileStructureCreateController(
            IUnitOfWork uow,
            IBankFileStructureCreateHandler bankFileStructureCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _bankFileStructureCreateHandler = bankFileStructureCreateHandler;
            _bankFileStructureCreateHandler.NotNull(nameof(bankFileStructureCreateHandler));
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
    }
}
