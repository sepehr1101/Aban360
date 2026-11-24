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
    [Route("v1/credit-without-document")]
    public class CreditWithoutDocumentCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICreditService _creditService;
        private readonly IDocumentService _documentService;
        public CreditWithoutDocumentCreateController(IUnitOfWork uow, ICreditService creditService, IDocumentService documentService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _creditService = creditService;
            _creditService.NotNull(nameof(creditService));
            _documentService = documentService;
            _documentService.NotNull(nameof(documentService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreditWithoutDocumentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreditWithoutDocumentCreateDto createDto, CancellationToken cancellationToken)
        {
            Guid documentId = Guid.Empty;
            if (createDto.DocumentFile != null)
                documentId = await _documentService.CreateAsync(createDto.DocumentFile, createDto.DocumentTypeId, createDto.Description, cancellationToken);
            await _creditService.CreateWithoutDocumentAsync(CurrentUser, createDto, documentId, cancellationToken);
            await _uow.SaveChangesAsync();
            return Ok(createDto);
        }
    }
}
