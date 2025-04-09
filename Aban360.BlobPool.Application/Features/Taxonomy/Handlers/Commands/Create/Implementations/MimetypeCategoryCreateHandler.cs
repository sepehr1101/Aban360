using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Implementations
{
    internal sealed class MimetypeCategoryCreateHandler : IMimetypeCategoryCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMimetypeCategoryCommandService _mimetypeCategoryCommandService;
        public MimetypeCategoryCreateHandler(
            IMapper mapper,
            IMimetypeCategoryCommandService mimetypeCategoryCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _mimetypeCategoryCommandService = mimetypeCategoryCommandService;
            _mimetypeCategoryCommandService.NotNull(nameof(_mimetypeCategoryCommandService));
        }

        public async Task Handle(MimetypeCategoryCreateDto createDto, CancellationToken cancellationToken)
        {
            var mimetypeCategory = _mapper.Map<MimetypeCategory>(createDto);
            await _mimetypeCategoryCommandService.Add(mimetypeCategory);
        }
    }
}
