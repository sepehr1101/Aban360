using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestWaterMeterSiphonCreateHandler : IRequestWaterMeterSiphonCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterSiphonCommandService _requestWaterMeterSiphonCommandService;
        private readonly IValidator<WaterMeterSiphonRequestCreateDto> _validator;

        public RequestWaterMeterSiphonCreateHandler(
            IMapper mapper,
            IRequestWaterMeterSiphonCommandService requestWaterMeterSiphonCommandService,
            IValidator<WaterMeterSiphonRequestCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterSiphonCommandService = requestWaterMeterSiphonCommandService;
            _requestWaterMeterSiphonCommandService.NotNull(nameof(_requestWaterMeterSiphonCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterMeterSiphonRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var requestWaterMeterSiphon = _mapper.Map<RequestWaterMeterSiphon>(createDto);
            await _requestWaterMeterSiphonCommandService.Add(requestWaterMeterSiphon);
        }
    }
}
