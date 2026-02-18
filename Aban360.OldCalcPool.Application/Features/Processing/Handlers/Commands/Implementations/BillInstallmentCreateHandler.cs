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

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class BillInstallmentCreateHandler : AbstractBaseConnection, IBillInstallmentCreateHandler
    {
        private readonly IMembersQueryService _membersQueryService;
        private readonly IBedBesQueryService _bedBesQueryService;
        private readonly IVariabService _variabService;
        private readonly IValidator<GhestAbInputDto> _validator;
        private const int _operator = 5;
        private const int _deadLineDay = 30;
        public BillInstallmentCreateHandler(
            IMembersQueryService membersQueryService,
            IBedBesQueryService bedBesQueryService,
            IVariabService variabService,
            IValidator<GhestAbInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _membersQueryService = membersQueryService;
            _membersQueryService.NotNull(nameof(membersQueryService));

            _bedBesQueryService = bedBesQueryService;
            _bedBesQueryService.NotNull(nameof(bedBesQueryService));

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
            BedBesWithAmountOutputDto bedBesInfo = await _bedBesQueryService.GetLatest(zoneIdCustomerNumber);
            ICollection<BillInstallmentCreateDto> installments = await GetInstallment(bedBesInfo, memberInfo, input);

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

            return GetResult(installments, memberInfo, bedBesInfo);
        }
        private ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto> GetResult(ICollection<BillInstallmentCreateDto> installment, MemberGetDto memberInfo, BedBesWithAmountOutputDto bedBesInfo)
        {
            BillInstallmentHeaderOutputDto header = new()
            {
                FullName = memberInfo.FullName,
                ZoneTitle = memberInfo.ZoneTitle,
                UsageTitle = memberInfo.UsageTitle,
                PreviousDateJalali = bedBesInfo.PreviousDateJalali,
                CurrentDateJalali = bedBesInfo.CurrentDateJalali,
                PreviousMeter = bedBesInfo.PreviousMeterNumber,
                CurrentMeter = bedBesInfo.CurrentMeterNumber,
                Consumption = bedBesInfo.Consumption,
                Payable = bedBesInfo.Payable,
                RegisterDateJalali = bedBesInfo.RegisterDateJalali
            };
            IEnumerable<BillInstallmentDataOutputDto> data = installment.Select(s =>
            {
                return new BillInstallmentDataOutputDto()
                {
                    DeadLineDateJalali = s.DeadLineDateJalali,
                    Payable = s.Payable,
                    QueueNumber = s.QueueNumber
                };
            });

            return new ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto>("اقساط آب‌بها", header, data);
        }
        private async Task<ICollection<BillInstallmentCreateDto>> GetInstallment(BedBesWithAmountOutputDto bedBesInfo, MemberGetDto memberInfo, GhestAbInputDto input)
        {
            ICollection<BillInstallmentCreateDto> allInstallments = new List<BillInstallmentCreateDto>();
            decimal[] rangeBarge = input.IsConfirmed ? await _variabService.GetAndRenew(memberInfo.ZoneId, input.InstallmentCount) : Array.Empty<decimal>();
            DateTime currentDate = DateTime.Now;
            long amount = bedBesInfo.Payable / input.InstallmentCount;
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
