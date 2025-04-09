using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands;
using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Commands.Create.Implementations
{
    internal sealed class ExecutableMimetypeCreateHandler : IExecutableMimetypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IExecutableMimetypeCommandService _executableMimetypeCommandService;
        public ExecutableMimetypeCreateHandler(
            IMapper mapper,
            IExecutableMimetypeCommandService executableMimetypeCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _executableMimetypeCommandService = executableMimetypeCommandService;
            _executableMimetypeCommandService.NotNull(nameof(_executableMimetypeCommandService));
        }

        public async Task Handle(ExecutableMimetypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var ExecutableMimetype = _mapper.Map<ExecutableMimetype>(createDto);
            await _executableMimetypeCommandService.Add(ExecutableMimetype);
        }
    }
}
