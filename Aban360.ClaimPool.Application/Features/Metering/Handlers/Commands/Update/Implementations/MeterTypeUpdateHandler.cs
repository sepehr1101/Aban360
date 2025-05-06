using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Implementations
{
    internal sealed class MeterTypeUpdateHandler : IMeterTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterTypeQueryService _meterTypeQueryService;
        private readonly IValidator<MeterTypeUpdateDto> _validator;
        public MeterTypeUpdateHandler(IMapper mapper,
            IMeterTypeQueryService meterTypeQueryService,
            IValidator<MeterTypeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterTypeQueryService = meterTypeQueryService;
            _meterTypeQueryService.NotNull(nameof(meterTypeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MeterTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            MeterType meterType = await _meterTypeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, meterType);
        }
    }
}
