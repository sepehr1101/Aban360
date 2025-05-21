using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Draft.Entites;
using Aban360.ClaimPool.Persistence.Features.Draft.Commands.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using AutoMapper;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Implementations
{
    internal sealed class RequestWaterMeterCreateHandler : IRequestWaterMeterCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IRequestWaterMeterCommandService _requestWaterMeterCommandService;
        private readonly IValidator<WaterMeterRequestCreateDto> _validator;

        public RequestWaterMeterCreateHandler(
            IMapper mapper,
            IRequestWaterMeterCommandService requestWaterMeterCommandService,
            IValidator<WaterMeterRequestCreateDto> validator)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _requestWaterMeterCommandService = requestWaterMeterCommandService;
            _requestWaterMeterCommandService.NotNull(nameof(_requestWaterMeterCommandService));

            _validator = validator;
            _validator.NotNull(nameof(validator));

        }

        public async Task Handle(IAppUser currentUser, WaterMeterRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomeValidationException(message);
            }

            var requestWaterMeter = _mapper.Map<RequestWaterMeter>(createDto);
            requestWaterMeter.Hash = "-";
            requestWaterMeter.InsertLogInfo = "-";
            requestWaterMeter.ValidFrom = DateTime.Now;
            requestWaterMeter.UserId = currentUser.UserId;

            createDto.TagIds.ForEach(tags =>
            {
                RequestWaterMeterTag requestWaterMeterTag = new RequestWaterMeterTag()
                {
                    RequestWaterMeter = requestWaterMeter,
                    WaterMeterTagDefinitionId = tags,
                    Hash = "-",
                    InsertLogInfo = "-",
                    ValidFrom = DateTime.Now,
                };
                requestWaterMeter.WaterMeterTags.Add(requestWaterMeterTag);
            });

            await _requestWaterMeterCommandService.Add(requestWaterMeter);
        }
    }
}
