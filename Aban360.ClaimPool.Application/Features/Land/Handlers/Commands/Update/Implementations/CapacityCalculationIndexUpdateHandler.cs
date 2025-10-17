using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class CapacityCalculationIndexUpdateHandler : ICapacityCalculationIndexUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ICapacityCalculationIndexQueryService _capacityCalculationIndexQueryService;
        private readonly IValidator<CapacityCalculationIndexUpdateDto> _validator;

        public CapacityCalculationIndexUpdateHandler(
            IMapper mapper,
            ICapacityCalculationIndexQueryService capacityCalculationIndexQueryService,
            IValidator<CapacityCalculationIndexUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _capacityCalculationIndexQueryService = capacityCalculationIndexQueryService;
            _capacityCalculationIndexQueryService.NotNull(nameof(_capacityCalculationIndexQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(CapacityCalculationIndexUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }


            var capacityCalculationIndex = await _capacityCalculationIndexQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, capacityCalculationIndex);
        }
    }
}
