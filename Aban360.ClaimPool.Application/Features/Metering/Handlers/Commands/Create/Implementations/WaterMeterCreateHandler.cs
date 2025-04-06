using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    internal sealed class WaterMeterCreateHandler : IWaterMeterCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterCommandService _commandService;
        private readonly IValidator<WaterMeterCreateDto> _waterMeterValidator;
        public WaterMeterCreateHandler(
            IMapper mapper,
            IWaterMeterCommandService commandService,
            IValidator<WaterMeterCreateDto> waterMeterValidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _waterMeterValidator = waterMeterValidator;
            _waterMeterValidator.NotNull(nameof(waterMeterValidator));
        }

        public async Task Handle(WaterMeterCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult=await _waterMeterValidator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                string message=string.Join(",",validationResult.Errors.Select(x=>x.ErrorMessage));
                throw new BaseException(message);
            }

            WaterMeter waterMeter = _mapper.Map<WaterMeter>(createDto);
            waterMeter.ValidFrom=DateTime.Now;
            waterMeter.InsertLogInfo = "SampleLogInfo";
            waterMeter.Hash = "SampleHash";

            await _commandService.Add(waterMeter);
        }
    }
}