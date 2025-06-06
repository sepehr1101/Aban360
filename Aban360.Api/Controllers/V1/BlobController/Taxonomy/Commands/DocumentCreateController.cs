﻿using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.Taxonomy.Commands
{
    [Route("v1/document")]
    public class DocumentCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDocumentCreateHandler _documentCreateHandler;
        public DocumentCreateController(
            IUnitOfWork uow,
            IDocumentCreateHandler documentCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _documentCreateHandler = documentCreateHandler;
            _documentCreateHandler.NotNull(nameof(documentCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DocumentCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] DocumentCreateDto createDto, CancellationToken cancellationToken)
        {
            await _documentCreateHandler.Handle(createDto.DocumentFile, createDto.DocumentTypeId, createDto.Description, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
