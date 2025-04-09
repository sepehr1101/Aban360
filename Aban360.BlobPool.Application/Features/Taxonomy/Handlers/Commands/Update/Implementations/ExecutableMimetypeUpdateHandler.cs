using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Update.Implementations
{
    internal sealed class ExecutableMimetypeUpdateHandler : IExecutableMimetypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IExecutableMimetypeQueryService _executableMimetypeQueryService;
        public ExecutableMimetypeUpdateHandler(
            IMapper mapper,
            IExecutableMimetypeQueryService executableMimetypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _executableMimetypeQueryService = executableMimetypeQueryService;
            _executableMimetypeQueryService.NotNull(nameof(_executableMimetypeQueryService));
        }

        public async Task Handle(ExecutableMimetypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var executableMimetype = await _executableMimetypeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, executableMimetype);
        }
    }
}
