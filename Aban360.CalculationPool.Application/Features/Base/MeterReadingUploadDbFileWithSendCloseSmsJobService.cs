using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.CommunicationPool.Domain.Features.Sms.Queries;
using Aban360.CommunicationPool.Persistence.Features.Sms.Commands.Implementations;
using Aban360.CommunicationPool.Persistence.Features.Sms.Queries.Implementations;
using Aban360.NotificationPool.Application.Features.Sms;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.Base
{
    public interface IMeterReadingUploadDbFileWithSendCloseSmsJobService
    {
        Task Upload(MeterReadingFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken);
        Task SendSms(int flowStepId);
    }
    internal sealed class MeterReadingUploadDbFileWithSendCloseSmsJobService : AbstractBaseConnection, IMeterReadingUploadDbFileWithSendCloseSmsJobService
    {
        private readonly IBackgroundJobClient _jobClient;
        private readonly ISmsOldHandler _smsOldHandler;
        private readonly IMeterReadingFileCreateHandler _meterReadingFileCreateHandler;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly ISmsDraftQueryService _draftQueryService;
        public MeterReadingUploadDbFileWithSendCloseSmsJobService(
            IMeterReadingFileCreateHandler meterReadingFileCreateHandler,
            IBackgroundJobClient jobClient,
            ISmsOldHandler smsOldHandler,
            IMeterFlowQueryService meterFlowQueryService,
            ISmsDraftQueryService draftQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _meterReadingFileCreateHandler = meterReadingFileCreateHandler;
            _meterReadingFileCreateHandler.NotNull(nameof(meterReadingFileCreateHandler));

            _jobClient = jobClient;
            _jobClient.NotNull(nameof(jobClient));

            _smsOldHandler = smsOldHandler;
            _smsOldHandler.NotNull(nameof(smsOldHandler));

            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));

            _draftQueryService = draftQueryService;
            _draftQueryService.NotNull(nameof(draftQueryService));
        }

        public async Task Upload(MeterReadingFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = await _meterReadingFileCreateHandler.Handle(input, appUser, cancellationToken);
            int flowImportedId = result?.ReportData?.FirstOrDefault()?.FlowImportedId ?? 0;
            _jobClient.Enqueue(() => SendSms(flowImportedId));
        }
        public async Task SendSms(int flowStepId)
        {
            IEnumerable<SmsDraftGetDto> smsDraftInfo = await _draftQueryService.GetByReferenceId(flowStepId.ToString(), hasFetchDate: false, hasSendDate: false);
            if (smsDraftInfo.Any())
            {
                await UpdateDateExecSql(smsDraftInfo.Select(s => s.Id).ToList(), flowStepId.ToString(),true);

                //SendSms
                //if SmsResult==200 -> UpdateSendDateTime
                await UpdateDateExecSql(smsDraftInfo.Select(s => s.Id).ToList(), flowStepId.ToString(), false);
            }
            await SendSms(flowStepId);
        }
        public async Task UpdateDateExecSql(IEnumerable<Guid> smsIds, string referenceId,bool isFetch)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    SmsDraftCommandService smsDraftCommandService = new(connection, transaction);
                    await smsDraftCommandService.Update(smsIds, DateTime.Now, referenceId, isFetch);

                    transaction.Commit();
                }
            }
        }
    }
}