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
    internal sealed class MeterProducerCreateHandler : IMeterProducerCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterProducerCommandService _meterProducerCommandService;
        private readonly IValidator<MeterProducerCreateDto> _validator;

        public MeterProducerCreateHandler(
            IMapper mapper,
            IMeterProducerCommandService meterProducerCommandService,
            IValidator<MeterProducerCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterProducerCommandService = meterProducerCommandService;
            _meterProducerCommandService.NotNull(nameof(meterProducerCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MeterProducerCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            MeterProducer meterProducer = _mapper.Map<MeterProducer>(createDto);
            await _meterProducerCommandService.Add(meterProducer);
        }
    }

}
