using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    internal sealed class SiphonDiameterGetSingleHandler : ISiphonDiameterGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonDiameterQueryService _queryService;
        public SiphonDiameterGetSingleHandler(
            IMapper mapper,
            ISiphonDiameterQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<SiphonDiameterGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            SiphonDiameter siphonDiameter = await _queryService.Get(id);
            return _mapper.Map<SiphonDiameterGetDto>(siphonDiameter);
        }
    }
}
