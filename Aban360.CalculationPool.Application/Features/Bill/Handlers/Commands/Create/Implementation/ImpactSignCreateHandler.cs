using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Implementation
{
    internal sealed class ImpactSignCreateHandler : IImpactSignCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IImpactSignCommandService _impactSignCommandService;
        private readonly IValidator<ImpactSignCreateDto> _validator;
        public ImpactSignCreateHandler(
            IMapper mapper,
            IImpactSignCommandService impactSignCommandService,
            IValidator<ImpactSignCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _impactSignCommandService = impactSignCommandService;
            _impactSignCommandService.NotNull(nameof(_impactSignCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(ImpactSignCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            ImpactSign impactSign = _mapper.Map<ImpactSign>(createDto);
            await _impactSignCommandService.Add(impactSign);
        }
    }
}
