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
    internal sealed class MeterUseTypeUpdateHandler : IMeterUseTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterUseTypeQueryService _meterUseTypeQueryService;
        private readonly IValidator<MeterUseTypeUpdateDto> _validator;
        public MeterUseTypeUpdateHandler(
            IMapper mapper,
            IMeterUseTypeQueryService meterUseTypeQueryService,
            IValidator<MeterUseTypeUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterUseTypeQueryService = meterUseTypeQueryService;
            _meterUseTypeQueryService.NotNull(nameof(meterUseTypeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MeterUseTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            MeterUseType meterUseType = await _meterUseTypeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, meterUseType);
        }
    }
}
