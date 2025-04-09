using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Implementations
{
    internal sealed class MimetypeCategoryGetSingleHandler : IMimetypeCategoryGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IMimetypeCategoryQueryService _mimetypeCategoryQueryService;
        public MimetypeCategoryGetSingleHandler(
            IMapper mapper,
            IMimetypeCategoryQueryService mimetypeCategoryQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _mimetypeCategoryQueryService = mimetypeCategoryQueryService;
            _mimetypeCategoryQueryService.NotNull(nameof(_mimetypeCategoryQueryService));
        }

        public async Task<MimetypeCategoryGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var mimetypeCategory = await _mimetypeCategoryQueryService.Get(id);
            return _mapper.Map<MimetypeCategoryGetDto>(mimetypeCategory);
        }
    }
}
