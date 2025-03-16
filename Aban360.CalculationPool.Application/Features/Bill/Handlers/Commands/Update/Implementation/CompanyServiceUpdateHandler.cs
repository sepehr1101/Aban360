using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    internal sealed class CompanyServiceUpdateHandler : ICompanyServiceUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceQueryService _companyServiceQueryService;
        public CompanyServiceUpdateHandler(
            IMapper mapper,
            ICompanyServiceQueryService companyServiceQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceQueryService = companyServiceQueryService;
            _companyServiceQueryService.NotNull(nameof(companyServiceQueryService));
        }

        public async Task Handle(CompanyServiceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            CompanyService companyService = await _companyServiceQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, companyService);
        }
    }
}
