using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.CommunicationPool.Domain.Features.Sms.Commands;
using Aban360.CommunicationPool.Domain.Features.Sms.Queries;
using Aban360.CommunicationPool.Persistence.Features.Sms.Commands.Implementations;
using Aban360.CommunicationPool.Persistence.Features.Sms.Queries.Contracts;
using Aban360.NotificationPool.Application.Features.Sms;
using Aban360.NotificationPool.Domain.Features.Sms;
using Aban360.ReportPool.Domain.Base;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterReadingDetailSendSmsHandler : AbstractBaseConnection, IMeterReadingDetailSendSmsHandler
    {
        private readonly IBackgroundJobClient _jobClient;
        private readonly ISmsOldHandler _smsOldHandler;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly ISmsStateTemplateQueryService _meterSmsStateTemplateQueryService;
        private readonly ISmsFlowQueryService _meterSmsFlowQueryService;
        private readonly ICalculationConfirmationHandler _calcConfirmationHandler;
        private readonly ISmsDraftQueryService _smsDraftQueryService;
        private const long _minWaterDebt = 30_000;
        private int _defaultSmsFlowSmsCount = -1;
        private int _smsGroupId = CommonLiterals.SmsStateGroupCollectBills_LastSms;
        public MeterReadingDetailSendSmsHandler(
            IBackgroundJobClient jobClient,
            ISmsOldHandler smsOldHandler,
            IMeterFlowQueryService meterFlowQueryService,
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
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

            _meterSmsStateTemplateQueryService = meterSmsStateTemplateQueryService;
            _meterSmsStateTemplateQueryService.NotNull(nameof(meterSmsStateTemplateQueryService));

            _meterSmsFlowQueryService = meterSmsFlowQueryService;
            _meterSmsFlowQueryService.NotNull(nameof(meterSmsFlowQueryService));

            _calcConfirmationHandler = calcConfirmationHandler;
            _calcConfirmationHandler.NotNull(nameof(calcConfirmationHandler));

            _smsDraftQueryService = smsDraftQueryService;
            _smsDraftQueryService.NotNull(nameof(smsDraftQueryService));



            string BillToDateTable = @"آبفا {ZoneTitle}{newLine}{BillTypeLiteral}{newLine}{FullName}{newLine}از: {PreviousDateJalali}{newLine}تا: {CurrentDateJalali}{newLine}از شماره: {PreviousNumber}{newLine}تا شماره: {CurrentNumber}{newLine}{TarefeSaxtValue}دوره: {ThisPeriodAmountValue} ریال{newLine}{PreBedBesLiteral}{PreBedBesValue} ریال{newLine}مبلغ: {Payable} ریال{newLine}ش ق: {BillId}{newLine}{PayText}{newLine}{DeadlineText}{newLine}{LinkValue}";
            string Bill = @"{ZoneTitle}{newLine}" +
                @"{BillTypeLiteral}{newLine}" +
                @"{FullName}{newLine}" +
                @"از: {PreviousDateJalali}{newLine}" +
                @"تا: {CurrentDateJalali}{newLine}" +
                @"از شماره: {PreviousNumber}{newLine}" +
                @"تا شماره: {CurrentNumber}{newLine}" +
                @"{TarefeSaxtValue}" +
                @"دوره: {ThisPeriodAmountValue} ریال{newLine}" +
                @"{PreBedBesLiteral}{PreBedBesValue} ریال{newLine}" +
                @"مبلغ: {Payable} ریال{newLine}" +
                @"ش ق: {BillId}{newLine}" +
                @"{PayText}{newLine}" +
                @"{DeadlineText}{newLine}" +
                @"{LinkValue}";

            string dueBill = @"آبفا {ZoneTitle}{newLine}مهلت صورتحساب آب‌بها {FullName} به پایان رسیده است. جهت استمرار خدمات مبلغ {Payable} ریال پرداخت گردد.{newLine}ش ق: {BillId}{newLine}ش پ: {PaymentId}{newLine}لطفاً جهت پرداخت مبلغ در ساعات اداری از طریق لینک زیر و در ساعات غیر‌اداری از طریق شناسه قبض و پرداخت اقدام نمائید.{newLine}{LinkValue}";
            string rimember = @"آبفا {ZoneTitle}{newLine}مهلت صورتحساب آب‌بها {FullName} رو به پایان است لطفاً جهت استمرار خدمات مبلغ {Payable} ریال پرداخت گردد.{newLine}ش ق: {BillId}{newLine}ش پ: {PaymentId}{newLine}لطفاً جهت پرداخت مبلغ در ساعات اداری از طریق لینک زیر و در ساعات غیر‌اداری از طریق شناسه قبض و پرداخت اقدام نمائید.{newLine}{LinkValue}";
        }
        public async Task Handle(int latestFlowId, Guid userId, CancellationToken cancellationToken)
        {
            MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.Get(latestFlowId);
            if (meterFlowInfo.ZoneId != 131302)
            {
                return;
            }
            _jobClient.Enqueue(() => CreateJob(meterFlowInfo.FirstFlowId, userId));
        }
        [AutomaticRetry(Attempts = 2)]
        public async Task CreateJob(int firstFlowId, Guid userId)
        {
            var (nextTemplate, newMeterSmsFlowInfo) = await GetCurrentTemplateAndFlow(firstFlowId, userId);
            int newSmsFlowId = await ExecSql(nextTemplate, newMeterSmsFlowInfo);
            SmsFlowGetDto? newSmsFlowInfo = await _meterSmsFlowQueryService.Get(newSmsFlowId, false);
            if (newSmsFlowInfo is null || newSmsFlowInfo.Id == 0 || newSmsFlowInfo.SendDateTime.HasValue)
            {
                return;
            }
            string jobId = _jobClient.Schedule(() => SendSms(newSmsFlowId, userId), new DateTimeOffset(newSmsFlowInfo.DueDateTime));//ToDo: changeDateTimeType

            return;
        }
        [AutomaticRetry(Attempts = 2)]
        public async Task SendSms(int newSmsFlowId, Guid userId)
        {
            SmsFlowGetDto? newSmsFlowInfo = await _meterSmsFlowQueryService.Get(newSmsFlowId, false);
            if (newSmsFlowInfo is null || newSmsFlowInfo.Id == 0 || newSmsFlowInfo.SendDateTime.HasValue)
            {
                return;
            }
            SmsStateTemplateGetDto currentSmsTemplateToSendInfo = await _meterSmsStateTemplateQueryService.Get(newSmsFlowInfo.SmsTemplateId);
            MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.Get(newSmsFlowInfo.FirstFlowId);
            IEnumerable<MeterReadingDetailToSendMessageDto> customerInfoToSendSms = await _meterReadingDetailQueryService.GetToSend(newSmsFlowInfo.FirstFlowId, meterFlowInfo.ZoneId, _minWaterDebt, ReportLiterals.RayabOperator);
            await ExecSql(customerInfoToSendSms, currentSmsTemplateToSendInfo, newSmsFlowInfo.Id, userId);

            IEnumerable<SmsDraftGetDto> smsDraftToSend = await _smsDraftQueryService.Get(newSmsFlowInfo.FirstFlowId.ToString(), newSmsFlowId, false, false);
            IEnumerable<Guid> smsDraftIds = smsDraftToSend?.Select(s => s.Id)?.ToList() ?? new List<Guid>();

            SmsDraftUpdateDto smsDraftUpdateDto = new(smsDraftIds, newSmsFlowInfo.FirstFlowId.ToString(), newSmsFlowId);
            await ExecSql(smsDraftUpdateDto, true);

            IEnumerable<SendSmsDataDto> smsData = GetSmsDto(smsDraftToSend);
            if ((smsData?.Count() ?? 0) > 0)
            {
                await _smsOldHandler.Send(smsData);
                SmsFlowUpdateDto smsFlowUpdateDto = new(newSmsFlowId);
                await ExecSql(smsFlowUpdateDto, smsDraftUpdateDto, false);
            }
            await CreateJob(newSmsFlowInfo.FirstFlowId, userId);

        }
        public ICollection<SmsDraftInsertDto> GetNewSmsDraftList(SmsStateTemplateGetDto? nextTemplate, IEnumerable<MeterReadingDetailToSendMessageDto> customerInfoToSendSms, int smsFlowId)
        {
            ICollection<SmsDraftInsertDto> smsDraftList = new List<SmsDraftInsertDto>();
            if (nextTemplate is not null)
            {
                foreach (var customerInfo in customerInfoToSendSms)
                {
                    string billTypeLiteral = customerInfo.CounterSatetCode == (int)CounterStateCodeEnum.Close ? "نحوه محاسبه: براوردی" : "صورتحساب آب بها";
                    long preBedBes = customerInfo.Sum + customerInfo.DiscountAmount - customerInfo.Baha;
                    long preBedBesValue = Math.Abs(preBedBes);
                    string preBedBesLiteral = preBedBes < 0 ? "بستانکاری: " : "بدهی: ";
                    string tarefeSaxtValue = customerInfo.BranchTypeId == (int)BranchTypeEnum.SakhtOSaz ? @"تعرفه: ساخت و ساز{newLine}" : "";
                    var thisPeriodAmountValue = customerInfo.Baha + customerInfo.DiscountAmount;
                    var hasBillPayDead = customerInfo.Payable > 0 ? true : false;
                    var deadlineText = customerInfo.Sum - customerInfo.Baha > 30000 ? "فوری" : "مهلت: {DueDateJalali}";
                    string payText = hasBillPayDead ? @"ش پ: {PaymentId}" : string.Empty;
                    string linkValue = @"https://crm.abfaesfahan.ir/" + customerInfo.BillId.Trim() + "/sms/bill";

                    string message = nextTemplate.SmsText
                                            .Replace("{ZoneTitle}", customerInfo.ZoneTitle)
                                            .Replace("{FullName}", customerInfo.FullName)
                                            .Replace("{Register}", customerInfo.RegisterDateJalali)
                                            .Replace("{Payable}", customerInfo.Payable.ToString())
                                            .Replace("{Baha}", customerInfo.Baha.ToString())
                                            .Replace("{Jam}", customerInfo.Sum.ToString())
                                            .Replace("{DiscountAmount}", customerInfo.DiscountAmount.ToString())
                                            .Replace("{PreBedBes}", preBedBes.ToString())
                                            .Replace("{PreBedBesValue}", preBedBesValue.ToString())
                                            .Replace("{PreBedBesLiteral}", preBedBesLiteral)
                                            .Replace("{PayText}", payText)
                                            .Replace("{BillId}", customerInfo.BillId)
                                            .Replace("{PaymentId}", customerInfo.PaymentId)
                                            .Replace("{DeadlineText}", deadlineText)
                                            .Replace("{DueDateJalali}", customerInfo.DueDateJalali)
                                            .Replace("{PreviousNumber}", customerInfo.PreviousNumber.ToString())
                                            .Replace("{CurrentNumber}", customerInfo.CurrentNumber.ToString())
                                            .Replace("{PreviousDateJalali}", customerInfo.PreviousDateJalali)
                                            .Replace("{CurrentDateJalali}", customerInfo.CurrentDateJalali)
                                            .Replace("{TarefeSaxtValue}", tarefeSaxtValue)
                                            .Replace("{ThisPeriodAmountValue}", thisPeriodAmountValue.ToString())
                                            .Replace("{LinkValue}", linkValue)
                                            .Replace("{BillTypeLiteral}", billTypeLiteral)
                                            .Replace("{newLine}", Environment.NewLine);
                    SmsDraftInsertDto newSms = new(null, customerInfo.BillId, customerInfo.PaymentId, smsFlowId, message, customerInfo.MobileNumber, customerInfo.FlowImportedId.ToString(), customerInfo.MeterReadingDetailId.ToString());
                    smsDraftList.Add(newSms);
                }
            }
            return smsDraftList;
        }
        public async Task<int> ExecSql(SmsStateTemplateGetDto? nextTemplate, SmsFlowInsertDto? newSmsFlowInfo)
        {
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
                        newSmsFlowId = await smsFlowCommandService.Insert(newSmsFlowInfo);
                        transaction.Commit();
                    }
                }
                return newSmsFlowId;
            }
            return newSmsFlowId;
        }
        public async Task ExecSql(IEnumerable<MeterReadingDetailToSendMessageDto> customerInfoToSendSms, SmsStateTemplateGetDto? nextTemplate, int newSmsflowId, Guid userId)
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

                    await smsFlowCommandService.Update(newSmsflowId, customerInfoToSendSms?.Count() ?? 0);
                    ICollection<SmsDraftInsertDto> smsDraftList = GetNewSmsDraftList(nextTemplate, customerInfoToSendSms, newSmsflowId);
                    await smsDraftCommandService.Insert(smsDraftList);

                    transaction.Commit();
                }
            }
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
        public async Task<(SmsStateTemplateGetDto?, SmsFlowInsertDto?)> GetCurrentTemplateAndFlow(int firstFlowId, Guid userId)
        {
            SmsStateTemplateGetDto? nextTemplateInfo = await _meterSmsStateTemplateQueryService.GetNextTemplate(firstFlowId, _smsGroupId);
            if (nextTemplateInfo is not null)
            {
                SmsFlowInsertDto newMeterSmsFlowInfo = GetMeterSmsFlowInsertDto(nextTemplateInfo, firstFlowId, _defaultSmsFlowSmsCount, userId);
                return (nextTemplateInfo, newMeterSmsFlowInfo);
            }
            return (null, null);
        }
        private IEnumerable<SendSmsDataDto> GetSmsDto(IEnumerable<SmsDraftGetDto> input)
        {
            return input.Select(s =>
            new SendSmsDataDto(s.Message, s.MobileNumber, s.BillId, s.PaymentId, s.Id)
            ).ToList();
        }
    }
}
