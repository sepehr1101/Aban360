using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Constants;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.OldCalcPools.WaterReturn.Dto.Queries;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class BillInstallmentCreateHandler : AbstractBaseConnection, IBillInstallmentCreateHandler
    {
        private readonly IMembersQueryService _membersQueryService;
        private readonly IVariabService _variabService;
        private readonly IValidator<BillInstallmentInputDto> _validator;
        private const int _operator = 5;
        private const int _deadLineDay = 30;
        private const long _debtAmountLimit = 1000000;
        private const string _title = "اقساط آب‌بها";
        public BillInstallmentCreateHandler(
            IMembersQueryService membersQueryService,
            IVariabService variabService,
            IValidator<BillInstallmentInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _membersQueryService = membersQueryService;
            _membersQueryService.NotNull(nameof(membersQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto>> Handle(BillInstallmentInputDto input, CancellationToken cancellationToken)
        {
            await Validation(input, cancellationToken);
            MemberGetDto memberInfo = await GetMemberInfo(input.BillId);

            ZoneIdAndCustomerNumberOutputDto zoneIdCustomerNumber = new(memberInfo.ZoneId, memberInfo.CustomerNumber);
            ICollection<BillInstallmentCreateDto> installments = await GetInstallment(memberInfo, input);

            if (input.IsConfirm)
            {
                string dbName = GetDbName(memberInfo.ZoneId);
                using (IDbConnection connection = _sqlReportConnection)
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        GhestAbCommandService ghestAbCommandService = new(connection, transaction);
                        await ghestAbCommandService.Insert(installments, dbName);

                        transaction.Commit();
                    }
                }
            }

            return GetResult(installments, memberInfo);
        }
        private async Task<MemberGetDto> GetMemberInfo(string billId)
        {
            MemberGetDto memberInfo = await _membersQueryService.Get(billId);
            if (memberInfo.LatestDebt < _debtAmountLimit)
            {
                throw new InvalidInstallmentException(Exceptionliterals.InvalidDebtlessThan100000);
            }

            return memberInfo;
        }
        private ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto> GetResult(ICollection<BillInstallmentCreateDto> installment, MemberGetDto memberInfo)
        {
            BillInstallmentHeaderOutputDto header = new()
            {
                FullName = memberInfo.FullName,
                ZoneTitle = memberInfo.ZoneTitle,
                UsageTitle = memberInfo.UsageTitle,
                Payable = memberInfo.LatestDebt,
                BillId = memberInfo.BillId,
                MobileNumber = memberInfo.MobileNumber,
                NationalCode = memberInfo.NationalCode,
                PhoneNumber = memberInfo.PhoneNumber,
                Title = _title,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = installment?.Count() ?? 0,
            };
            IEnumerable<BillInstallmentDataOutputDto> data = installment.Select(s =>
            {
                return new BillInstallmentDataOutputDto()
                {
                    DeadLineDateJalali = s.DeadLineDateJalali,
                    Payable = s.Payable,
                    QueueNumber = s.QueueNumber,
                    BillId = memberInfo.BillId,
                    PaymentId = TransactionIdGenerator.GeneratePaymentId(s.Payable, memberInfo.BillId, $"00{s.QueueNumber}"),
                    QueueNumberTitle = $"قسط {s.QueueNumber.NumberToText(Language.Persian)}"
                };
            });

            return new ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto>(_title, header, data);
        }
        private async Task<ICollection<BillInstallmentCreateDto>> GetInstallment(MemberGetDto memberInfo, BillInstallmentInputDto input)
        {
            ICollection<BillInstallmentCreateDto> allInstallments = new List<BillInstallmentCreateDto>();
            decimal[] rangeBarge = input.IsConfirm ? await _variabService.GetAndRenew(memberInfo.ZoneId, input.InstallmentCount) : Array.Empty<decimal>();
            DateTime currentDate = DateTime.Now;

            long firstPayment = (long)(memberInfo.LatestDebt * input.PrepaymentPercent / 100.0);
            long otherPaymentTotal = memberInfo.LatestDebt - firstPayment;
            long eachInstallmentWithZero = otherPaymentTotal / (input.InstallmentCount - 1);
            long eachInstallmentWithoutZero = (eachInstallmentWithZero / 1000) * 1000;
            long remained = otherPaymentTotal - (eachInstallmentWithoutZero * (input.InstallmentCount - 1));
            firstPayment += remained;

            allInstallments.Add(new BillInstallmentCreateDto()
            {
                ZoneId = memberInfo.ZoneId,
                CustomerNumber = memberInfo.CustomerNumber,
                ReadingNumber = memberInfo.ReadingNumber,
                Barge = input.IsConfirm ? (int)rangeBarge[0] : 0,
                DeadLineDateJalali = currentDate.AddDays(_deadLineDay).FormatDateToShortPersianDate(),
                Payable = firstPayment,
                UsageId = memberInfo.UsageId,
                MeterDiameterId = memberInfo.MeterDiamterId,
                QueueNumber = 1,
                Operator = _operator
            });
            for (int i = 2; i <= input.InstallmentCount; i++)
            {
                int deadLineDay = _deadLineDay * i;
                BillInstallmentCreateDto ghestAdDto = new BillInstallmentCreateDto()
                {
                    ZoneId = memberInfo.ZoneId,
                    CustomerNumber = memberInfo.CustomerNumber,
                    ReadingNumber = memberInfo.ReadingNumber,
                    Barge = input.IsConfirm ? (int)rangeBarge[i - 1] : 0,
                    DeadLineDateJalali = currentDate.AddDays(deadLineDay).FormatDateToShortPersianDate(),
                    Payable = eachInstallmentWithoutZero,
                    UsageId = memberInfo.UsageId,
                    MeterDiameterId = memberInfo.MeterDiamterId,
                    QueueNumber = i,
                    Operator = _operator
                };
                allInstallments.Add(ghestAdDto);
            }

            return allInstallments;
        }
        private async Task Validation(BillInstallmentInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
