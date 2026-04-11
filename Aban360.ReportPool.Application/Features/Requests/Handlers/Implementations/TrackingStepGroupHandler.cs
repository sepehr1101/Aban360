using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Requests.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Request.Inputs;
using Aban360.ReportPool.Domain.Features.Request.Outputs;
using Aban360.ReportPool.Persistence.Features.Request.Contracts;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.Requests.Handlers.Implementations
{
    internal sealed class TrackingStepGroupHandler : ITrackingStepGroupHandler
    {
        private readonly ITrackingStepGroupQueryService _queryService;
        private readonly IValidator<TrackingInputDto> _validator;
        public TrackingStepGroupHandler(
            ITrackingStepGroupQueryService queryService,
            IValidator<TrackingInputDto> validator)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<TrackingStepHeaderOutputDto, TrackingStepGroupDataOutputDto>> Handle(TrackingInputDto inputDto, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            return await _queryService.Get(inputDto);
        }
        private async Task InputValidation(TrackingInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
