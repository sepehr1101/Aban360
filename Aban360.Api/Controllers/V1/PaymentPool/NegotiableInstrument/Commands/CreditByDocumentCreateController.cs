using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands
{
    [Route("v1/credit-by-document")]
    public class CreditByDocumentCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICreditByDocumentCreateHandler _creditByDocumentHandler;
        private readonly IDocumentCreateHandler _documentCreateHandler;

        public CreditByDocumentCreateController(
           IUnitOfWork uow,
           ICreditByDocumentCreateHandler creditByDocumentHandler,
           IDocumentCreateHandler documentCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _creditByDocumentHandler = creditByDocumentHandler;
            _creditByDocumentHandler.NotNull(nameof(creditByDocumentHandler));

            _documentCreateHandler = documentCreateHandler;
            _documentCreateHandler.NotNull(nameof(documentCreateHandler));

        }
        [HttpPost, HttpGet]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreditWithDocumentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] CreditWithDocumentCreateDto creditWithDocumentCreateDto, CancellationToken cancellationToken)
        {
            Guid documentdId = await _documentCreateHandler.Handle(creditWithDocumentCreateDto.DocumentFile, creditWithDocumentCreateDto.DocumentTypeId, creditWithDocumentCreateDto.Description, cancellationToken);
            await _creditByDocumentHandler.Handle(CurrentUser, creditWithDocumentCreateDto.LetterNumber,creditWithDocumentCreateDto.BankId, documentdId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(creditWithDocumentCreateDto);
        }
    }
}
