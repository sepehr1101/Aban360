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
    internal sealed class MeterMaterialCreateHandler : IMeterMaterialCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterMaterialCommandService _meterMaterialCommandService;
        private readonly IValidator<MeterMaterialCreateDto> _validator;

        public MeterMaterialCreateHandler(
            IMapper mapper,
            IMeterMaterialCommandService meterMaterialCommandService,
            IValidator<MeterMaterialCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterMaterialCommandService = meterMaterialCommandService;
            _meterMaterialCommandService.NotNull(nameof(_meterMaterialCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MeterMaterialCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            MeterMaterial meterMaterial = _mapper.Map<MeterMaterial>(createDto);
            await _meterMaterialCommandService.Add(meterMaterial);
        }
    }
}
