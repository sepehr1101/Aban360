using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class CompanyServiceOfferingGetSingleHandler : ICompanyServiceOfferingGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceOfferingQueryService _companyServiceOfferingQueryService;
        public CompanyServiceOfferingGetSingleHandler(
            IMapper mapper,
            ICompanyServiceOfferingQueryService companyServiceOfferingQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceOfferingQueryService = companyServiceOfferingQueryService;
            _companyServiceOfferingQueryService.NotNull(nameof(companyServiceOfferingQueryService));
        }

        public async Task<CompanyServiceOfferingGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var companyServiceOffering = await _companyServiceOfferingQueryService.Get(id);
            if (companyServiceOffering == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<CompanyServiceOfferingGetDto>(companyServiceOffering);
        }
    }
}
