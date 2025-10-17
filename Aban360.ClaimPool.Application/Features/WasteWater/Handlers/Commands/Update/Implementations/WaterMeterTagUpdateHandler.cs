using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Implementations
{
    internal sealed class WaterMeterTagUpdateHandler : IWaterMeterTagUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterTagQueryService _waterMeterTagQueryService;
        private readonly IValidator<WaterMeterTagUpdateDto> _validator;
        public WaterMeterTagUpdateHandler(
            IMapper mapper,
            IWaterMeterTagQueryService waterMeterTagQueryService,
            IValidator<WaterMeterTagUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _waterMeterTagQueryService = waterMeterTagQueryService;
            _waterMeterTagQueryService.NotNull(nameof(waterMeterTagQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterMeterTagUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var waterMeterTag = await _waterMeterTagQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, waterMeterTag);
        }
    }
}
