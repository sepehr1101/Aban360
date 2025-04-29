using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class CompanyServiceOfferingCreateHandler : ICompanyServiceOfferingCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceOfferingCommandService _companyServiceOfferingCommandService;
        private readonly IValidator<CompanyServiceOfferingCreateDto> _validator;

        public CompanyServiceOfferingCreateHandler(
            IMapper mapper,
            ICompanyServiceOfferingCommandService companyServiceOfferingCommandService,
            IValidator<CompanyServiceOfferingCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceOfferingCommandService = companyServiceOfferingCommandService;
            _companyServiceOfferingCommandService.NotNull(nameof(companyServiceOfferingCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(CompanyServiceOfferingCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            CompanyServiceOffering companyServiceOffering = _mapper.Map<CompanyServiceOffering>(createDto);
            await _companyServiceOfferingCommandService.Add(companyServiceOffering);
        }
    }
}
