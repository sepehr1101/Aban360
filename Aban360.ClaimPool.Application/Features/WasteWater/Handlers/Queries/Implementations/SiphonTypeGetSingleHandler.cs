using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    internal sealed class SiphonTypeGetSingleHandler : ISiphonTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonTypeQueryService _queryService;
        public SiphonTypeGetSingleHandler(
            IMapper mapper,
            ISiphonTypeQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<SiphonTypeGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            SiphonType siphonType = await _queryService.Get(id);
            return _mapper.Map<SiphonTypeGetDto>(siphonType);
        }
    }
}
