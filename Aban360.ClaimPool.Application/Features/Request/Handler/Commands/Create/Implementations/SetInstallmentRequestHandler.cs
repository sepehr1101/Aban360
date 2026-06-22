using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class SetInstallmentRequestHandler : AbstractBaseConnection, ISetInstallmentRequestHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IKartQueryService _kartQueryService;
        private readonly IVariabService _variabService;
        private readonly IValidator<InstallmentRequestInputDto> _validator;
        static int[] _enableStatus = { 60, 75 };
        static int _maxInterval = 3;
        static int _maxInstallmentCount = 9;
        static int _prePaymentType = 0;
        static int _installmentType = 1;
        static int _operator = 666;
        static string _title = "تقسیط";
        static string _insertBy = "Aban";
        public SetInstallmentRequestHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IKartQueryService kartQueryService,
            IVariabService variabService,
            IValidator<InstallmentRequestInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _kartQueryService = kartQueryService;
            _kartQueryService.NotNull(nameof(kartQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto>> Handle(InstallmentRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await GetTrackingWithValidation(inputDto, cancellationToken);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            IEnumerable<CalculationRequestDisplayDataOutputDto> kartInfo = await _kartQueryService.Get(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);
            if (string.IsNullOrWhiteSpace(trackingInfo.BillId))
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
            }

            ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto> result = GetResult(inputDto, kartInfo, trackingInfo, moshtrakInfo);
            IEnumerable<GhestInsertDto> ghestsInsertDto = await GetGhestsInsertDto(result.ReportData, trackingInfo, moshtrakInfo);
            MoshtrakInstallmentUpdateDto moshtrakUpdate = new(trackingInfo.TrackNumber, inputDto.PrepaymentPercent, inputDto.InstallmentCount);
            KartInstallmentUpdateDto kartUpdateDto = new()
            {
                ZoneId = moshtrakInfo.ZoneId,
                StringTrackNumber = moshtrakInfo.StringTrackNumber,
                FirstInstallment = ghestsInsertDto?.Where(g => g.InstallmentNumber == 0)?.FirstOrDefault()?.Cod3 ?? 0,
                InstallmentPercent = inputDto.PrepaymentPercent,
                InstallmentCount = inputDto.InstallmentCount,
                Installment = ghestsInsertDto?.Where(g => g.InstallmentNumber != 0)?.FirstOrDefault()?.Cod3 ?? 0,
                InsertedBy = _insertBy,
                Operator = _operator
            };
            await ExecSql(ghestsInsertDto, moshtrakUpdate, kartUpdateDto, trackingInfo.ZoneId);

            return result;
        }
        private async Task ExecSql(IEnumerable<GhestInsertDto> ghestsInsertDto, MoshtrakInstallmentUpdateDto moshtrakUpdate, KartInstallmentUpdateDto kartUpdateDto, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    GhestCommandService ghestCommandService = new(connection, transaction);
                    KartCommandService kartCommandService = new(connection, transaction);
                    MoshtrakCommandService moshtrakCommandService = new(connection, transaction);

                    await ghestCommandService.Remove(ghestsInsertDto?.FirstOrDefault()?.StringTrackNumber ?? string.Empty, dbName);
                    await ghestCommandService.Insert(ghestsInsertDto, dbName);
                    await moshtrakCommandService.Update(moshtrakUpdate, dbName);
                    await kartCommandService.Update(kartUpdateDto, dbName);

                    transaction.Commit();
                }
            }
        }
        private async Task<TrackingOutputDto> GetTrackingWithValidation(InstallmentRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            if (!_enableStatus.Contains(trackingInfo.StatusId))
            {
                //throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
            if (inputDto.MonthlyDuration > _maxInterval)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidMonthlyDuration(_maxInterval));
            }
            if (inputDto.InstallmentCount > _maxInstallmentCount)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInstallmentCount(_maxInstallmentCount));
            }

            return trackingInfo;
        }
        private (long, long, long) GetInstallmentAmount(InstallmentRequestInputDto inputDto, long payable)
        {
            long firstInstallmentWithZero = (long)(payable * (inputDto.PrepaymentPercent / 100.0));
            long firstInstallmentWithoutZero = (firstInstallmentWithZero / 1000) * 1000;
            if (inputDto.InstallmentCount == 1 || inputDto.PrepaymentPercent == 100)
            {
                return (firstInstallmentWithoutZero, 0, 0);
            }

            long otherAmount = payable - firstInstallmentWithoutZero;
            long eachInstallmentAmountWithZero = otherAmount / (inputDto.InstallmentCount);
            long eachInstallmentAmountWithoutZero = (eachInstallmentAmountWithZero / 1000) * 1000;
            long remain = otherAmount - (eachInstallmentAmountWithoutZero * (inputDto.InstallmentCount));

            return (firstInstallmentWithoutZero, eachInstallmentAmountWithoutZero, remain);
        }
        private IEnumerable<InstallmentRequestDataOutputDto> GetData(InstallmentRequestInputDto inputDto, IEnumerable<CalculationRequestDisplayDataOutputDto> kartInfo, string billId)
        {
            string[] dueDatesJalali = GetDate(inputDto.InstallmentCount + 1, inputDto.MonthlyDuration);
            long amount = kartInfo?.Sum(x => x.Amount) ?? 0;
            long discount = kartInfo?.Sum(x => x.Discount) ?? 0;
            long payable = amount - discount;//todo: true or not?

            if (inputDto.PrepaymentPercent == 100 || inputDto.InstallmentCount == 1)
            {
                return GetCashInstallments(billId, dueDatesJalali[0], payable);
            }
            else
            {
                return GetInstallments(inputDto, billId, dueDatesJalali, payable);
            }
        }
        private ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto> GetResult(InstallmentRequestInputDto inputDto, IEnumerable<CalculationRequestDisplayDataOutputDto> kartInfo, TrackingOutputDto trackingInfo, MoshtrakOutputDto moshtrakInfo)
        {
            IEnumerable<InstallmentRequestDataOutputDto> data = GetData(inputDto, kartInfo, trackingInfo.BillId);
            InstallmentRequestHeaderOutputDto header = new()
            {
                Amount = data?.Sum(x => x.Amount) ?? 0,
                InstallmentCount = inputDto.InstallmentCount,
                PrePaymentAmount = data?.FirstOrDefault()?.Amount ?? 0,
                PrePaymentPercent = inputDto.PrepaymentPercent,//todo,
                InstallmentAmount = (data?.Count() ?? 0) > 1 ? data?.ElementAt(1)?.Amount ?? 0 : 0,//todo
                TrackNumber = trackingInfo.TrackNumber,
                ServiceGroupTitle = trackingInfo.ServiceGroupTitle,
                BillId = trackingInfo.BillId,
                RegionTitle = trackingInfo.RegionTitle,
                ZoneTitle = trackingInfo.ZoneTitle,
                FullName = $"{moshtrakInfo?.FirstName ?? string.Empty} {moshtrakInfo?.Surname ?? string.Empty}",
                RecordCount = data?.Count() ?? 0,
                Title = _title,
            };
            return new ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto>(_title, header, data);
        }
        private IEnumerable<InstallmentRequestDataOutputDto> GetInstallments(InstallmentRequestInputDto inputDto, string billId, string[] dueDatesJalali, long payable)
        {
            var (firstInstallmentWithoutZero, eachInstallmentAmountWithoutZero, remain) = GetInstallmentAmount(inputDto, payable);
            string firstPaymentId = TransactionIdGenerator.GeneratePaymentId(firstInstallmentWithoutZero, billId, $"20{0}");
            InstallmentRequestDataOutputDto firstInstallment = new(firstInstallmentWithoutZero + remain, dueDatesJalali[0], firstPaymentId, 0);
            ICollection<InstallmentRequestDataOutputDto> data = new List<InstallmentRequestDataOutputDto>(); ;
            data.Add(firstInstallment);
            for (int i = 1; i <= inputDto.InstallmentCount; i++)
            {
                string dueDateJalali = dueDatesJalali[i];
                string paymentId = TransactionIdGenerator.GeneratePaymentId(eachInstallmentAmountWithoutZero, billId, $"20{i}");
                InstallmentRequestDataOutputDto otherinstallment = new(eachInstallmentAmountWithoutZero, dueDateJalali, paymentId, i);
                data.Add(otherinstallment);
            }

            return data;
        }
        private IEnumerable<InstallmentRequestDataOutputDto> GetCashInstallments(string billId, string dueDateJalali, long payable)
        {
            ICollection<InstallmentRequestDataOutputDto> data = new List<InstallmentRequestDataOutputDto>();
            data.Add(new InstallmentRequestDataOutputDto(payable, dueDateJalali, TransactionIdGenerator.GeneratePaymentId(payable, billId, $"200"), 0));

            return data;
        }
        private async Task Validate(InstallmentRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task<IEnumerable<GhestInsertDto>> GetGhestsInsertDto(IEnumerable<InstallmentRequestDataOutputDto> data, TrackingOutputDto trackingInfo, MoshtrakOutputDto moshtrakInfo)
        {
            int counter = 0;
            decimal[] barges = await _variabService.GetAndRenew(trackingInfo.ZoneId, data?.Count() ?? 0);
            ICollection<GhestInsertDto> ghests = new List<GhestInsertDto>();
            data.ForEach(x =>
            {
                GhestInsertDto ghest = new()
                {
                    ZoneId = trackingInfo.ZoneId,
                    CustomerNumber = moshtrakInfo.CustomerNumber,
                    StringTrackNumber = trackingInfo.StringTrackNumber,
                    Identify = 0,
                    Cod1 = 0,
                    Cod2 = 0,
                    Cod3 = x.Amount,
                    Barge = barges[counter],
                    Payable = x.Amount,
                    Type = counter == 0 ? _prePaymentType : _installmentType,
                    InstallmentNumber = counter,
                    CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                    DueDateJalali = x.DueDateJalali,
                    InsertBy = _insertBy,
                    BillId = trackingInfo.BillId ?? string.Empty,
                    PaymentId = x.PaymentId,
                };
                ghests.Add(ghest);
                counter++;
            });

            return ghests;
        }
        private string[] GetDate(int installmentCount, int monthlyDuration)
        {
            string dateJalali = DateTime.Now.ToShortPersianDateString();


            string[] dateJalaliItems = dateJalali.Split('/');
            short day = Convert.ToInt16(dateJalaliItems[2]);

            string formalDay = "01";
            if (day > 25 || day <= 5)
                formalDay = "05";
            else if (day > 5 && day <= 15)
                formalDay = "15";
            else if (day > 15 && day <= 25)
                formalDay = "25";
            string firstDateJalali = $@"{dateJalaliItems[0]}/{dateJalaliItems[1]}/{formalDay}";

            string[] dueDatesJalali = new string[installmentCount];
            dueDatesJalali[0] = firstDateJalali;
            int month = Convert.ToInt32(dateJalaliItems[1]);
            for (int i = 1; i < installmentCount; i++)
            {
                month = month + monthlyDuration;
                if (month > 12)
                {
                    month = month - 12;
                    dateJalaliItems[0] = (Convert.ToInt32(dateJalaliItems[0]) + 1).ToString();
                }

                string formalMonth = month.ToString();
                if (formalMonth.Length != 2)
                {
                    formalMonth = $@"0{month.ToString()}";
                }
                string date = $@"{dateJalaliItems[0]}/{formalMonth}/{formalDay}";
                dueDatesJalali[i] = date;
            }
            return dueDatesJalali;
        }
    }
}
