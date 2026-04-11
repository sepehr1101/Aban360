using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
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
        private readonly IValidator<InstallmentRequestInputDto> _validator;
        static int _intervalDueDate = 30;
        static int _calculationConfirmedStatus = 60;
        static string _title = "تقسیط";
        static string _insertBy = "Aban";
        public SetInstallmentRequestHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IKartQueryService kartQueryService,
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

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto>> Handle(InstallmentRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await GetTrackingWithValidation(inputDto, cancellationToken);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            IEnumerable<CalculationRequestDisplayDataOutputDto> kartInfo = await _kartQueryService.Get(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);

            IEnumerable<InstallmentRequestDataOutputDto> data = GetInstallments(inputDto, kartInfo);
            InstallmentRequestHeaderOutputDto header = new(data?.Sum(x => x.Amount) ?? 0, inputDto.InstallmentCount);
            ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto> result = new(_title, header, data);

            IEnumerable<GhestInsertDto> ghestsInsertDto = GetGhestsInsertDto(data, trackingInfo, moshtrakInfo);
            await ExecuteSqlCommand(ghestsInsertDto, trackingInfo.ZoneId);

            return result;
        }
        private async Task<TrackingOutputDto> GetTrackingWithValidation(InstallmentRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            if (trackingInfo.StatusId != _calculationConfirmedStatus)
            {
                // throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }

            return trackingInfo;
        }
        private (long, long, long, long) GetInstallmentAmount(InstallmentRequestInputDto inputDto, IEnumerable<CalculationRequestDisplayDataOutputDto> kartInfo)
        {
            long amount = kartInfo?.Sum(x => x.Amount) ?? 0;
            long discount = kartInfo?.Sum(x => x.Discount) ?? 0;
            long payable = amount - discount;//todo: true or not?

            long firstInstallmentWithZero = (long)(payable * (inputDto.PrepaymentPercent / 100.0));
            long firstInstallmentWithoutZero = (firstInstallmentWithZero / 1000) * 1000;

            long otherAmount = payable - firstInstallmentWithoutZero;
            long eachInstallmentAmountWithZero = otherAmount / (inputDto.InstallmentCount - 1);
            long eachInstallmentAmountWithoutZero = (eachInstallmentAmountWithZero / 1000) * 1000;
            long remain = otherAmount - (eachInstallmentAmountWithoutZero * (inputDto.InstallmentCount - 1));

            return (payable, firstInstallmentWithoutZero, eachInstallmentAmountWithoutZero, remain);
        }
        private IEnumerable<InstallmentRequestDataOutputDto> GetInstallments(InstallmentRequestInputDto inputDto, IEnumerable<CalculationRequestDisplayDataOutputDto> kartInfo)
        {
            int dailyInterval = 30 * inputDto.MonthlyDuration;
            var (payable, firstInstallmentWithoutZero, eachInstallmentAmountWithoutZero, remain) = GetInstallmentAmount(inputDto, kartInfo);
            InstallmentRequestDataOutputDto firstInstallment = new(firstInstallmentWithoutZero + remain, DateTime.Now.AddDays(dailyInterval).ToShortPersianDateString(), string.Empty);
            ICollection<InstallmentRequestDataOutputDto> data = new List<InstallmentRequestDataOutputDto>(); ;
            data.Add(firstInstallment);
            for (int i = 2; i <= inputDto.InstallmentCount; i++)
            {
                string dueDateJalali = DateTime.Now.AddDays(i * dailyInterval).ToShortPersianDateString();
                InstallmentRequestDataOutputDto otherinstallment = new(eachInstallmentAmountWithoutZero, dueDateJalali, string.Empty);
                data.Add(otherinstallment);
            }

            return data;
        }
        private async Task InputValidation(InstallmentRequestInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private IEnumerable<GhestInsertDto> GetGhestsInsertDto(IEnumerable<InstallmentRequestDataOutputDto> data, TrackingOutputDto trackingInfo, MoshtrakOutputDto moshtrakInfo)
        {
            if (string.IsNullOrWhiteSpace(trackingInfo.BillId))
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
            }
            int counter = 0;
            return data.Select(x => new GhestInsertDto()
            {
                ZoneId = trackingInfo.ZoneId,
                CustomerNumber = moshtrakInfo.CustomerNumber,
                StringTrackNumber = trackingInfo.StringTrackNumber,
                Identify = 0,//todo
                Cod1 = 0,//todo
                Cod2 = 0,//todo
                Cod3 = 0,//todo
                Barge = 0,//todo
                Payable = x.Amount,
                Type = 0,//todo
                InstallmentNumber = counter,
                CurrentDateJalali = DateTime.Now.ToShortDateString(),
                DueDateJalali = x.DueDateJalali,
                InsertBy = _insertBy,
                BillId = trackingInfo.BillId ?? string.Empty,
                PaymentId = TransactionIdGenerator.GeneratePaymentId(x.Amount, trackingInfo.BillId, $"00{counter++}"),
            });
        }
        private async Task ExecuteSqlCommand(IEnumerable<GhestInsertDto> ghestsInsertDto, int zoneId)
        {
            string dbName = GetDbName(zoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    GhestCommandService ghestCommandService = new(connection, transaction);

                    await ghestCommandService.Remove(ghestsInsertDto.FirstOrDefault().StringTrackNumber, dbName);
                    await ghestCommandService.Insert(ghestsInsertDto, dbName);

                    transaction.Commit();
                }
            }
        }
    }
}
