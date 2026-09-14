using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.CommunicationPool.Domain.Features.Sms.Commands;
using Aban360.CommunicationPool.Domain.Features.Sms.Queries;
using Aban360.CommunicationPool.Persistence.Features.Sms.Commands.Implementations;
using Aban360.CommunicationPool.Persistence.Features.Sms.Queries.Contracts;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.ReportPool.Domain.Base;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.Base
{
    public interface IMeterReadingDetailSendSms
    {
        Task Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken);
        Task CreateJob(int firstFlowId, Guid userId);
    }
    internal sealed class MeterReadingDetailSendSms : AbstractBaseConnection, IMeterReadingDetailSendSms
    {

        private readonly IBackgroundJobClient _jobClient;
        private readonly ISmsOldHandler _smsOldHandler;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly ISmsStateTemplateQueryService _meterSmsStateTemplateQueryService;
        private readonly ISmsFlowQueryService _meterSmsFlowQueryService;
        private readonly ICalculationConfirmationHandler _calcConfirmationHandler;
        private readonly ISmsDraftQueryService _smsDraftQueryService;
        private const long _minWaterDebt = 100_000;
        private int _smsGroupId = CommonLiterals.SmsStateGroupCollectBills_LastSms;
        public MeterReadingDetailSendSms(
            IBackgroundJobClient jobClient,
            ISmsOldHandler smsOldHandler,
            IMeterFlowQueryService meterFlowQueryService,
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            ICommonZoneService commonZoneService,
            ISmsStateTemplateQueryService meterSmsStateTemplateQueryService,
            ISmsFlowQueryService meterSmsFlowQueryService,
            ICalculationConfirmationHandler calcConfirmationHandler,
            ISmsDraftQueryService smsDraftQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _jobClient = jobClient;
            _jobClient.NotNull(nameof(jobClient));

            _smsOldHandler = smsOldHandler;
            _smsOldHandler.NotNull(nameof(smsOldHandler));

            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));

            _meterReadingDetailQueryService = meterReadingDetailQueryService;
            _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _meterSmsStateTemplateQueryService = meterSmsStateTemplateQueryService;
            _meterSmsStateTemplateQueryService.NotNull(nameof(meterSmsStateTemplateQueryService));

            _meterSmsFlowQueryService = meterSmsFlowQueryService;
            _meterSmsFlowQueryService.NotNull(nameof(meterSmsFlowQueryService));

            _calcConfirmationHandler = calcConfirmationHandler;
            _calcConfirmationHandler.NotNull(nameof(calcConfirmationHandler));

            _smsDraftQueryService = smsDraftQueryService;
            _smsDraftQueryService.NotNull(nameof(smsDraftQueryService));
        }

        public async Task Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            MeterReadingCheckedOutputDto calculationConfirmResult = await _calcConfirmationHandler.Handle(latestFlowId, appUser, cancellationToken);
            MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.Get(latestFlowId);
            await _commonZoneService.IsUserInZone(appUser, meterFlowInfo.ZoneId);

            _jobClient.Enqueue(() => CreateJob(meterFlowInfo.FirstFlowId, appUser.UserId));
        }
        public async Task CreateJob(int firstFlowId, Guid userId)
        {
            MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.Get(firstFlowId);
            IEnumerable<MeterReadingDetailToSendMessageDto> customerInfoToSendSms = await _meterReadingDetailQueryService.GetToSend(firstFlowId, meterFlowInfo.ZoneId, _minWaterDebt, ReportLiterals.RayabOperator);


            int newSmsFlowId = await ExecSql(customerInfoToSendSms, firstFlowId, userId);
            SmsFlowGetDto? newSmsFlowInfo = await _meterSmsFlowQueryService.Get(newSmsFlowId, false);
            if (newSmsFlowInfo is null || newSmsFlowInfo.Id == 0 || newSmsFlowInfo.SendDateTime.HasValue)
            {
                return;
            }
            string jobId = _jobClient.Schedule(() => SendSms(firstFlowId, newSmsFlowId, userId), new DateTimeOffset(newSmsFlowInfo.DueDateTime));

            return;
        }
        public async Task SendSms(int firstFlowId, int newSmsFlowId, Guid userId)
        {
            SmsFlowGetDto? newSmsFlowInfo = await _meterSmsFlowQueryService.Get(newSmsFlowId, false);
            if (newSmsFlowInfo is null || newSmsFlowInfo.Id == 0 || newSmsFlowInfo.SendDateTime.HasValue)
            {
                return;
            }
            IEnumerable<SmsDraftGetDto> smsDraftToSend = await _smsDraftQueryService.Get(firstFlowId.ToString(), newSmsFlowId, false, false);
            IEnumerable<Guid> smsDraftIds = smsDraftToSend?.Select(s => s.Id)?.ToList() ?? new List<Guid>();

            SmsFlowUpdateDto smsFlowUpdateDto = new(newSmsFlowId);
            SmsDraftUpdateDto smsDraftUpdateDto = new(smsDraftIds, firstFlowId.ToString(), newSmsFlowId);
            await ExecSql(smsDraftUpdateDto, true);

            //if(smsDraftCount>0) then send to Sms4040
            //if smsSendResult==200 then update smsDraft.FetchTime  MeterStatFlow.SendTime
            //else makeException
            await ExecSql(smsFlowUpdateDto, smsDraftUpdateDto, false);

            await CreateJob(firstFlowId, userId);

        }
        public ICollection<SmsDraftInsertDto> GetNewSmsDraftList(SmsStateTemplateGetDto? nextTemplate, IEnumerable<MeterReadingDetailToSendMessageDto> customerInfoToSendSms, int smsFlowId)
        {
            ICollection<SmsDraftInsertDto> smsDraftList = new List<SmsDraftInsertDto>();
            if (nextTemplate is not null)
            {
                foreach (var customerInfo in customerInfoToSendSms)
                {
                    string message = nextTemplate.SmsText
                                            .Replace("{ZoneTitle}", customerInfo.ZoneTitle)
                                            .Replace("{FullName}", customerInfo.FullName)
                                            .Replace("{Register}", customerInfo.RegisterDateJalali)
                                            .Replace("{Payable}", customerInfo.Payable.ToString())
                                            .Replace("{BillId}", customerInfo.BillId)
                                            .Replace("{PaymentId}", customerInfo.PaymentId)
                                            .Replace("{DueDateJalali}", customerInfo.DueDateJalali)
                                            .Replace("{PreviousNumber}", customerInfo.PreviousNumber.ToString())
                                            .Replace("{CurrentNumber}", customerInfo.CurrentNumber.ToString())
                                            .Replace("{PreviousDateJalali}", customerInfo.PreviousDateJalali)
                                            .Replace("{CurrentDateJalali}", customerInfo.CurrentDateJalali)
                                            .Replace("{newLine}", Environment.NewLine);
                    SmsDraftInsertDto newSms = new(null, customerInfo.BillId, smsFlowId, message, customerInfo.MobileNumber, customerInfo.FlowImportedId.ToString(), customerInfo.MeterReadingDetailId.ToString());
                    smsDraftList.Add(newSms);
                }
            }
            return smsDraftList;
        }
        public async Task<int> ExecSql(IEnumerable<MeterReadingDetailToSendMessageDto> customerInfoToSendSms, int firstFlowId, Guid userId)
        {
            var (nextTemplate, newSmsFlowInfo) = await GetCurrentTemplateAndFlow(customerInfoToSendSms, firstFlowId, userId);
            int newSmsFlowId = 0;
            if (nextTemplate is not null)
            {
                using (IDbConnection connection = _sqlReportConnection)
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        SmsFlowCommandService smsFlowCommandService = new(connection, transaction);
                        SmsDraftCommandService smsDraftCommandService = new(connection, transaction);

                        newSmsFlowId = await smsFlowCommandService.Insert(newSmsFlowInfo);
                        ICollection<SmsDraftInsertDto> smsDraftList = GetNewSmsDraftList(nextTemplate, customerInfoToSendSms, newSmsFlowId);
                        await smsDraftCommandService.Insert(smsDraftList);

                        transaction.Commit();
                    }
                }
                return newSmsFlowId;
            }
            return newSmsFlowId;
        }
        public async Task ExecSql(SmsDraftUpdateDto updateDto, bool isFetch)
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
                    await smsDraftCommandService.Update(updateDto, isFetch);

                    transaction.Commit();
                }
            }
        }
        public async Task ExecSql(SmsFlowUpdateDto smsFlowUpdate, SmsDraftUpdateDto smsDraftUpdateDto, bool isFetch)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    SmsFlowCommandService smsFlowCommandService = new(connection, transaction);
                    SmsDraftCommandService smsDraftCommandService = new(connection, transaction);

                    await smsFlowCommandService.Update(smsFlowUpdate);
                    await smsDraftCommandService.Update(smsDraftUpdateDto, isFetch);

                    transaction.Commit();
                }
            }
        }
        public SmsFlowInsertDto GetMeterSmsFlowInsertDto(SmsStateTemplateGetDto currentTemplate, int firstFlowId, int smsCount, Guid userId)
        {
            DateTime currentDate = DateTime.Now;
            return new SmsFlowInsertDto()
            {
                FirstFlowId = firstFlowId,
                SmsCount = smsCount,
                SmsTemplateId = currentTemplate.Id,
                InsertBy = userId,
                InsertDateTime = currentDate,
                DueDateTime = currentDate.AddDays(currentTemplate.DueDay)
            };
        }
        public async Task<(SmsStateTemplateGetDto?, SmsFlowInsertDto?)> GetCurrentTemplateAndFlow(IEnumerable<MeterReadingDetailToSendMessageDto> customerInfoToSendSms, int firstFlowId, Guid userId)
        {
            SmsStateTemplateGetDto? nextTemplateInfo = await _meterSmsStateTemplateQueryService.GetNextTemplate(firstFlowId, _smsGroupId);
            if (nextTemplateInfo is not null)
            {
                SmsFlowInsertDto newMeterSmsFlowInfo = GetMeterSmsFlowInsertDto(nextTemplateInfo, firstFlowId, customerInfoToSendSms?.Count() ?? 0, userId);
                return (nextTemplateInfo, newMeterSmsFlowInfo);
            }
            return (null, null);
        }
    }
}