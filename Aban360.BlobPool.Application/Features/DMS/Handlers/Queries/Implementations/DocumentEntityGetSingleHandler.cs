﻿using Aban360.BlobPool.Domain.Features.DMS.Dto.Queries;
using Aban360.BlobPool.Application.Features.DMS.Handlers.Queries.Contracts;
using Aban360.BlobPool.Persistence.Features.DMS.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.DMS.Handlers.Queries.Implementations
{
    internal sealed class DocumentEntityGetSingleHandler : IDocumentEntityGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentEntityQueryService _documentEntityQueryService;
        public DocumentEntityGetSingleHandler(
            IMapper mapper,
            IDocumentEntityQueryService documentEntityQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentEntityQueryService = documentEntityQueryService;
            _documentEntityQueryService.NotNull(nameof(_documentEntityQueryService));
        }

        public async Task<DocumentEntityGetDto> Handle(long id, CancellationToken cancellationToken)
        {
            var documentEntity = await _documentEntityQueryService.Get(id);
            return _mapper.Map<DocumentEntityGetDto>(documentEntity);
        }
    }
}
