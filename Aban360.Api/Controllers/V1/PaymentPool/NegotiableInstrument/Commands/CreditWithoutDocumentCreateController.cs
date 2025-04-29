using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands
{
    [Route("v1/credit-without-document")]
    public class CreditWithoutDocumentCreateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICreditWithoutDocumentCreateHandler _creditWithoutDocumentCreateHandler;
        public CreditWithoutDocumentCreateController(
           IUnitOfWork uow,
           ICreditWithoutDocumentCreateHandler creditWithoutDocumentCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _creditWithoutDocumentCreateHandler = creditWithoutDocumentCreateHandler;
            _creditWithoutDocumentCreateHandler.NotNull(nameof(creditWithoutDocumentCreateHandler));
        }

        [HttpPost, HttpGet]
        [Route("create")]
        public async Task<IActionResult> Create(CreditWithoutDocumentCreateDto createDto, CancellationToken cancellationToken)
        {
            await _creditWithoutDocumentCreateHandler.Handle(CurrentUser, createDto, cancellationToken);
            await _uow.SaveChangesAsync();
            return Ok();
        }
    }
}
