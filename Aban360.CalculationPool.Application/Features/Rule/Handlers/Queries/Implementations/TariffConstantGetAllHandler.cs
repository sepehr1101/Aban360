using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    internal sealed class TariffConstantGetAllHandler : ITariffConstantGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffConstantQueryService _tariffConstantQueryService;
        public TariffConstantGetAllHandler(
            IMapper mapper,
            ITariffConstantQueryService tariffConstantQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffConstantQueryService = tariffConstantQueryService;
            _tariffConstantQueryService.NotNull(nameof(tariffConstantQueryService));
        }

        public async Task<ICollection<TariffConstantGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<TariffConstant> tariffConstant = await _tariffConstantQueryService.Get();
            return _mapper.Map<ICollection<TariffConstantGetDto>>(tariffConstant);
        }
    }
}
