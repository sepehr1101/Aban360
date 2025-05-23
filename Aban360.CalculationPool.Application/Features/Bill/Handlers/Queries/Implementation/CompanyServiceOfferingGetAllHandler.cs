﻿using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Implementation
{
    internal sealed class CompanyServiceOfferingGetAllHandler : ICompanyServiceOfferingGetAllHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceOfferingQueryService _companyServiceOfferingQueryService;
        public CompanyServiceOfferingGetAllHandler(
            IMapper mapper,
            ICompanyServiceOfferingQueryService companyServiceOfferingQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceOfferingQueryService = companyServiceOfferingQueryService;
            _companyServiceOfferingQueryService.NotNull(nameof(companyServiceOfferingQueryService));
        }

        public async Task<ICollection<CompanyServiceOfferingGetDto>> Handle(CancellationToken cancellationToken)
        {
            ICollection<CompanyServiceOffering> companyServiceOffering = await _companyServiceOfferingQueryService.Get();
            return _mapper.Map<ICollection<CompanyServiceOfferingGetDto>>(companyServiceOffering);
        }
    }
}
