using Aban360.ClaimPool.Application.Features.DMS.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.DMS.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.DMS.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.DMS.Handlers.Queries.Implementations
{
    internal sealed class DocumentEntityGetAllHandler : IDocumentEntityGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IDocumentEntityQueryService _documentEntityQueryService;
        public DocumentEntityGetAllHandler(
            IMapper mapper,
            IDocumentEntityQueryService documentEntityQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _documentEntityQueryService = documentEntityQueryService;
            _documentEntityQueryService.NotNull(nameof(_documentEntityQueryService));
        }

        public async Task<ICollection<DocumentEntityGetDto>> Handle(CancellationToken cancellationToken)
        {
            var documentEntity = await _documentEntityQueryService.Get();
            return _mapper.Map<ICollection<DocumentEntityGetDto>>(documentEntity);
        }
    }
}
