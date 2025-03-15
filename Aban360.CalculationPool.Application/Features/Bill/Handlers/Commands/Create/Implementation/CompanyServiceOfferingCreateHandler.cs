using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class CompanyServiceOfferingCreateHandler : ICompanyServiceOfferingCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceOfferingCommandService _companyServiceOfferingCommandService;
        public CompanyServiceOfferingCreateHandler(
            IMapper mapper,
            ICompanyServiceOfferingCommandService companyServiceOfferingCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceOfferingCommandService = companyServiceOfferingCommandService;
            _companyServiceOfferingCommandService.NotNull(nameof(companyServiceOfferingCommandService));
        }

        public async Task Handle(CompanyServiceOfferingCreateDto createDto, CancellationToken cancellationToken)
        {
            CompanyServiceOffering companyServiceOffering = _mapper.Map<CompanyServiceOffering>(createDto);
            await _companyServiceOfferingCommandService.Add(companyServiceOffering);
        }
    }
}
