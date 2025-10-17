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
    internal sealed class MeterProducerUpdateHandler : IMeterProducerUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterProducerQueryService _meterProducerQueryService;
        private readonly IValidator<MeterProducerUpdateDto> _validator;
        public MeterProducerUpdateHandler(
            IMapper mapper,
            IMeterProducerQueryService meterProducerQueryService,
            IValidator<MeterProducerUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterProducerQueryService = meterProducerQueryService;
            _meterProducerQueryService.NotNull(nameof(meterProducerQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MeterProducerUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            MeterProducer meterProducer = await _meterProducerQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, meterProducer);
        }
    }

}
