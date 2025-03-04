using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Implementation
{
    internal sealed class CompanyServiceDeleteHandler : ICompanyServiceDeleteHandler
    {
        private readonly ICompanyServiceCommandService _companyServiceCommandService;
        private readonly ICompanyServiceQueryService _companyServiceQueryService;
        public CompanyServiceDeleteHandler(
            ICompanyServiceCommandService companyServiceCommandService,
            ICompanyServiceQueryService companyServiceQueryService)
        {
            _companyServiceCommandService = companyServiceCommandService;
            _companyServiceCommandService.NotNull(nameof(companyServiceCommandService));

            _companyServiceQueryService = companyServiceQueryService;
            _companyServiceQueryService.NotNull(nameof(companyServiceQueryService));
        }

        public async Task Handle(CompanyServiceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            var companyService = await _companyServiceQueryService.Get(deleteDto.Id);
            if (companyService == null)
            {
                throw new InvalidDataException();
            }
            await _companyServiceCommandService.Remove(companyService);
        }
    }
}
