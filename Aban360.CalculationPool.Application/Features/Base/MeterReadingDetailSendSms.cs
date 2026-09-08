using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Hangfire;

namespace Aban360.CalculationPool.Application.Features.Base
{
    public interface IMeterReadingDetailSendSms
    {
        Task SendSms(int firstFlowId, IAppUser appUser, CancellationToken cancellationToken);
    }
    internal sealed class MeterReadingDetailSendSms : IMeterReadingDetailSendSms
    {
        private readonly IBackgroundJobClient _jobClient;
        private readonly ISmsOldHandler _smsOldHandler;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly IMembersQueryService _membersQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IMeterSmsStateTemplateQueryService _meterSmsStateTemplateQueryService;
        private readonly IMeterSmsFlowQueryService _meterSmsFlowQueryService;
        public MeterReadingDetailSendSms(
            IBackgroundJobClient jobClient,
            ISmsOldHandler smsOldHandler,
            IMeterFlowQueryService meterFlowQueryService,
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            IMembersQueryService membersQueryService,
            ICommonZoneService commonZoneService,
            IMeterSmsStateTemplateQueryService meterSmsStateTemplateQueryService,
            IMeterSmsFlowQueryService meterSmsFlowQueryService)
        {
            _jobClient = jobClient;
            _jobClient.NotNull(nameof(jobClient));

            _smsOldHandler = smsOldHandler;
            _smsOldHandler.NotNull(nameof(smsOldHandler));

            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));

            _meterReadingDetailQueryService = meterReadingDetailQueryService;
            _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));

            _membersQueryService = membersQueryService;
            _membersQueryService.NotNull(nameof(membersQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _meterSmsStateTemplateQueryService = meterSmsStateTemplateQueryService;
            _meterSmsStateTemplateQueryService.NotNull(nameof(meterSmsStateTemplateQueryService));

            _meterSmsFlowQueryService = meterSmsFlowQueryService;
            _meterSmsFlowQueryService.NotNull(nameof(meterSmsFlowQueryService));
        }

        public async Task SendSms(int firstFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.Get(firstFlowId);
            await _commonZoneService.IsUserInZone(appUser, meterFlowInfo.ZoneId);

            IEnumerable<MeterReadingDetailDataOutputDto> meterReadingDetailInfo = await _meterReadingDetailQueryService.Get(firstFlowId, hasExcluded: false);
            //Select Someone who have members.bedbes>100.000 


            //Send Sms to CustomersUp

        }
    }
}
