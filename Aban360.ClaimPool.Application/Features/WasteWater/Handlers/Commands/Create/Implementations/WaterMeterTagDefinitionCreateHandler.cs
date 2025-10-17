using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Implementations
{
    internal sealed class WaterMeterTagDefinitionCreateHandler : IWaterMeterTagDefinitionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterTagDefinitionCommandService _commandService;
        private readonly IWaterMeterTagDefinitionQueryService _queryService;
        private readonly IValidator<WaterMeterTagDefinitionCreateDto> _validator;
        public WaterMeterTagDefinitionCreateHandler(
            IMapper mapper,
            IWaterMeterTagDefinitionCommandService commandService,
            IWaterMeterTagDefinitionQueryService queryService,
            IValidator<WaterMeterTagDefinitionCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterMeterTagDefinitionCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var waterMeterTagDefinition = _mapper.Map<WaterMeterTagDefinition>(createDto);
            await _commandService.Add(waterMeterTagDefinition);
        }
    }
}
