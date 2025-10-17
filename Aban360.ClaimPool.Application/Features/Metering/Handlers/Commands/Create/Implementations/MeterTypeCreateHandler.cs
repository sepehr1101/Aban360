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
    internal sealed class MeterTypeCreateHandler : IMeterTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterTypeCommandService _meterTypeCommandService;
        private readonly IValidator<MeterTypeCreateDto> _validator;

        public MeterTypeCreateHandler(
            IMapper mapper,
            IMeterTypeCommandService meterTypeCommandService,
            IValidator<MeterTypeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterTypeCommandService = meterTypeCommandService;
            _meterTypeCommandService.NotNull(nameof(meterTypeCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MeterTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            MeterType meterType = _mapper.Map<MeterType>(createDto);
            await _meterTypeCommandService.Add(meterType);
        }
    }
}
