using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands
{
    [Route("v1/credit-by-document")]
    public class CreditByDocumentCreateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICreditByDocumentCreateHandler _creditByDocumentHandler;
        public CreditByDocumentCreateController(
           IUnitOfWork uow,
           ICreditByDocumentCreateHandler creditByDocumentHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _creditByDocumentHandler = creditByDocumentHandler;
            _creditByDocumentHandler.NotNull(nameof(creditByDocumentHandler));
        }

        [HttpPost, HttpGet]
        [Route("create")]
        public async Task<IActionResult> Create(CreditByDocumentCreateDto createDto, CancellationToken cancellationToken)
        {
            await _creditByDocumentHandler.Handle(CurrentUser, createDto, cancellationToken);
            await _uow.SaveChangesAsync();
            return Ok();
        }
    }
}
