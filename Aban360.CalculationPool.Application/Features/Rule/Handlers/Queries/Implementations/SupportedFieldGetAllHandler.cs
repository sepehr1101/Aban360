using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Implementations
{
    internal sealed class SupportedFieldGetAllHandler : ISupportedFieldGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ISupportedFieldQueryService _supportedFieldQueryService;
        public SupportedFieldGetAllHandler(
            IMapper mapper,
            ISupportedFieldQueryService supportedFieldQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _supportedFieldQueryService = supportedFieldQueryService;
            _supportedFieldQueryService.NotNull(nameof(_supportedFieldQueryService));
        }

        public async Task<ICollection<SupportedFieldGetDto>> Handle(CancellationToken cancellationToken)
        {
            var supportedField = await _supportedFieldQueryService.Get();
            return _mapper.Map<ICollection<SupportedFieldGetDto>>(supportedField);
        }
    }
}
