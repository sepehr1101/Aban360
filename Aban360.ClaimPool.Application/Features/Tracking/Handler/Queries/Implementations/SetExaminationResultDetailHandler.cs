using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class SetExaminationResultDetailHandler : ISetExaminationResultDetailHandler
    {
        private readonly ITrackingDetailQueryService _trackingDetailQueryService;
        private readonly IValidator<TrackingDetailGetDto> _validator;
        public SetExaminationResultDetailHandler(
            ITrackingDetailQueryService trackingDetailQueryService,
            IValidator<TrackingDetailGetDto> validator)
        {
            _trackingDetailQueryService = trackingDetailQueryService;
            _trackingDetailQueryService.NotNull(nameof(trackingDetailQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<SetExaminationResultOutputDto> Handle(TrackingDetailGetDto inputDto, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);
            SetExaminationResultDataDto data = await _trackingDetailQueryService.GetSetExaminationResultDto(inputDto);
            return GetResult(data);
        }
        private async Task Validation(TrackingDetailGetDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new BaseException(message);
            }
        }
        private SetExaminationResultOutputDto GetResult(SetExaminationResultDataDto input)
        {
            return new SetExaminationResultOutputDto()
            {
                AssessmentCode = input.AssessmentCode,
                AssessmentName = input.AssessmentName,
                AssessmentMobile = input.AssessmentMobile,
                AssessmentDayJalali = input.AssessmentDayJalali,
                FullName = input.FullName,
                TrackNumber = input.TrackNumber,
                Address = input.Address,
                MobileNumber = input.MobileNumber,
                AssessmentResultTitle = input.AssessmentResultTitle,
                HasTrench = (input.IsNewBranch || input.IsNewSewage) && input.IsResultSuccess,
            };
        }
    }
}
