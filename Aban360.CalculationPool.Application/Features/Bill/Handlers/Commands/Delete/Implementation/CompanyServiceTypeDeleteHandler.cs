using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Implementation
{
    internal sealed class CompanyServiceTypeDeleteHandler : ICompanyServiceTypeDeleteHandler
    {
        private readonly ICompanyServiceTypeCommandService _companyServiceTypeCommandService;
        private readonly ICompanyServiceTypeQueryService _companyServiceTypeQueryService;
        public CompanyServiceTypeDeleteHandler(
            ICompanyServiceTypeCommandService companyServiceTypeCommandService,
            ICompanyServiceTypeQueryService companyServiceTypeQueryService)
        {
            _companyServiceTypeCommandService = companyServiceTypeCommandService;
            _companyServiceTypeCommandService.NotNull(nameof(companyServiceTypeCommandService));

            _companyServiceTypeQueryService = companyServiceTypeQueryService;
            _companyServiceTypeQueryService.NotNull(nameof(companyServiceTypeQueryService));
        }

        public async Task Handle(CompanyServiceTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            CompanyServiceType companyServiceType = await _companyServiceTypeQueryService.Get(deleteDto.Id);
            await _companyServiceTypeCommandService.Remove(companyServiceType);
        }
    }
}
