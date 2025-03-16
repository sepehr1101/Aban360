using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    internal sealed class TariffGetByOfferingIdHandler : ITariffGetByOfferingIdHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffQueryService _tariffQueryService;
        public TariffGetByOfferingIdHandler(
            IMapper mapper,
            ITariffQueryService tariffQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffQueryService = tariffQueryService;
            _tariffQueryService.NotNull(nameof(tariffQueryService));
        }

        public async Task<ICollection<TariffGetDto>> Handle(short id, CancellationToken cancellationToken)
        {
            ICollection<Tariff> tariff = await _tariffQueryService.GetByOfferingId(id);
            return _mapper.Map<ICollection<TariffGetDto>>(tariff);
        }
    }
}
