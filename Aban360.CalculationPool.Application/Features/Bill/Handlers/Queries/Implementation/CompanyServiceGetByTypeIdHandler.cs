using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class CompanyServiceGetByTypeIdHandler : ICompanyServiceGetByTypeIdHandler
    {
        private readonly IMapper _mapper;
        private readonly IT9QueryService _t9queryService;
        public CompanyServiceGetByTypeIdHandler(
            IMapper mapper,
            IT9QueryService t9queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _t9queryService = t9queryService;
            _t9queryService.NotNull(nameof(t9queryService));
        }

        public async Task<IEnumerable<NumericDictionary>> Handle(int typeId, CancellationToken cancellationToken)
        {
            return await _t9queryService.GetByTypeId(typeId);
        }
    }
}
