using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    public class ImpactSignGetAllHandler : IImpactSignGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly IImpactSignQueryService _impactSignQueryService;
        public ImpactSignGetAllHandler(
            IMapper mapper,
            IImpactSignQueryService impactSignQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _impactSignQueryService = impactSignQueryService;
            _impactSignQueryService.NotNull(nameof(_impactSignQueryService));
        }

        public async Task<ICollection<ImpactSignGetDto>> Handle(CancellationToken cancellationToken)
        {
            var ImpactSign = await _impactSignQueryService.Get();
            return _mapper.Map<ICollection<ImpactSignGetDto>>(ImpactSign);
        }
    }
}
