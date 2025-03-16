using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Implementation
{
    internal sealed class CompanyServiceOfferingDeleteHandler : ICompanyServiceOfferingDeleteHandler
    {
        private readonly ICompanyServiceOfferingCommandService _companyServiceOfferingCommandService;
        private readonly ICompanyServiceOfferingQueryService _companyServiceOfferingQueryService;
        public CompanyServiceOfferingDeleteHandler(
            ICompanyServiceOfferingCommandService companyServiceOfferingCommandService,
            ICompanyServiceOfferingQueryService companyServiceOfferingQueryService)
        {
            _companyServiceOfferingCommandService = companyServiceOfferingCommandService;
            _companyServiceOfferingCommandService.NotNull(nameof(companyServiceOfferingCommandService));

            _companyServiceOfferingQueryService = companyServiceOfferingQueryService;
            _companyServiceOfferingQueryService.NotNull(nameof(companyServiceOfferingQueryService));
        }

        public async Task Handle(CompanyServiceOfferingDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            CompanyServiceOffering companyServiceOffering = await _companyServiceOfferingQueryService.Get(deleteDto.Id);
            await _companyServiceOfferingCommandService.Remove(companyServiceOffering);
        }
    }
}
