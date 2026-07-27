using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
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
using Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Implementations;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;

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
        private string _insertWayTitle = "رایاب";
        private string _insertBy = "Aban";
        private int _afterSaleRequestServiceId = 2;
        private int _operator = 666;
        private int _kartTypeId = 3;
        private int _type = 1;
        private float _taxPercent = 0.1f;
        private int _taxItemId = 550;
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
            await SqlCommands(kartsInsertDto, requestBillDetailsInsertDto, inputDto, memberInfo, appUser, opLogText);
            return GetResult(memberInfo, requestBillDetailsInsertDto);
        }
        private async Task SqlCommands(IEnumerable<KartInsertDto> kartsInsertDto, IEnumerable<RequestBillDetailsInsertDto> requestBillDetailsInsertDto, OtherExpensesInsertInputDto inputDto, MemberInfoGetDto memeberInfo, IAppUser appUser, string opLogText)
        {
            string dbName = GetDbName(kartsInsertDto?.FirstOrDefault()?.ZoneId ?? 0);
            //string dbName = "Atlas";

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    T0CommandService t0CommandService = new(connection, transaction);
                    MoshtrakCommandService moshtrakCommandService = new(connection, transaction);
                    KartCommandService kartCommandService = new(connection, transaction);
                    RequestBillDetailsCommandService requestBillDetailCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    int trackNumber = (int)(await t0CommandService.GetTrackNumber());
                    string stringTrackNumber = trackNumber.ToString().PadLeft(11, '0');
                    MoshtrakCreateDto moshtrakInsertDto = GetMoshtrackCreateDto(inputDto, memeberInfo, trackNumber);
                    foreach (var item in kartsInsertDto)
                    {
                        item.StringTrackNumber = stringTrackNumber;
                    }
                    foreach (var item in requestBillDetailsInsertDto)
                    {
                        item.TrackNumber = stringTrackNumber;
                    }

                    await moshtrakCommandService.Insert(moshtrakInsertDto, dbName);
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
                MobileNumber = memberInfo.MobileNumber,
                PaymentId = requestBillDetails?.FirstOrDefault()?.PayId ?? string.Empty,
                TrackNumber=requestBillDetails?.FirstOrDefault()?.TrackNumber??string.Empty,
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
        private MoshtrakCreateDto GetMoshtrackCreateDto(OtherExpensesInsertInputDto inputDto, MemberInfoGetDto memberInfo, int trackNumber)//todo
        {
            MoshtrakServiceDto serviceSelected = MoshtrakService.GetServicesSelected(new List<int> { (int)inputDto.Offering });
            return new MoshtrakCreateDto()
            {
                TrackNumber = trackNumber,
                ServiceGroupId = _afterSaleRequestServiceId,
                StringTrackNumber = trackNumber.ToString().PadLeft(11, '0'),
                BillId = memberInfo.BillId,
                CustomerNumber = memberInfo.CustomerNumber,
                NeighbourBillId = null,
                ZoneId = memberInfo.ZoneId,
                NotificationMobile = memberInfo.MobileNumber,
                UsageId = memberInfo.UsageId,
                MeterDiameterId = memberInfo.MeterDiameterId,
                BranchTypeId = memberInfo.UseStateId,
                DiscountTypeId = memberInfo.DiscountId,
                DiscountCount = memberInfo.DiscountCount,
                PhoneNumber = memberInfo.PhoneNumber,
                MobileNumber = memberInfo.MobileNumber,
                NationalCode = memberInfo.NationalCode,
                FirstName = memberInfo.FirstName,
                Surname = memberInfo.Surname,
                FatherName = memberInfo.FatherName,
                Premises = memberInfo.Premises,
                ImprovementCommertial = memberInfo.CommercialImprovement,
                ImprovementDomestic = memberInfo.DomesticImprovement,
                ImprovementOverall = memberInfo.OverallImprovement,
                Siphon100 = memberInfo.Siphon100,
                Siphon125 = memberInfo.Siphon125,
                Siphon150 = memberInfo.Siphon150,
                Siphon200 = memberInfo.Siphon200,
                MainSiphon = int.Parse(memberInfo.MainSiphon),
                CommonSiphon = memberInfo.CommonSiphon1,
                ContractualCapacity = memberInfo.ContractualCapacity,
                HouseValue = 0,//todo
                CommertialUnit = memberInfo.CommercialUnit,
                DomesticUnit = memberInfo.DomesticUnit,
                OtherUnit = memberInfo.OtherUnit,
                IsNonPermanent = false,
                Address = memberInfo.Address,
                PreViewId = string.Empty,//todo
                CounterType = memberInfo.DeletionStateId,
                InstallAgentState = 0,//todo
                BlockId = string.IsNullOrWhiteSpace(memberInfo.BlockCode) ? string.Empty : memberInfo.BlockCode,
                InsertWayTitle = _insertWayTitle,
                PostalCode = memberInfo.PostalCode,
                IsSpecial = memberInfo.IsSpecial,
                ReadingNumber = memberInfo.ReadingNumber,
                CertificateNumber = string.Empty,
                BrokerId = 0,//todo
                s0 = serviceSelected.s0,
                s1 = serviceSelected.s1,
                s2 = serviceSelected.s2,
                s3 = serviceSelected.s3,
                s4 = serviceSelected.s4,
                s5 = serviceSelected.s5,
                s8 = serviceSelected.s8,
                s9 = serviceSelected.s9,
                s10 = serviceSelected.s10,
                s11 = serviceSelected.s11,
                s12 = serviceSelected.s12,
                s13 = serviceSelected.s13,
                s14 = serviceSelected.s14,
                s15 = serviceSelected.s15,
                s16 = serviceSelected.s16,
                s17 = serviceSelected.s17,
                s18 = serviceSelected.s18,
                s19 = serviceSelected.s19,
                s20 = serviceSelected.s20,
                s21 = serviceSelected.s21,
                s22 = serviceSelected.s22,
                s23 = serviceSelected.s23,
                s24 = serviceSelected.s24,
                s25 = serviceSelected.s25,
                s26 = serviceSelected.s26,
                s27 = serviceSelected.s27,
                s28 = serviceSelected.s28,
                s29 = serviceSelected.s29,
                s30 = serviceSelected.s30,
                s31 = serviceSelected.s31,
                s32 = serviceSelected.s32,
                s33 = serviceSelected.s33,
                s34 = serviceSelected.s34,
                s35 = serviceSelected.s35,
                s36 = serviceSelected.s36,
                s37 = serviceSelected.s37,
                s38 = serviceSelected.s38,
                s39 = serviceSelected.s39,
                s40 = serviceSelected.s40,
                s41 = serviceSelected.s41,
                s42 = serviceSelected.s42,
                s43 = serviceSelected.s43,
                s44 = serviceSelected.s44,
                s45 = serviceSelected.s45,
                s46 = serviceSelected.s46,
                s47 = serviceSelected.s47,
                s48 = serviceSelected.s48,
            };
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
