using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Implementations
{
    internal sealed class WaterMeterTagDefinitionUpdateHandler : IWaterMeterTagDefinitionUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterTagDefinitionQueryService _queryService;
        private readonly IValidator<WaterMeterTagDefinitionUpdateDto> _validator;
        public WaterMeterTagDefinitionUpdateHandler(
            IMapper mapper,
            IWaterMeterTagDefinitionQueryService queryService,
            IValidator<WaterMeterTagDefinitionUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterMeterTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            WaterMeterTagDefinition waterMeterTagDefinition = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, waterMeterTagDefinition);
        }
    }
}
