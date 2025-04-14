using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    internal sealed class SupportedFieldGetSingleHandler : ISupportedFieldGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ISupportedFieldQueryService _supportedFieldQueryService;
        public SupportedFieldGetSingleHandler(
            IMapper mapper,
            ISupportedFieldQueryService supportedFieldQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _supportedFieldQueryService = supportedFieldQueryService;
            _supportedFieldQueryService.NotNull(nameof(_supportedFieldQueryService));
        }

        public async Task<SupportedFieldGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var supportedField = await _supportedFieldQueryService.Get(id);
            return _mapper.Map<SupportedFieldGetDto>(supportedField);
        }
    }
}
