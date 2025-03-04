using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Implementation
{
    public class CompanyServiceTypeGetAllHandler : ICompanyServiceTypeGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceTypeQueryService _companyServiceTypeQueryService;
        public CompanyServiceTypeGetAllHandler(
            IMapper mapper,
            ICompanyServiceTypeQueryService companyServiceTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceTypeQueryService = companyServiceTypeQueryService;
            _companyServiceTypeQueryService.NotNull(nameof(companyServiceTypeQueryService));
        }

        public async Task<ICollection<CompanyServiceTypeGetDto>> Handle(CancellationToken cancellationToken)
        {
            var companyServiceType = await _companyServiceTypeQueryService.Get();
            if (companyServiceType == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<CompanyServiceTypeGetDto>>(companyServiceType);
        }
    }
}
