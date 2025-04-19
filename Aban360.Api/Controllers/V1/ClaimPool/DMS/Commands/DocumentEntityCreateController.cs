using Aban360.ClaimPool.Application.Features.DMS.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.DMS.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.DMS.Commands
{
    [Route("v1/document-entity")]
    public class DocumentEntityCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentEntityCreateHandler _documentEntityCreateHandler;
        public DocumentEntityCreateController(
            IUnitOfWork uow,
            IDocumentEntityCreateHandler documentEntityCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentEntityCreateHandler = documentEntityCreateHandler;
            _documentEntityCreateHandler.NotNull(nameof(documentEntityCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentEntityCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] DocumentEntityCreateDto createDto, CancellationToken cancellationToken)
        {
            await _documentEntityCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
