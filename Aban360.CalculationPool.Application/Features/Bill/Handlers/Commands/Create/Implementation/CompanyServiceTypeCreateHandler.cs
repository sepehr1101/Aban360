using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Implementation
{
    internal sealed class CompanyServiceTypeCreateHandler : ICompanyServiceTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceTypeCommandService _companyServiceTypeCommandService;
        private readonly IValidator<CompanyServiceTypeCreateDto> _validator;
        public CompanyServiceTypeCreateHandler(
            IMapper mapper,
            ICompanyServiceTypeCommandService companyServiceTypeCommandService,
             IValidator<CompanyServiceTypeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceTypeCommandService = companyServiceTypeCommandService;
            _companyServiceTypeCommandService.NotNull(nameof(companyServiceTypeCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(CompanyServiceTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            CompanyServiceType companyServiceType = _mapper.Map<CompanyServiceType>(createDto);
            await _companyServiceTypeCommandService.Add(companyServiceType);
        }
    }
}
