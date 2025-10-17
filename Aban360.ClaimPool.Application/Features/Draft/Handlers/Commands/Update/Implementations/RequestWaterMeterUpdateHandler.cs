using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;
using System.Threading;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Implementations
{
    internal sealed class RequestWaterMeterUpdateHandler : IRequestWaterMeterUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterQueryService _requestWaterMeterQueryService;
        private readonly IValidator<WaterMeterRequestUpdateDto> _validator;

        public RequestWaterMeterUpdateHandler(
            IMapper mapper,
            IRequestWaterMeterQueryService requestWaterMeterQueryService,
            IValidator<WaterMeterRequestUpdateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterQueryService = requestWaterMeterQueryService;
            _requestWaterMeterQueryService.NotNull(nameof(_requestWaterMeterQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IAppUser currentUser, WaterMeterRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            var requestWaterMeter = await _requestWaterMeterQueryService.Get(updateDto.Id);
            requestWaterMeter.Hash = "-";
            requestWaterMeter.InsertLogInfo = "-";
            requestWaterMeter.ValidFrom = DateTime.Now;
            requestWaterMeter.UserId = currentUser.UserId;

            _mapper.Map(updateDto, requestWaterMeter);
        }
    }
}
