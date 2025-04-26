using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Implementation
{
    internal sealed class CompanyServiceOfferingUpdateHandler : ICompanyServiceOfferingUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceOfferingQueryService _companyServiceOfferingQueryService;
        private readonly IValidator<CompanyServiceOfferingUpdateDto> _validator;
        public CompanyServiceOfferingUpdateHandler(
            IMapper mapper,
            ICompanyServiceOfferingQueryService companyServiceOfferingQueryService,
            IValidator<CompanyServiceOfferingUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceOfferingQueryService = companyServiceOfferingQueryService;
            _companyServiceOfferingQueryService.NotNull(nameof(companyServiceOfferingQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(CompanyServiceOfferingUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            CompanyServiceOffering companyServiceOffering = await _companyServiceOfferingQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, companyServiceOffering);
        }
    }
}
