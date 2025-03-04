using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    public class CompanyServiceUpdateHandler : ICompanyServiceUpdateHandler
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
            var companyService = await _companyServiceQueryService.Get(updateDto.Id);
            if (companyService == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, companyService);
        }
    }
}
