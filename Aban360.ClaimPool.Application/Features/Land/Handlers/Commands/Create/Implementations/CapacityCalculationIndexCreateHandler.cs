using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class CapacityCalculationIndexCreateHandler : ICapacityCalculationIndexCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICapacityCalculationIndexCommandService _capacityCalculationIndexCommandService;
        private readonly IValidator<CapacityCalculationIndexCreateDto> _validator;

        public CapacityCalculationIndexCreateHandler(
            IMapper mapper,
            ICapacityCalculationIndexCommandService capacityCalculationIndexCommandService,
            IValidator<CapacityCalculationIndexCreateDto> validator )
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _capacityCalculationIndexCommandService = capacityCalculationIndexCommandService;
            _capacityCalculationIndexCommandService.NotNull(nameof(_capacityCalculationIndexCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(CapacityCalculationIndexCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var capacityCalculationIndex = _mapper.Map<CapacityCalculationIndex>(createDto);
            await _capacityCalculationIndexCommandService.Add(capacityCalculationIndex);
        }
    }
}
