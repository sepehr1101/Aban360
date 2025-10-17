using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestWaterMeterSiphonUpdateHandler : IRequestWaterMeterSiphonUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterSiphonQueryService _requestWaterMeterSiphonQueryService;
        private readonly IValidator<WaterMeterSiphonRequestUpdateDto> _validator;

        public RequestWaterMeterSiphonUpdateHandler(
            IMapper mapper,
            IRequestWaterMeterSiphonQueryService requestWaterMeterSiphonQueryService,
            IValidator<WaterMeterSiphonRequestUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterSiphonQueryService = requestWaterMeterSiphonQueryService;
            _requestWaterMeterSiphonQueryService.NotNull(nameof(_requestWaterMeterSiphonQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterMeterSiphonRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var requestWaterMeterSiphon = await _requestWaterMeterSiphonQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestWaterMeterSiphon);
        }
    }
}
