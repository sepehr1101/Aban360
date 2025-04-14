using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    internal sealed class SupportedOperatorGetSingleHandler : ISupportedOperatorGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ISupportedOperatorQueryService _supportedOperatorQueryService;
        public SupportedOperatorGetSingleHandler(
            IMapper mapper,
            ISupportedOperatorQueryService supportedOperatorQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _supportedOperatorQueryService = supportedOperatorQueryService;
            _supportedOperatorQueryService.NotNull(nameof(_supportedOperatorQueryService));
        }

        public async Task<SupportedOperatorGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var supportedOperator = await _supportedOperatorQueryService.Get(id);
            return _mapper.Map<SupportedOperatorGetDto>(supportedOperator);
        }
    }
}
