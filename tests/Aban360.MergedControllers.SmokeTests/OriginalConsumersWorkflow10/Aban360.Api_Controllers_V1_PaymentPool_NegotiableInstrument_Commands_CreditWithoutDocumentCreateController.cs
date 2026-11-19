using global::Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands;
using Microsoft.Extensions.Logging;
using global::Aban360.Api.Controllers.V1;
using global::Aban360.Common.Categories.ApiResponse;
using global::Aban360.Common.Extensions;
using global::Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using global::Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using global::Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using global::Aban360.BlobPool.Application.Features.Taxonomy.Services;

namespace BeforeConsumersWorkflow10.Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands{
    [Route("v1/credit-without-document")]
    public class CreditWithoutDocumentCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICreditWithoutDocumentCreateHandler _creditWithoutDocumentCreateHandler;
        private readonly IDocumentService _documentService;
        public CreditWithoutDocumentCreateController(
        IUnitOfWork uow,
        ICreditWithoutDocumentCreateHandler creditWithoutDocumentCreateHandler,
        IDocumentService documentService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _creditWithoutDocumentCreateHandler = creditWithoutDocumentCreateHandler;
            _creditWithoutDocumentCreateHandler.NotNull(nameof(creditWithoutDocumentCreateHandler));
            _documentService = documentService;
            _documentService.NotNull(nameof(documentService));
        }

        [HttpPost, HttpGet]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreditWithoutDocumentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreditWithoutDocumentCreateDto createDto, CancellationToken cancellationToken)
        {
            Guid documentId = Guid.Empty;
            if (createDto.DocumentFile != null)
                documentId = await _documentService.CreateAsync(createDto.DocumentFile, createDto.DocumentTypeId, createDto.Description, cancellationToken);
            await _creditWithoutDocumentCreateHandler.Handle(CurrentUser, createDto, documentId, cancellationToken);
            await _uow.SaveChangesAsync();
            return Ok(createDto);
        }
    }
}
