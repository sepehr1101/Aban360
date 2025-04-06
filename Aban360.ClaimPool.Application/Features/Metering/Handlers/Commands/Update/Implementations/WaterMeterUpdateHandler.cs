using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    internal sealed class WaterMeterUpdateHandler : IWaterMeterUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterQueryService _queryService;
        private readonly IValidator<WaterMeterUpdateDto> _waterMeterValidator;
        public WaterMeterUpdateHandler(
            IMapper mapper,
            IWaterMeterQueryService queryService,
            IValidator<WaterMeterUpdateDto> waterMeterVlidator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _waterMeterValidator = waterMeterVlidator;
            _waterMeterValidator.NotNull(nameof(waterMeterVlidator));
        }

        public async Task Handle(WaterMeterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _waterMeterValidator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                string message = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }

            WaterMeter waterMeter = await _queryService.Get(updateDto.Id);
            waterMeter.ValidFrom = DateTime.Now;
            waterMeter.InsertLogInfo = "SampleLogInfo";
            waterMeter.Hash = "SampleHash";

            _mapper.Map(updateDto, waterMeter);
        }
    }
}
