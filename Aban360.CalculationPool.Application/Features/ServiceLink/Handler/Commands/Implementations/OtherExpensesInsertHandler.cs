using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class OtherExpensesInsertHandler : AbstractBaseConnection, IOtherExpensesInsertHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _memberQueryService;
        private readonly ICommonZoneService _zoneService;
        private readonly IKartQueryService _kartQueryService;
        private readonly IVariabService _variabService;
        private readonly IModifyTypeQueryService _modifyTypeQueryService;
        private readonly IT100QueryService _t100QueryService;
        private readonly IValidator<OtherExpensesInsertInputDto> _validator;
        private string _currentDateJalali = DateTime.Now.ToShortPersianDateString();
        private string _title = ReportLiterals.ServiceLinkOtherExpenses;
        const string _insertBy = "Aban";
        const int _operator = 666;
        const int _kartTypeId = 3;
        const int _type = 1;
        const float _taxPercent = 0.1f;
        const int _taxItemId = 550;
        public OtherExpensesInsertHandler(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService memberQueryService,
            ICommonZoneService zoneService,
            IKartQueryService kartQueryService,
            IVariabService variabService,
            IModifyTypeQueryService modifyTypeQueryService,
            IT100QueryService t100QueryService,
            IValidator<OtherExpensesInsertInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _memberQueryService = memberQueryService;
            _memberQueryService.NotNull(nameof(memberQueryService));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _kartQueryService = kartQueryService;
            _kartQueryService.NotNull(nameof(kartQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _modifyTypeQueryService = modifyTypeQueryService;
            _modifyTypeQueryService.NotNull(nameof(modifyTypeQueryService));

            _t100QueryService = t100QueryService;
            _t100QueryService.NotNull(nameof(t100QueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto>> Handle(OtherExpensesInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, appUser, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumbere = await _memberQueryService.Get(inputDto.BillId);
            MemberInfoGetDto memberInfo = await _memberQueryService.Get(zoneIdAndCustomerNumbere);
            await _zoneService.IsUserInZone(appUser, memberInfo.ZoneId);
            decimal barge = await _variabService.GetAndRenew(memberInfo.ZoneId);

            IEnumerable<KartInsertDto> kartsInsertDto = GetKartInsertDto(inputDto, memberInfo, (int)barge);
            IEnumerable<RequestBillDetailsInsertDto> requestBillDetailsInsertDto = await GetRequestBillDetailsInsertDto(kartsInsertDto, memberInfo);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkOtherExpensesOpLog, inputDto.BillId, inputDto.Amount, kartsInsertDto?.Sum(s => s.FinalAmount) ?? 0);
            await SqlCommands(kartsInsertDto, requestBillDetailsInsertDto, appUser, opLogText);
            return GetResult(memberInfo, requestBillDetailsInsertDto);
        }
        private async Task SqlCommands(IEnumerable<KartInsertDto> kartsInsertDto, IEnumerable<RequestBillDetailsInsertDto> requestBillDetailsInsertDto, IAppUser appUser, string opLogText)
        {
            string dbName = GetDbName(kartsInsertDto?.FirstOrDefault()?.ZoneId ?? 0);
            //string dbName = "Atlas";

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    KartCommandService kartCommandService = new(connection, transaction);
                    RequestBillDetailsCommandService requestBillDetailCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await kartCommandService.Insert(kartsInsertDto, true, dbName);
                    await requestBillDetailCommandService.Insert(requestBillDetailsInsertDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto> GetResult(MemberInfoGetDto memberInfo, IEnumerable<RequestBillDetailsInsertDto> requestBillDetails)
        {
            OtherExpensesHeaderOutputDto header = new()
            {
                ZoneId = memberInfo.ZoneId,
                ZoneTitle = memberInfo.ZoneTitle,
                CustomerNumber = memberInfo.CustomerNumber,
                BillId = memberInfo.BillId,
                FirstName = memberInfo.FirstName,
                Surname = memberInfo.Surname,
                FullName = memberInfo.FullName,
                UsageId = memberInfo.UsageId,
                UsageTitle = memberInfo.UsageTitle,
                PaymentId = requestBillDetails?.FirstOrDefault()?.PayId ?? string.Empty,
                FinalAmount = requestBillDetails?.Sum(s => s.Amount) ?? 0,
                Title = _title,
                RecordCount = requestBillDetails?.Count() ?? 0,
            };
            ICollection<OtherExpensesDataOutputDto> data = new List<OtherExpensesDataOutputDto>();
            foreach (var item in requestBillDetails)
            {
                data.Add(new OtherExpensesDataOutputDto(item.ItemId, item.ItemTitle, item.Amount));
            }
            return new ReportOutput<OtherExpensesHeaderOutputDto, OtherExpensesDataOutputDto>(_title, header, data);
        }
        private async Task InputValidate(OtherExpensesInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private IEnumerable<KartInsertDto> GetKartInsertDto(OtherExpensesInsertInputDto input, MemberInfoGetDto memberInfo, int barge)
        {
            ICollection<KartInsertDto> karts = new List<KartInsertDto>();
            long offeringAmount = input.Amount;
            long taxAmount = (long)(input.Amount * _taxPercent);
            long totalAmount = offeringAmount + taxAmount;

            KartInsertDto offeringKart = new()
            {
                ZoneId = memberInfo.ZoneId,
                CustomerNumber = memberInfo.CustomerNumber,
                ReadingNumber = memberInfo.ReadingNumber,
                StringTrackNumber = _currentDateJalali,
                Serial = 0,
                Barge = barge,
                CurrentDateJalali = _currentDateJalali,
                DueDateJalali = DateTime.Now.AddMonths(1).ToShortPersianDateString(),
                DiscountTypeId = 0,
                FinalAmount = offeringAmount,
                DiscountAmount = 0,
                PardN = offeringAmount,
                PardG = 0,
                Sum = offeringAmount,
                AmountItemId = (int)input.Offering,//From T100
                SiphonId = int.Parse(memberInfo.MainSiphon),
                UsageId = memberInfo.UsageId,
                IsRegister = false,
                TotalServicesAmount = totalAmount,
                FirstInstallment = totalAmount,
                JGEST_FA = 0,
                PishFa = 0,
                InstallmentPercent = 100,
                Operator = _operator,
                DomesticUnit = memberInfo.DomesticUnit,
                CommercialUnit = memberInfo.CommercialUnit,
                OtherUnit = memberInfo.OtherUnit,
                KartTypeId = _kartTypeId,
                InsertedBy = _insertBy,
                BankDateJalali = string.Empty,
                Installment = 0,
                InstallmentCount = 1,
                MeterDiameterId = memberInfo.MeterDiameterId,
                Ser = 0,
                Type = _type,
            };
            KartInsertDto taxKart = new()
            {
                ZoneId = memberInfo.ZoneId,
                CustomerNumber = memberInfo.CustomerNumber,
                ReadingNumber = memberInfo.ReadingNumber,
                StringTrackNumber = _currentDateJalali,
                Serial = 0,
                Barge = barge,
                CurrentDateJalali = _currentDateJalali,
                DueDateJalali = DateTime.Now.AddMonths(1).ToShortPersianDateString(),
                DiscountTypeId = 0,
                FinalAmount = taxAmount,// amount,
                DiscountAmount = 0,//discountAmount,
                PardN = taxAmount,//amount,
                PardG = 0,
                Sum = taxAmount,
                AmountItemId = _taxItemId,//From T100
                SiphonId = int.Parse(memberInfo.MainSiphon),
                UsageId = memberInfo.UsageId,
                IsRegister = false,
                TotalServicesAmount = totalAmount,
                FirstInstallment = totalAmount,
                JGEST_FA = 0,
                PishFa = 0,
                InstallmentPercent = 100,
                Operator = _operator,
                DomesticUnit = memberInfo.DomesticUnit,
                CommercialUnit = memberInfo.CommercialUnit,
                OtherUnit = memberInfo.OtherUnit,
                KartTypeId = _kartTypeId,
                InsertedBy = _insertBy,
                BankDateJalali = string.Empty,
                Installment = 0,
                InstallmentCount = 1,
                MeterDiameterId = memberInfo.MeterDiameterId,
                Ser = 0,
                Type = _type,
            };

            karts.Add(offeringKart);
            karts.Add(taxKart);
            return karts;
        }
        private async Task<IEnumerable<RequestBillDetailsInsertDto>> GetRequestBillDetailsInsertDto(IEnumerable<KartInsertDto> items, MemberInfoGetDto memberInfo)
        {
            ICollection<RequestBillDetailsInsertDto> requests = new List<RequestBillDetailsInsertDto>();
            string paymentId = TransactionIdGenerator.GeneratePaymentId(items?.FirstOrDefault()?.TotalServicesAmount ?? 0, memberInfo.BillId, "200");
            foreach (var item in items)
            {
                ModifyTypeGetDto modifyTypeInfo = await _modifyTypeQueryService.GetByKarten75(item.Type);
                requests.Add(new RequestBillDetailsInsertDto()
                {
                    TrackNumber = item.StringTrackNumber,
                    ZoneId = item.ZoneId,
                    CustomerNumber = item.CustomerNumber,
                    BillId = memberInfo.BillId,
                    TypeId = modifyTypeInfo.Title,
                    TypeCode = modifyTypeInfo.RequestBillDetailsId,
                    ItemId = item.AmountItemId,
                    ItemTitle = (await _t100QueryService.Get(item.AmountItemId, true)).Title,
                    Amount = item.FinalAmount,
                    OffAmount = item.DiscountAmount,
                    OffTitle = string.Empty,
                    FinalAmount = item.TotalServicesAmount,
                    RegisterDate = item.CurrentDateJalali,
                    ZoneTitle = memberInfo.ZoneTitle,
                    UsageId = memberInfo.UsageId,
                    UsageTitle = memberInfo.UsageTitle,
                    PayId = paymentId,
                    CommercialCount = memberInfo.CommercialUnit,
                    DomesticCount = memberInfo.DomesticUnit,
                    OtherCount = memberInfo.OtherUnit,
                    ContractualCapacity = memberInfo.ContractualCapacity,
                });
            }
            return requests;
        }
    }
}
