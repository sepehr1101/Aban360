using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class CompanyServiceGetAllHandler : ICompanyServiceGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceQueryService _companyServiceQueryService;
        public CompanyServiceGetAllHandler(
            IMapper mapper,
            ICompanyServiceQueryService companyServiceQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceQueryService = companyServiceQueryService;
            _companyServiceQueryService.NotNull(nameof(companyServiceQueryService));
        }

        public async Task<ICollection<CompanyServiceGetDto>> Handle(CancellationToken cancellationToken)
        {
            var companyService = await _companyServiceQueryService.Get();
            if (companyService == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<ICollection<CompanyServiceGetDto>>(companyService);
        }
    }
}
