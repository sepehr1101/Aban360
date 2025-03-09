using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    public class ImpactSignCreateHandler : IImpactSignCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IImpactSignCommandService _impactSignCommandService;
        public ImpactSignCreateHandler(
            IMapper mapper,
            IImpactSignCommandService impactSignCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _impactSignCommandService = impactSignCommandService;
            _impactSignCommandService.NotNull(nameof(_impactSignCommandService));
        }

        public async Task Handle(ImpactSignCreateDto createDto, CancellationToken cancellationToken)
        {
            var impactSign = _mapper.Map<ImpactSign>(createDto);
            await _impactSignCommandService.Add(impactSign);
        }
    }
}
