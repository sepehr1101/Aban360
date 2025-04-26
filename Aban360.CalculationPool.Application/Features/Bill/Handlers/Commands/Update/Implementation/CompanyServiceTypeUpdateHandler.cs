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
    internal sealed class CompanyServiceTypeUpdateHandler : ICompanyServiceTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceTypeQueryService _companyServiceTypeQueryService;
        private readonly IValidator<CompanyServiceTypeUpdateDto> _validator;
        public CompanyServiceTypeUpdateHandler(
            IMapper mapper,
            ICompanyServiceTypeQueryService companyServiceTypeQueryService,
            IValidator<CompanyServiceTypeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceTypeQueryService = companyServiceTypeQueryService;
            _companyServiceTypeQueryService.NotNull(nameof(companyServiceTypeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(CompanyServiceTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            CompanyServiceType companyServiceType = await _companyServiceTypeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, companyServiceType);
        }
    }
}
