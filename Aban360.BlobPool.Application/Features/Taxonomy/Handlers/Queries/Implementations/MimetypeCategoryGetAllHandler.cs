using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Implementations
{
    internal sealed class MimetypeCategoryGetAllHandler : IMimetypeCategoryGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IMimetypeCategoryQueryService _mimetypeCategoryQueryService;
        public MimetypeCategoryGetAllHandler(
            IMapper mapper,
            IMimetypeCategoryQueryService mimetypeCategoryQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _mimetypeCategoryQueryService = mimetypeCategoryQueryService;
            _mimetypeCategoryQueryService.NotNull(nameof(_mimetypeCategoryQueryService));
        }

        public async Task<ICollection<MimetypeCategoryGetDto>> Handle(CancellationToken cancellationToken)
        {
            var mimetypeCategory = await _mimetypeCategoryQueryService.Get();
            return _mapper.Map<ICollection<MimetypeCategoryGetDto>>(mimetypeCategory);
        }
    }
}
