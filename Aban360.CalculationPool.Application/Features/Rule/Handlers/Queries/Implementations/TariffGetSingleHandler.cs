using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    internal sealed class TariffGetSingleHandler : ITariffGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffQueryService _tariffQueryService;
        public TariffGetSingleHandler(
            IMapper mapper,
            ITariffQueryService tariffQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffQueryService = tariffQueryService;
            _tariffQueryService.NotNull(nameof(tariffQueryService));
        }

        public async Task<TariffGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            Tariff tariff = await _tariffQueryService.Get(id);
            return _mapper.Map<TariffGetDto>(tariff);
        }
    }
}
