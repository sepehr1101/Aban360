using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    internal sealed class TariffByDetailGetSingleHandler : ITariffByDetailGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffByDetailQueryService _tariffByDetailQueryService;
        public TariffByDetailGetSingleHandler(
            IMapper mapper,
            ITariffByDetailQueryService tariffByDetailQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _tariffByDetailQueryService = tariffByDetailQueryService;
            _tariffByDetailQueryService.NotNull(nameof(_tariffByDetailQueryService));
        }

        public async Task<TariffByDetailGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            var tariffByDetail = await _tariffByDetailQueryService.Get(id);
            return _mapper.Map<TariffByDetailGetDto>(tariffByDetail);
        }
    }
}
