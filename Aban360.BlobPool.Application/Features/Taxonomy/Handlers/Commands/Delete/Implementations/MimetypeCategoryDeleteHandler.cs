using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Implementations
{
    internal sealed class MimetypeCategoryDeleteHandler : IMimetypeCategoryDeleteHandler
    {
        private readonly IMimetypeCategoryCommandService _mimetypeCategoryCommandService;
        private readonly IMimetypeCategoryQueryService _mimetypeCategoryQueryService;
        public MimetypeCategoryDeleteHandler(
            IMimetypeCategoryCommandService mimetypeCategoryCommandService,
            IMimetypeCategoryQueryService mimetypeCategoryQueryService)
        {
            _mimetypeCategoryCommandService = mimetypeCategoryCommandService;
            _mimetypeCategoryCommandService.NotNull(nameof(_mimetypeCategoryCommandService));

            _mimetypeCategoryQueryService = mimetypeCategoryQueryService;
            _mimetypeCategoryQueryService.NotNull(nameof(_mimetypeCategoryQueryService));
        }

        public async Task Handle(MimetypeCategoryDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var mimetypeCategory = await _mimetypeCategoryQueryService.Get(deleteDto.Id);
            _mimetypeCategoryCommandService.Remove(mimetypeCategory);
        }
    }
}
