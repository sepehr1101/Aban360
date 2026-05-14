using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Constant;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Implementations
{
    internal sealed class BillInstallmentManualCreateHandler : AbstractBaseConnection, IBillInstallmentManualCreateHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IGhestAbQueryService _ghestAbQueryService;
        private readonly IVariabService _variabService;
        private readonly IValidator<BillInstallmentManualInputDto> _validator;
        private const int _operator = 5;
        private const string _title = "اقساط آب‌بها";
        public BillInstallmentManualCreateHandler(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService commonMemberQueryService,
            IGhestAbQueryService ghestAbQueryService,
            IVariabService variabService,
            IValidator<BillInstallmentManualInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _ghestAbQueryService = ghestAbQueryService;
            _ghestAbQueryService.NotNull( nameof(ghestAbQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(validator));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto>> Handle(BillInstallmentManualInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validation(input, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(input.BillId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);

            ICollection<BillInstallmentCreateDto> allInstallments = await GetInstallments(memberInfo, input);
            string logText = string.Format(Literals.BillInstallmentManualOpLog, memberInfo.BillId, allInstallments?.Sum(x => x.Payable) ?? 0, input.Installments?.Count ?? 0, allInstallments?.FirstOrDefault()?.Payable ?? 0);
            if (input.IsConfirm)
            {
                await SqlCommands(memberInfo, zoneIdAndCustomerNumber, allInstallments, logText, appUser);
            }

            return GetResult(allInstallments, memberInfo);
        }
        private async Task Validation(BillInstallmentManualInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task<ICollection<BillInstallmentCreateDto>> GetInstallments(MemberInfoGetDto memberInfo, BillInstallmentManualInputDto input)
        {
            decimal[] rangeBarge = input.IsConfirm ? await _variabService.GetAndRenew(memberInfo.ZoneId, input.Installments?.Count() ?? 0) : Array.Empty<decimal>();
            ICollection<BillInstallmentCreateDto> allInstallments = new List<BillInstallmentCreateDto>();
            for (int i = 0; i < input.Installments.Count; i++)
            {
                allInstallments.Add(new BillInstallmentCreateDto()
                {
                    ZoneId = memberInfo.ZoneId,
                    CustomerNumber = memberInfo.CustomerNumber,
                    ReadingNumber = memberInfo.ReadingNumber,
                    Barge = input.IsConfirm ? (int)rangeBarge[i] : 0,
                    DeadLineDateJalali = input.Installments?.ElementAt(i)?.DueDateJalali ?? string.Empty,
                    Payable = input.Installments?.ElementAt(i).Amount ?? 0,
                    UsageId = memberInfo.UsageId,
                    MeterDiameterId = memberInfo.MeterDiameterId,
                    QueueNumber = i + 1,
                    Operator = _operator,
                });
            }

            return allInstallments;
        }
        private async Task SqlCommands(MemberInfoGetDto memberInfo, ZoneIdAndCustomerNumber zoneIdCustomerNumber, ICollection<BillInstallmentCreateDto> installments, string logText, IAppUser appUser)
        {
            await DuplicateValidation(zoneIdCustomerNumber);
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
                    OpLogCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await ghestAbCommandService.Insert(installments, dbName);
                    await opLogCommandService.Insert(logText, appUser);

                    transaction.Commit();
                }
            }
        }
        private ReportOutput<BillInstallmentHeaderOutputDto, BillInstallmentDataOutputDto> GetResult(ICollection<BillInstallmentCreateDto> installment, MemberInfoGetDto memberInfo)
        {
            BillInstallmentHeaderOutputDto header = new()
            {
                FullName = memberInfo.FullName,
                ZoneTitle = memberInfo.ZoneTitle,
                UsageTitle = memberInfo.UsageTitle,
                Payable = installment?.Sum(p => p.Payable) ?? 0,
                BillId = memberInfo.BillId,
                MobileNumber = memberInfo.MobileNumber,
                NationalCode = memberInfo.NationalCode,
                PhoneNumber = memberInfo.PhoneNumber,
                Title = _title,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = installment?.Count() ?? 0,

                CommercialUnit = memberInfo.CommercialUnit,
                DomesticUnit = memberInfo.DomesticUnit,
                OtherUnit = memberInfo.OtherUnit,
                EmptyUnit = memberInfo.EmptyUnit,
                MeterDiameterId = memberInfo.MeterDiameterId,
                MeterDiameterTitle = memberInfo.MeterDiameterTitle,
                PostalCode = memberInfo.PostalCode,
                ReadingNumber = memberInfo.ReadingNumber,
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
        private async Task DuplicateValidation(ZoneIdAndCustomerNumber input)
        {
            IEnumerable<BillInstallmentOutputDto> currantInstallments = await _ghestAbQueryService.Get(input, DateTime.Now.ToShortPersianDateString());
            if (currantInstallments.Any())
            {
                throw new InvalidInstallmentException(ExceptionLiterals.InvalidDuplicateInstallment(currantInstallments?.FirstOrDefault()?.InsertedBy ?? "-", currantInstallments?.Count() ?? 0, currantInstallments?.FirstOrDefault()?.RegisterDateJalali ?? "-"));
            }
        }
    }
}
