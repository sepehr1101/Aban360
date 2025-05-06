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
    internal sealed class MeterDiameterCreateHandler : IMeterDiameterCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterDiameterCommandService _meterDiameterCommandService;
        private readonly IValidator<MeterDiameterCreateDto> _validator;

        public MeterDiameterCreateHandler(
            IMapper mapper,
            IMeterDiameterCommandService meterDiameterCommandService,
            IValidator<MeterDiameterCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterDiameterCommandService = meterDiameterCommandService;
            _meterDiameterCommandService.NotNull(nameof(meterDiameterCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MeterDiameterCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            MeterDiameter meterDiameter = _mapper.Map<MeterDiameter>(createDto);
            await _meterDiameterCommandService.Add(meterDiameter);
        }
    }
}

