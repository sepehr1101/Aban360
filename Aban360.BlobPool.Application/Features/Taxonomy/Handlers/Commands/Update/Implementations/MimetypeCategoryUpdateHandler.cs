using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Implementations
{
    internal sealed class MimetypeCategoryUpdateHandler : IMimetypeCategoryUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMimetypeCategoryQueryService _mimetypeCategoryQueryService;
        public MimetypeCategoryUpdateHandler(
            IMapper mapper,
            IMimetypeCategoryQueryService mimetypeCategoryQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _mimetypeCategoryQueryService = mimetypeCategoryQueryService;
            _mimetypeCategoryQueryService.NotNull(nameof(_mimetypeCategoryQueryService));
        }

        public async Task Handle(MimetypeCategoryUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var mimetypeCategory = await _mimetypeCategoryQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, mimetypeCategory);
        }
    }
}
