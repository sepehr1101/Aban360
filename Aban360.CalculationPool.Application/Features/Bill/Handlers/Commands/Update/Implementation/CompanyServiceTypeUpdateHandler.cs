using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    public class CompanyServiceTypeUpdateHandler : ICompanyServiceTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceTypeQueryService _companyServiceTypeQueryService;
        public CompanyServiceTypeUpdateHandler(
            IMapper mapper,
            ICompanyServiceTypeQueryService companyServiceTypeQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceTypeQueryService = companyServiceTypeQueryService;
            _companyServiceTypeQueryService.NotNull(nameof(companyServiceTypeQueryService));
        }

        public async Task Handle(CompanyServiceTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var companyServiceType = await _companyServiceTypeQueryService.Get(updateDto.Id);
            if (companyServiceType == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, companyServiceType);
        }
    }
}
