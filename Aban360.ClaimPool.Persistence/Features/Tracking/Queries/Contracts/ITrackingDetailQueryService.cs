using Aban360.ClaimPool.Domain.Features.Tracking.Dto;

namespace Aban360.ClaimPool.Persistence.Features.Tracking.Queries.Contracts
{
    public interface ITrackingDetailQueryService
    {
        Task<RequestIsRegisterdDto> GetRequestIsRegistered(TrackingDetailGetDto inputDto);
        Task<ExamineTimeSetOutputDto> GetExamineTimeSetDto(TrackingDetailGetDto inputDto);
        Task<SetExaminationResultOutputDto> GetSetExaminationResultDto(TrackingDetailGetDto inputDto);
        Task<TrackNumberAndDescriptionOutputDto> GetTrackNumberAndDescription(TrackingDetailGetDto inputDto);
        Task<CalculationConfirmedDto> GetCalculationConfirmed(TrackingDetailGetDto inputDto);
        Task<CustomerNumberSpecifiedOutputDto> GetCustomerNumberSpecified(TrackingDetailGetDto inputDto);
        Task<AmountConfirmedOutputDto> GetAmountConfirmed(TrackingDetailGetDto inputDto);
    }
}
