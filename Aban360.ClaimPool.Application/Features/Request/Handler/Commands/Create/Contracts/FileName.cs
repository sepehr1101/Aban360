using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts
{
    public interface ISetAssessmentResultHandler
    {
        Task Handle(AssessmentResultInputDto inputDto, CancellationToken cancellationToken);
    }
    internal sealed class SetAssessmentResultHandler : ISetAssessmentResultHandler
    {
        private readonly IMoshtrakCommandService _moshtrackCommandService;
        private readonly ITrackingCommandService _trackingCommandService;
        private static int _status = 110;
        public SetAssessmentResultHandler(
            IMoshtrakCommandService moshtrackCommandService,
            ITrackingCommandService trackingCommandService)
        {
            _moshtrackCommandService = moshtrackCommandService;
            _moshtrackCommandService.NotNull(nameof(moshtrackCommandService));

            _trackingCommandService = trackingCommandService;
            _trackingCommandService.NotNull(nameof(trackingCommandService));
        }

        public async Task Handle(AssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            TrackingInsertDto trackingInsertDto = new(inputDto.TrackNumber,_status,"");

            await _trackingCommandService.UpdateIsConsiderd(inputDto.TrackNumber);
           // await _trackingCommandService.Insert()
        }
        
    }
}
