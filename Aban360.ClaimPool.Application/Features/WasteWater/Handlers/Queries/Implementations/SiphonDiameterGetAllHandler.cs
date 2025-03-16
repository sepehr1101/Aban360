using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Implementations
{
    internal sealed class SiphonDiameterGetAllHandler : ISiphonDiameterGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonDiameterQueryService _queryService;
        public SiphonDiameterGetAllHandler(
            IMapper mapper,
            ISiphonDiameterQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ICollection<SiphonDiameterGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<SiphonDiameter> siphonDiameter = await _queryService.Get();
            return _mapper.Map<ICollection<SiphonDiameterGetDto>>(siphonDiameter);
        }
    }
}
