using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.CommunicationPool.Persistence.Features.Sms.Queries.Implementations;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;

namespace Aban360.CalculationPool.Application.Features.Base
{
    public interface IMeterReadingUploadDbFileWithSendCloseSmsJobService
    {
        Task Upload(MeterReadingFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
    internal sealed class MeterReadingUploadDbFileWithSendCloseSmsJobService : IMeterReadingUploadDbFileWithSendCloseSmsJobService
    {
        private readonly IBackgroundJobClient _jobClient;
        private readonly ISmsOldHandler _smsOldHandler;
        private readonly IMeterReadingFileCreateHandler _meterReadingFileCreateHandler;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly IT51QueryService _zoneQueryService;
        private readonly ISmsDraftQueryService _draftQueryService;
        public MeterReadingUploadDbFileWithSendCloseSmsJobService(
            IMeterReadingFileCreateHandler meterReadingFileCreateHandler,
            IBackgroundJobClient jobClient,
            ISmsOldHandler smsOldHandler,
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            IT51QueryService zoneQueryService,
            ISmsDraftQueryService draftQueryService)
        {
            _meterReadingFileCreateHandler = meterReadingFileCreateHandler;
            _meterReadingFileCreateHandler.NotNull(nameof(meterReadingFileCreateHandler));

            _jobClient = jobClient;
            _jobClient.NotNull(nameof(jobClient));

            _smsOldHandler = smsOldHandler;
            _smsOldHandler.NotNull(nameof(smsOldHandler));

            _meterReadingDetailQueryService = meterReadingDetailQueryService;
            _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));

            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));

            _draftQueryService = draftQueryService;
            _draftQueryService.NotNull(nameof(draftQueryService));
        }

        public async Task Upload(MeterReadingFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = await _meterReadingFileCreateHandler.Handle(input, appUser, cancellationToken);
            int flowImportedId = result?.ReportData?.FirstOrDefault()?.FlowImportedId ?? 0;
            if (flowImportedId > 0)
            {
                _jobClient.Enqueue(() => SendSms(flowImportedId));
            }
        }
        public async Task SendSms(int flowStepId)
        {
            IEnumerable<MeterReadingDetailDataOutputDto> meterReadingDetailDto = await _meterReadingDetailQueryService.Get(flowStepId, false);
            IEnumerable<MeterReadingDetailDataOutputDto> meterReadingToSendSms = meterReadingDetailDto?.Where(m => m.CurrentCounterStateCode == (int)CounterStateCodeEnum.Close) ?? new List<MeterReadingDetailDataOutputDto>();
            NumericDictionary zoneInfo = await _zoneQueryService.Get(meterReadingDetailDto?.FirstOrDefault()?.ZoneId ?? 0, false);
            /////TODO: Use SmsDraft
            foreach (var item in meterReadingToSendSms)
            {
                string smsText = string.Format(SmsTemplates.ClosedBill, zoneInfo?.Title ?? string.Empty, item.BillId, Environment.NewLine);
                _jobClient.Enqueue(() => _smsOldHandler.Send(item.MobileNumber ?? string.Empty, smsText, Guid.NewGuid()));//todo:Guid
                //OutBox pattern
            }
        }
    }
}
