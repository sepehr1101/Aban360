using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal class CompanyServiceCreateHandler : ICompanyServiceCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceCommandService _companyServiceCommandService;
        private readonly IValidator<CompanyServiceCreateDto> _validator;
        public CompanyServiceCreateHandler(
            IMapper mapper,
            ICompanyServiceCommandService companyServiceCommandService,
            IValidator<CompanyServiceCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceCommandService = companyServiceCommandService;
            _companyServiceCommandService.NotNull(nameof(companyServiceCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(CompanyServiceCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            CompanyService companyService = _mapper.Map<CompanyService>(createDto);
            await _companyServiceCommandService.Add(companyService);
        }
    }
}
