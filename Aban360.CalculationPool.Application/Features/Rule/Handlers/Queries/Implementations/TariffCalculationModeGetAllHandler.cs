using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    public class TariffCalculationModeGetAllHandler : ITariffCalculationModeGetAllHandler
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
            var tariffCalculationMode = await _tariffCalculationModeQueryService.Get();
            if (tariffCalculationMode == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<TariffCalculationModeGetDto>>(tariffCalculationMode);
        }
    }
}
