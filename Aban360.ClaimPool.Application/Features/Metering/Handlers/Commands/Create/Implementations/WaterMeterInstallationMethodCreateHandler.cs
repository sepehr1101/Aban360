using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    internal sealed class WaterMeterInstallationMethodCreateHandler : IWaterMeterInstallationMethodCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterInstallationMethodCommandService _waterMeterInstallationMethodCommandService;
        private readonly IValidator<WaterMeterInstallationMethodCreateDto> _validator;

        public WaterMeterInstallationMethodCreateHandler(
            IMapper mapper,
            IWaterMeterInstallationMethodCommandService waterMeterInstallationMethodCommandService,
            IValidator<WaterMeterInstallationMethodCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterMeterInstallationMethodCommandService = waterMeterInstallationMethodCommandService;
            _waterMeterInstallationMethodCommandService.NotNull(nameof(_waterMeterInstallationMethodCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterMeterInstallationMethodCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var waterMeterInstallationMethod = _mapper.Map<WaterMeterInstallationMethod>(createDto);
            await _waterMeterInstallationMethodCommandService.Add(waterMeterInstallationMethod);
        }
    }
}
