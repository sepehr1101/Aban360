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
    internal sealed class CompanyServiceUpdateHandler : ICompanyServiceUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceQueryService _companyServiceQueryService;
        private readonly IValidator<CompanyServiceUpdateDto> _validator;

        public CompanyServiceUpdateHandler(
            IMapper mapper,
            ICompanyServiceQueryService companyServiceQueryService,
            IValidator<CompanyServiceUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _companyServiceQueryService = companyServiceQueryService;
            _companyServiceQueryService.NotNull(nameof(companyServiceQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(CompanyServiceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            CompanyService companyService = await _companyServiceQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, companyService);
        }
    }
}
