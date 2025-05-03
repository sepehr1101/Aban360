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
    internal sealed class RequestWaterMeterTagCreateHandler : IRequestWaterMeterTagCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterTagCommandService _requestWaterMeterTagCommandService;
        private readonly IValidator<WaterMeterTagRequestCreateDto> _validator;

        public RequestWaterMeterTagCreateHandler(
            IMapper mapper,
            IRequestWaterMeterTagCommandService requestWaterMeterTagCommandService,
            IValidator<WaterMeterTagRequestCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterTagCommandService = requestWaterMeterTagCommandService;
            _requestWaterMeterTagCommandService.NotNull(nameof(_requestWaterMeterTagCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterMeterTagRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestWaterMeterTag = _mapper.Map<RequestWaterMeterTag>(createDto);
            await _requestWaterMeterTagCommandService.Add(requestWaterMeterTag);
        }
    }
}
