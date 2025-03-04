using Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Implementation
{
    public class CompanyServiceTypeDeleteHandler : ICompanyServiceTypeDeleteHandler
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
            var companyServiceType = await _companyServiceTypeQueryService.Get(deleteDto.Id);
            if (companyServiceType == null)
            {
                throw new InvalidDataException();
            }
            await _companyServiceTypeCommandService.Remove(companyServiceType);
        }
    }
}
