using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class CompanyServiceGetByTypeIdHandler : ICompanyServiceGetByTypeIdHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceQueryService _companyServiceQueryService;
        public CompanyServiceGetByTypeIdHandler(
            IMapper mapper,
            ICompanyServiceQueryService companyServiceQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceQueryService = companyServiceQueryService;
            _companyServiceQueryService.NotNull(nameof(companyServiceQueryService));
        }

        public async Task<ICollection<NumericDictionary>> Handle(int typeId, CancellationToken cancellationToken)
        {
            return await _companyServiceQueryService.GetByTypeId(typeId);
        }
    }
}
