using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal class CompanyServiceCreateHandler : ICompanyServiceCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceCommandService _companyServiceCommandService;
        public CompanyServiceCreateHandler(
            IMapper mapper,
            ICompanyServiceCommandService companyServiceCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceCommandService = companyServiceCommandService;
            _companyServiceCommandService.NotNull(nameof(companyServiceCommandService));
        }

        public async Task Handle(CompanyServiceCreateDto createDto, CancellationToken cancellationToken)
        {
            CompanyService companyService = _mapper.Map<CompanyService>(createDto);
            await _companyServiceCommandService.Add(companyService);
        }
    }
}
