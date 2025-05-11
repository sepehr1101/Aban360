using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    internal sealed class TariffConstantDictionaryHandler : ITariffConstantDictionaryHandler
    {
        private readonly IMapper _mapper;
        private readonly ITariffConstantQueryService _tariffConstantQueryService;
        public TariffConstantDictionaryHandler(
            IMapper mapper,
            ITariffConstantQueryService tariffConstantQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _tariffConstantQueryService = tariffConstantQueryService;
            _tariffConstantQueryService.NotNull(nameof(tariffConstantQueryService));
        }

        public async Task<ICollection<StringDictionary>> Handle(CancellationToken cancellationToken)
        {
            ICollection<StringDictionary> tariffConstant = await _tariffConstantQueryService.GetDictionary();
            return tariffConstant;
        }
    }
}
