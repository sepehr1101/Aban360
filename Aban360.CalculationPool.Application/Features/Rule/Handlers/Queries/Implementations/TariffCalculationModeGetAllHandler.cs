using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    internal sealed class TariffCalculationModeGetAllHandler : ITariffCalculationModeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffCalculationModeQueryService _tariffCalculationModeQueryService;
        public TariffCalculationModeGetAllHandler(
            IMapper mapper,
            ITariffCalculationModeQueryService tariffCalculationModeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffCalculationModeQueryService = tariffCalculationModeQueryService;
            _tariffCalculationModeQueryService.NotNull(nameof(tariffCalculationModeQueryService));
        }

        public async Task<ICollection<TariffCalculationModeGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<TariffCalculationMode> tariffCalculationMode = await _tariffCalculationModeQueryService.Get();
            return _mapper.Map<ICollection<TariffCalculationModeGetDto>>(tariffCalculationMode);
        }
    }
}
