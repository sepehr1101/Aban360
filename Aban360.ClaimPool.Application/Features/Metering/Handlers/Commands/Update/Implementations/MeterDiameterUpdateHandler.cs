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
    internal sealed class MeterDiameterUpdateHandler : IMeterDiameterUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterDiameterQueryService _meterDiameterQueryService;
        private readonly IValidator<MeterDiameterUpdateDto> _validator;

        public MeterDiameterUpdateHandler(
            IMapper mapper,
            IMeterDiameterQueryService meterDiameterQueryService,
            IValidator<MeterDiameterUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterDiameterQueryService = meterDiameterQueryService;
            _meterDiameterQueryService.NotNull(nameof(meterDiameterQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MeterDiameterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            MeterDiameter meterDiameter = await _meterDiameterQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, meterDiameter);
        }
    }
}

