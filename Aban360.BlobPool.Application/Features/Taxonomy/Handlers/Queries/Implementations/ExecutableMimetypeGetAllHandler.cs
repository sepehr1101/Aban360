using Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries;
using Aban360.BlobPool.Persistence.Features.Taxonomy.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.BlobPool.Application.Features.Taxonomy.Handlers.Queries.Implementations
{
    internal sealed class ExecutableMimetypeGetAllHandler : IExecutableMimetypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IExecutableMimetypeQueryService _executableMimetypeQueryService;
        public ExecutableMimetypeGetAllHandler(
            IMapper mapper,
            IExecutableMimetypeQueryService executableMimetypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _executableMimetypeQueryService = executableMimetypeQueryService;
            _executableMimetypeQueryService.NotNull(nameof(_executableMimetypeQueryService));
        }

        public async Task<ICollection<ExecutableMimetypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            var executableMimetype = await _executableMimetypeQueryService.Get();
            return _mapper.Map<ICollection<ExecutableMimetypeGetDto>>(executableMimetype);
        }
    }
}
