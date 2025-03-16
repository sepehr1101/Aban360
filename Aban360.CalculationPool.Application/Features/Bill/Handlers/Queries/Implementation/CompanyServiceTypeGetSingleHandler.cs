using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class CompanyServiceTypeGetSingleHandler : ICompanyServiceTypeGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceTypeQueryService _companyServiceTypeQueryService;
        public CompanyServiceTypeGetSingleHandler(
            IMapper mapper,
            ICompanyServiceTypeQueryService companyServiceTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceTypeQueryService = companyServiceTypeQueryService;
            _companyServiceTypeQueryService.NotNull(nameof(companyServiceTypeQueryService));
        }

        public async Task<CompanyServiceTypeGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            CompanyServiceType companyServiceType = await _companyServiceTypeQueryService.Get(id);
            return _mapper.Map<CompanyServiceTypeGetDto>(companyServiceType);
        }
    }
}
