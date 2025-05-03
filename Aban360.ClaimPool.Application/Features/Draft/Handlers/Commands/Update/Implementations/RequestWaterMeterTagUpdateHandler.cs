using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestWaterMeterTagUpdateHandler : IRequestWaterMeterTagUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterTagQueryService _requestWaterMeterTagQueryService;
        private readonly IValidator<WaterMeterTagRequestUpdateDto> _validator;

        public RequestWaterMeterTagUpdateHandler(
            IMapper mapper,
            IRequestWaterMeterTagQueryService requestWaterMeterTagQueryService,
            IValidator<WaterMeterTagRequestUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterTagQueryService = requestWaterMeterTagQueryService;
            _requestWaterMeterTagQueryService.NotNull(nameof(_requestWaterMeterTagQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(WaterMeterTagRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestWaterMeterTag = await _requestWaterMeterTagQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, requestWaterMeterTag);
        }
    }
}
