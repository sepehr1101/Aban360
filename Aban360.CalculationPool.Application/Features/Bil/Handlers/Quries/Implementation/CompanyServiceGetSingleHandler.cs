using Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Implementation
{
    public class CompanyServiceGetSingleHandler : ICompanyServiceGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceQueryService _companyServiceQueryService;
        public CompanyServiceGetSingleHandler(
            IMapper mapper,
            ICompanyServiceQueryService companyServiceQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceQueryService = companyServiceQueryService;
            _companyServiceQueryService.NotNull(nameof(companyServiceQueryService));
        }

        public async Task<CompanyServiceGetDto> Handle(short id, CancellationToken cancellationToken)
        {
            var companyService = await _companyServiceQueryService.Get(id);
            if (companyService == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<CompanyServiceGetDto>(companyService);
        }
    }
}
