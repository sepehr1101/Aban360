using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Delete.Implementations
{
    internal sealed class ExecutableMimetypeDeleteHandler : IExecutableMimetypeDeleteHandler
    {
        private readonly IExecutableMimetypeCommandService _executableMimetypeCommandService;
        private readonly IExecutableMimetypeQueryService _executableMimetypeQueryService;
        public ExecutableMimetypeDeleteHandler(
            IExecutableMimetypeCommandService executableMimetypeCommandService,
            IExecutableMimetypeQueryService executableMimetypeQueryService)
        {
            _executableMimetypeCommandService = executableMimetypeCommandService;
            _executableMimetypeCommandService.NotNull(nameof(_executableMimetypeCommandService));

            _executableMimetypeQueryService = executableMimetypeQueryService;
            _executableMimetypeQueryService.NotNull(nameof(_executableMimetypeQueryService));
        }

        public async Task Handle(ExecutableMimetypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var executableMimetype = await _executableMimetypeQueryService.Get(deleteDto.Id);
            _executableMimetypeCommandService.Remove(executableMimetype);
        }
    }
}
