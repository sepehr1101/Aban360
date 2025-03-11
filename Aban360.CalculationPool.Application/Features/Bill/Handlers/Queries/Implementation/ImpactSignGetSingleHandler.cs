using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    public class ImpactSignGetSingleHandler : IImpactSignGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IImpactSignQueryService _impactSignQueryService;
        public ImpactSignGetSingleHandler(
            IMapper mapper,
            IImpactSignQueryService impactSignQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _impactSignQueryService = impactSignQueryService;
            _impactSignQueryService.NotNull(nameof(_impactSignQueryService));
        }

        public async Task<ImpactSignGetDto> Handle(ImpactSignEnum id, CancellationToken cancellationToken)
        {
            var ImpactSign = await _impactSignQueryService.Get(id);
            return _mapper.Map<ImpactSignGetDto>(ImpactSign);
        }
    }
}
