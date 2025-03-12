using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    public class ImpactSignUpdateHandler : IImpactSignUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IImpactSignQueryService _impactSignQueryService;
        public ImpactSignUpdateHandler(
            IMapper mapper,
            IImpactSignQueryService impactSignQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _impactSignQueryService = impactSignQueryService;
            _impactSignQueryService.NotNull(nameof(_impactSignQueryService));
        }

        public async Task Handle(ImpactSignUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var impactSign = await _impactSignQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto,impactSign);
        }
    }
}
