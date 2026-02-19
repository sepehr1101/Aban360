using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPools.Persistence.Features.WaterReturn.Queries.Contracts;
using Aban360.OldCalcPools.WaterReturn.Dto.Queries;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;
using DNTPersianUtils.Core;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class BillInstallmentCreateHandler : AbstractBaseConnection, IBillInstallmentCreateHandler
    {
        private readonly IMembersQueryService _membersQueryService;
        private readonly IVariabService _variabService;
        private readonly IValidator<GhestAbInputDto> _validator;
        private const int _operator = 5;
        private const int _deadLineDay = 30;
        public BillInstallmentCreateHandler(
            IMembersQueryService membersQueryService,
            IVariabService variabService,
            IValidator<GhestAbInputDto> validator,
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

        public async Task<ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto>> Handle(GhestAbInputDto input, CancellationToken cancellationToken)
        {
            await Validation(input, cancellationToken);
            MemberGetDto memberInfo = await _membersQueryService.Get(input.BillId);
            ZoneIdAndCustomerNumberOutputDto zoneIdCustomerNumber = new(memberInfo.ZoneId, memberInfo.CustomerNumber);
            ICollection<BillInstallmentCreateDto> installments = await GetInstallment(memberInfo, input);

            if (input.IsConfirmed)
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
                PhoneNumber = memberInfo.PhoneNumber
            };
            IEnumerable<BillInstallmentDataOutputDto> data = installment.Select(s =>
            {
                return new BillInstallmentDataOutputDto()
                {
                    DeadLineDateJalali = s.DeadLineDateJalali,
                    Payable = s.Payable,
                    QueueNumber = s.QueueNumber,
                    BillId = memberInfo.BillId,
                    PaymentId = TransactionIdGenerator.GeneratePaymentId(s.Payable,memberInfo.BillId,$"00{s.QueueNumber}"),
                    QueueNumberTitle = $"قسط {s.QueueNumber.NumberToText(Language.Persian)}"
                };
            });

            return new ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto>("اقساط آب‌بها", header, data);
        }
        private async Task<ICollection<BillInstallmentCreateDto>> GetInstallment(MemberGetDto memberInfo, GhestAbInputDto input)
        {
            ICollection<BillInstallmentCreateDto> allInstallments = new List<BillInstallmentCreateDto>();
            decimal[] rangeBarge = input.IsConfirmed ? await _variabService.GetAndRenew(memberInfo.ZoneId, input.InstallmentCount) : Array.Empty<decimal>();
            DateTime currentDate = DateTime.Now;
            long amount = memberInfo.LatestDebt / input.InstallmentCount;
            long installmenAmount = (amount / 1000) * 1000;

            for (int i = 1; i <= input.InstallmentCount; i++)
            {
                int deadLineDay = _deadLineDay * i;
                BillInstallmentCreateDto ghestAdDto = new BillInstallmentCreateDto()
                {
                    ZoneId = memberInfo.ZoneId,
                    CustomerNumber = memberInfo.CustomerNumber,
                    ReadingNumber = memberInfo.ReadingNumber,
                    Barge = input.IsConfirmed ? (int)rangeBarge[i - 1] : 0,
                    DeadLineDateJalali = currentDate.AddDays(deadLineDay).FormatDateToShortPersianDate(),
                    Payable = installmenAmount,
                    UsageId = memberInfo.UsageId,
                    MeterDiameterId = memberInfo.MeterDiamterId,
                    QueueNumber = i,
                    Operator = _operator
                };
                allInstallments.Add(ghestAdDto);
            }

            return allInstallments;
        }
        private async Task Validation(GhestAbInputDto inputDto, CancellationToken cancellationToken)
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
