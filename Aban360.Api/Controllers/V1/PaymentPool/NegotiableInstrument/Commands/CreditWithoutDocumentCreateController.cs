using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands
{
    [Route("v1/credit-without-document")]
    public class CreditWithoutDocumentCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICreditWithoutDocumentCreateHandler _creditWithoutDocumentCreateHandler;
        private readonly IDocumentCreateHandler _documentCreateHandler;

        public CreditWithoutDocumentCreateController(
           IUnitOfWork uow,
           ICreditWithoutDocumentCreateHandler creditWithoutDocumentCreateHandler,
           IDocumentCreateHandler documentCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _creditWithoutDocumentCreateHandler = creditWithoutDocumentCreateHandler;
            _creditWithoutDocumentCreateHandler.NotNull(nameof(creditWithoutDocumentCreateHandler));

            _documentCreateHandler = documentCreateHandler;
            _documentCreateHandler.NotNull(nameof(documentCreateHandler));

        }

        [HttpPost, HttpGet]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreditWithoutDocumentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreditWithoutDocumentCreateDto createDto, CancellationToken cancellationToken)
        {
            Guid documentId= Guid.Empty;
            if (createDto.DocumentFile != null)
                documentId = await _documentCreateHandler.Handle(createDto.DocumentFile, createDto.DocumentTypeId, createDto.Description, cancellationToken);


            await _creditWithoutDocumentCreateHandler.Handle(CurrentUser, createDto,documentId, cancellationToken);
            await _uow.SaveChangesAsync();
            return Ok(createDto);
        }
    }
}
