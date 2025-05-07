using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    internal sealed class WaterMeterInstallationMethodUpdateHandler : IWaterMeterInstallationMethodUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IWaterMeterInstallationMethodQueryService _waterMeterInstallationMethodQueryService;
        private readonly IValidator<WaterMeterInstallationMethodUpdateDto> _validator;
        public WaterMeterInstallationMethodUpdateHandler(
            IMapper mapper,
            IWaterMeterInstallationMethodQueryService waterMeterInstallationMethodQueryService,
            IValidator<WaterMeterInstallationMethodUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _waterMeterInstallationMethodQueryService = waterMeterInstallationMethodQueryService;
            _waterMeterInstallationMethodQueryService.NotNull(nameof(_waterMeterInstallationMethodQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterMeterInstallationMethodUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var waterMeterInstallationMethod = await _waterMeterInstallationMethodQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, waterMeterInstallationMethod);
        }
    }
}
