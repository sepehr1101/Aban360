using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Implementation
{
    internal sealed class CompanyServiceTypeCreateHandler : ICompanyServiceTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceTypeCommandService _companyServiceTypeCommandService;
        public CompanyServiceTypeCreateHandler(
            IMapper mapper,
            ICompanyServiceTypeCommandService companyServiceTypeCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceTypeCommandService = companyServiceTypeCommandService;
            _companyServiceTypeCommandService.NotNull(nameof(companyServiceTypeCommandService));
        }

        public async Task Handle(CompanyServiceTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var companyServiceType = _mapper.Map<CompanyServiceType>(createDto);
            if (companyServiceType == null)
            {
                throw new InvalidDataException();
            }
            await _companyServiceTypeCommandService.Add(companyServiceType);
        }
    }
}
