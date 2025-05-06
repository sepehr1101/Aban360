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
    internal sealed class MeterMaterialUpdateHandler : IMeterMaterialUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterMaterialQueryService _meterMaterialQueryService;
        private readonly IValidator<MeterMaterialUpdateDto> _validator;
        public MeterMaterialUpdateHandler(
            IMapper mapper,
            IMeterMaterialQueryService meterMaterialQueryService,
            IValidator<MeterMaterialUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterMaterialQueryService = meterMaterialQueryService;
            _meterMaterialQueryService.NotNull(nameof(meterMaterialQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(MeterMaterialUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            MeterMaterial meterMaterial = await _meterMaterialQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, meterMaterial);
        }
    }
}
