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
    internal sealed class MeterUseTypeCreateHandler : IMeterUseTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterUseTypeCommandService _meterUseTypeCommandService;
        private readonly IValidator<MeterUseTypeCreateDto> _validator;

        public MeterUseTypeCreateHandler(
            IMapper mapper,
            IMeterUseTypeCommandService meterUseTypeCommandService,
            IValidator<MeterUseTypeCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterUseTypeCommandService = meterUseTypeCommandService;
            _meterUseTypeCommandService.NotNull(nameof(meterUseTypeCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MeterUseTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            MeterUseType meterUseType = _mapper.Map<MeterUseType>(createDto);
            await _meterUseTypeCommandService.Add(meterUseType);
        }
    }
}
