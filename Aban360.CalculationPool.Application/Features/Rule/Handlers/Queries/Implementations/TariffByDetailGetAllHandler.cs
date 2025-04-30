using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    internal sealed class TariffByDetailGetAllHandler : ITariffByDetailGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffByDetailQueryService _tariffByDetailQueryService;
        public TariffByDetailGetAllHandler(
            IMapper mapper,
            ITariffByDetailQueryService tariffByDetailQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _tariffByDetailQueryService = tariffByDetailQueryService;
            _tariffByDetailQueryService.NotNull(nameof(_tariffByDetailQueryService));
        }

        public async Task<ICollection<TariffByDetailGetDto>> Handle(CancellationToken cancellationToken)
        {
            var TariffByDetail = await _tariffByDetailQueryService.Get();
            return _mapper.Map<ICollection<TariffByDetailGetDto>>(TariffByDetail);
        }
    }
}
