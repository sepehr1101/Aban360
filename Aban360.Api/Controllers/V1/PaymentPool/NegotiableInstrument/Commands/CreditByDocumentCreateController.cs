using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.NegotiableInstrument.Dto.Commands;
using Aban360.PaymentPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Aban360.BlobPool.Application.Features.Taxonomy.Services;
using Aban360.PaymentPool.Application.Features.NegotiableInstrument.Services;

namespace Aban360.Api.Controllers.V1.PaymentPool.NegotiableInstrument.Commands
{
    [Route("v1/credit-by-document")]
    public class CreditByDocumentCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICreditService _creditService;
        private readonly IDocumentService _documentService;
        public CreditByDocumentCreateController(IUnitOfWork uow, ICreditService creditService, IDocumentService documentService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _creditService = creditService;
            _creditService.NotNull(nameof(creditService));
            _documentService = documentService;
            _documentService.NotNull(nameof(documentService));
        }

        [HttpPost, HttpGet]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreditWithDocumentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] CreditWithDocumentCreateDto creditWithDocumentCreateDto, CancellationToken cancellationToken)
        {
            Guid documentdId = await _documentService.CreateAsync(creditWithDocumentCreateDto.DocumentFile, creditWithDocumentCreateDto.DocumentTypeId, creditWithDocumentCreateDto.Description, cancellationToken);
            await _creditService.CreateByDocumentAsync(CurrentUser, creditWithDocumentCreateDto.LetterNumber, creditWithDocumentCreateDto.BankId, documentdId, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(creditWithDocumentCreateDto);
        }
    }
}
