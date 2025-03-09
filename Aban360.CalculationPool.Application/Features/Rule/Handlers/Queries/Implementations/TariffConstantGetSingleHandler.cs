using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    public class TariffConstantGetSingleHandler : ITariffConstantGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffConstantQueryService _tariffConstantQueryService;
        public TariffConstantGetSingleHandler(
            IMapper mapper,
            ITariffConstantQueryService tariffConstantQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffConstantQueryService = tariffConstantQueryService;
            _tariffConstantQueryService.NotNull(nameof(tariffConstantQueryService));
        }

        public async Task<TariffConstantGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var tariffConstant = await _tariffConstantQueryService.Get(id);
            if (tariffConstant == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<TariffConstantGetDto>(tariffConstant);
        }
    }
}
