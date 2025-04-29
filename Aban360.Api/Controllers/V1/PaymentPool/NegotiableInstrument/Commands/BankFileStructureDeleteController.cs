using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Delete.Contracts;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands
{
    [Route("v1/bank-file-structure")]
    public class BankFileStructureDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IBankFileStructureDeleteHandler _bankFileStructureDeleteHandler;
        public BankFileStructureDeleteController(
            IUnitOfWork uow,
            IBankFileStructureDeleteHandler bankFileStructureDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _bankFileStructureDeleteHandler = bankFileStructureDeleteHandler;
            _bankFileStructureDeleteHandler.NotNull(nameof(bankFileStructureDeleteHandler));
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
    }
}
