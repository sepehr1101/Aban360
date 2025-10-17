using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Implementations
{
    internal sealed class WaterMeterTagCreateHandler : IWaterMeterTagCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterTagCommandService _waterMeterTagCommandService;
        private readonly IValidator<WaterMeterTagCreateDto> _validator;
        public WaterMeterTagCreateHandler(
            IMapper mapper,
            IWaterMeterTagCommandService waterMeterTagCommandService,
            IValidator<WaterMeterTagCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _waterMeterTagCommandService = waterMeterTagCommandService;
            _waterMeterTagCommandService.NotNull(nameof(waterMeterTagCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterMeterTagCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            WaterMeterTag waterMeterTag = _mapper.Map<WaterMeterTag>(createDto);
            await _waterMeterTagCommandService.Add(waterMeterTag);
        }
    }
}
