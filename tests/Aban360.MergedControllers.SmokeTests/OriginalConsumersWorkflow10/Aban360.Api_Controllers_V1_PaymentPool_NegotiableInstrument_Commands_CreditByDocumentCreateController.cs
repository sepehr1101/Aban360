using global::Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands;
using Microsoft.Extensions.Logging;
using global::Aban360.Api.Controllers.V1;
using global::Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using global::Aban360.Common.Categories.ApiResponse;
using global::Aban360.Common.Extensions;
using global::Aban360.PaymentPool.Application.Features.NegotiableInstrument.Handler.Commands.Create.Contracts;
using global::Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using global::Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using global::Aban360.BlobPool.Application.Features.Taxonomy.Services;

namespace BeforeConsumersWorkflow10.Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands{
    [Route("v1/credit-by-document")]
    public class CreditByDocumentCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICreditByDocumentCreateHandler _creditByDocumentHandler;
        private readonly IDocumentService _documentService;
        public CreditByDocumentCreateController(
        IUnitOfWork uow,
        ICreditByDocumentCreateHandler creditByDocumentHandler,
        IDocumentService documentService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _creditByDocumentHandler = creditByDocumentHandler;
            _creditByDocumentHandler.NotNull(nameof(creditByDocumentHandler));
            _documentService = documentService;
            _documentService.NotNull(nameof(documentService));
        }

        [HttpPost, HttpGet]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreditWithDocumentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] CreditWithDocumentCreateDto creditWithDocumentCreateDto, CancellationToken cancellationToken)
        {
            Guid documentdId = await _documentService.CreateAsync(creditWithDocumentCreateDto.DocumentFile, creditWithDocumentCreateDto.DocumentTypeId, creditWithDocumentCreateDto.Description, cancellationToken);
            await _creditByDocumentHandler.Handle(CurrentUser, creditWithDocumentCreateDto.LetterNumber, creditWithDocumentCreateDto.BankId, documentdId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(creditWithDocumentCreateDto);
        }
    }
}
