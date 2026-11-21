using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations;
using Aban360.Common.Exceptions;
using FluentValidation;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.Common.Literals;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Services
{
    public interface IServiceLinkReturnService
    {
        Task<ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto>> ServiceLinkReturnDisconnectAsync(ServiceLinkReturnDisconnectInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task ServiceLinkReturnAsync(ServiceLinkReturnInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        IEnumerable<NumericDictionary> ServiceLinkReturnReturnCodesAsync();
        Task ServiceLinkReturnRemoveAsync(ServiceLinkReturnRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }

    internal sealed class ServiceLinkReturnService : AbstractBaseConnection, IServiceLinkReturnService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly ISaleGetHandler _saleGetHandler;
        private readonly IVariabService _variabService;
        private readonly IModifyTypeQueryService _modifyTypeQueryService;
        private readonly IDiscountTypeQueryService _discountTypeQueryService;
        private readonly IT100QueryService _t100QueryService;
        static string _ServiceLinkReturnDisconnectinsertBy = "Aban";
        static string _ServiceLinkReturnDisconnecttitle = "برگشتی حق انشعاب-جمع آوری انشعاب";
        static float _ServiceLinkReturnDisconnectreturnMultiplier = 0.5f;
        static short _ServiceLinkReturnDisconnectwaterSubscriptionId = 101;
        static short _ServiceLinkReturnDisconnectsewageSubscriptionId = 201;
        static short[] _ServiceLinkReturnDisconnectallowedOfferingIds =
        {
            _ServiceLinkReturnDisconnectwaterSubscriptionId,
            _ServiceLinkReturnDisconnectsewageSubscriptionId
        };
        static int _ServiceLinkReturnDisconnectmanualSerial = 10000;
        static int _ServiceLinkReturnDisconnectoperator = 666;
        static int _ServiceLinkReturnDisconnectkartTypeId = 8;
        static int _ServiceLinkReturnDisconnectreturnCategoryType = 3; //returnCategoryId=3
        static int _ServiceLinkReturnDisconnectcollectBranchDeletionState = 1;
        private readonly IValidator<ServiceLinkReturnInputDto> _validator;
        const string _ServiceLinkReturninsertBy = "Aban";
        static int[] _ServiceLinkReturnallowedMultipleAmount = [_ServiceLinkReturnreturnTypeCodeInRequestBillDetail, _ServiceLinkReturncreditTypeCodeInRequestBillDetail];
        const int _ServiceLinkReturnreturnTypeCodeInRequestBillDetail = 4;
        const int _ServiceLinkReturncreditTypeCodeInRequestBillDetail = 6;
        const int _ServiceLinkReturnmanualSerial = 10000;
        const int _ServiceLinkReturnoperator = 666;
        const int _ServiceLinkReturnkartTypeId = 2;
        const int _ServiceLinkReturndiscountDescriptionCode = 14;
        private readonly IRequestBillDetailQueryService _requestBillDetailQueryService;
        public ServiceLinkReturnService(IHttpContextAccessor contextAccessor, ICommonMemberQueryService commonMemberQueryService, ICommonZoneService commonZoneService, ISaleGetHandler saleGetHandler, IVariabService variabService, IModifyTypeQueryService modifyTypeQueryService, IDiscountTypeQueryService discountTypeQueryService, IT100QueryService t100QueryService, IConfiguration configuration, IValidator<ServiceLinkReturnInputDto> validator, IRequestBillDetailQueryService requestBillDetailQueryService) : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));
            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
            _saleGetHandler = saleGetHandler;
            _saleGetHandler.NotNull(nameof(saleGetHandler));
            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));
            _modifyTypeQueryService = modifyTypeQueryService;
            _modifyTypeQueryService.NotNull(nameof(modifyTypeQueryService));
            _discountTypeQueryService = discountTypeQueryService;
            _discountTypeQueryService.NotNull(nameof(discountTypeQueryService));
            _t100QueryService = t100QueryService;
            _t100QueryService.NotNull(nameof(t100QueryService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
            _requestBillDetailQueryService = requestBillDetailQueryService;
            _requestBillDetailQueryService.NotNull(nameof(requestBillDetailQueryService));
        }

        public async Task<ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto>> ServiceLinkReturnDisconnectAsync(ServiceLinkReturnDisconnectInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            memberInfo.DiscountId = inputDto.DiscountTypeId;
            memberInfo.DiscountCount = inputDto.DiscountCount;
            ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto> subscriptionAmounts = await ServiceLinkReturnDisconnectGetSubscriptionAmount(memberInfo, inputDto, cancellationToken);
            if (!inputDto.IsConfirm)
            {
                return subscriptionAmounts;
            }

            decimal barge = await _variabService.GetAndRenew(memberInfo.ZoneId);
            IEnumerable<KartInsertDto> kartsInsertDto = ServiceLinkReturnDisconnectGetKartInsertDto(subscriptionAmounts, inputDto, memberInfo, (int)barge);
            ICollection<RequestBillDetailsInsertDto> requestBillDetailsInsertDto = await ServiceLinkReturnDisconnectGetRequestBillDetailsInsertDto(kartsInsertDto, memberInfo);
            CustomerDeletionStateUpdateDto customerUpdateDto = new(memberInfo.Id, memberInfo.ZoneId, memberInfo.CustomerNumber, memberInfo.BillId, _ServiceLinkReturnDisconnectcollectBranchDeletionState);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkReturnDisconnectOpLog, inputDto.BillId, subscriptionAmounts.ReportHeader.Amount);
            await ServiceLinkReturnDisconnectSqlCommands(kartsInsertDto, requestBillDetailsInsertDto, customerUpdateDto, zoneIdAndCustomerNumber, appUser, opLogText);
            return subscriptionAmounts;
        }

        private async Task<ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto>> ServiceLinkReturnDisconnectGetSubscriptionAmount(MemberInfoGetDto memberInfo, ServiceLinkReturnDisconnectInputDto inputDto, CancellationToken cancellationToken)
        {
            SaleInputDto saleInputDto = new()
            {
                WaterDiameterId = (short)memberInfo.MeterDiameterId,
                SiphonDiameterId = short.Parse(memberInfo.MainSiphon),
                Block = inputDto.BlockCode,
                CommertialUnit = memberInfo.CommercialUnit,
                DomesticUnit = memberInfo.DomesticUnit,
                OtherUnit = memberInfo.OtherUnit,
                ContractualCapacity = memberInfo.ContractualCapacity,
                DiscountCount = memberInfo.DiscountCount,
                DiscountTypeId = memberInfo.DiscountId,
                HasSewageArticle11 = false,
                HasWaterArticle11 = false,
                HasWaterBroker = false,
                IsSewageDiscount = inputDto.DiscountTypeId == 0 ? false : true,
                IsWaterDiscount = inputDto.DiscountTypeId == 0 ? false : true,
                UsageId = memberInfo.UsageId,
                ZoneId = memberInfo.ZoneId,
            };
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> saleResult = await _saleGetHandler.Handle(saleInputDto, cancellationToken);
            IEnumerable<ServiceLinkReturnDisconnectDataOutputDto> data = saleResult.ReportData?.Where(s => _ServiceLinkReturnDisconnectallowedOfferingIds.Contains(s.Id)).Select(s => new ServiceLinkReturnDisconnectDataOutputDto(s.Id, s.Title, s.Amount, s.Discount, (long)(s.FinalAmount * _ServiceLinkReturnDisconnectreturnMultiplier), s.DiscountTypeId)).ToList();
            ServiceLinkReturnDisconnectHeaderOutputDto header = new(inputDto.BillId, _ServiceLinkReturnDisconnecttitle, data?.Count() ?? 0, data?.Sum(d => d.FinalAmount) ?? 0);
            return new ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto>(_ServiceLinkReturnDisconnecttitle, header, data);
        }

        private async Task ServiceLinkReturnDisconnectSqlCommands(IEnumerable<KartInsertDto> kartsInsertDto, IEnumerable<RequestBillDetailsInsertDto> requestBillDetailsInsertDto, CustomerDeletionStateUpdateDto customerUpdateDto, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, IAppUser appUser, string opLogText)
        {
            //string dbName = GetDbName(memberInfo.ZoneId);
            string dbName = "Atlas";
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    KartCommandService kartCommandService = new(connection, transaction);
                    RequestBillDetailsCommandService requestBillDetailCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    ArchMemCommandService _archMemCommandService = new(connection, transaction);
                    MembersCommandService _membersCommandService = new(connection, transaction);
                    ClientsCommandService _clientCommandService = new(connection, transaction);
                    await kartCommandService.Insert(kartsInsertDto, true, dbName);
                    await requestBillDetailCommandService.Insert(requestBillDetailsInsertDto);
                    int rowId = await _archMemCommandService.Insert(customerUpdateDto, dbName);
                    await _membersCommandService.Update(customerUpdateDto, dbName);
                    await _clientCommandService.UpdateToDayJalali(zoneIdAndCustomerNumber, customerUpdateDto.ToDayDateJalali);
                    await _clientCommandService.InsertByArchMemId(rowId, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);
                    transaction.Commit();
                }
            }
        }

        private IEnumerable<KartInsertDto> ServiceLinkReturnDisconnectGetKartInsertDto(ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto> subscriptionAmounts, ServiceLinkReturnDisconnectInputDto input, MemberInfoGetDto memberInfo, int barge)
        {
            return subscriptionAmounts.ReportData.Select(s => new KartInsertDto() { ZoneId = memberInfo.ZoneId, CustomerNumber = memberInfo.CustomerNumber, ReadingNumber = memberInfo.ReadingNumber, StringTrackNumber = DateTime.Now.ToShortPersianDateString(), Serial = _ServiceLinkReturnDisconnectmanualSerial, Barge = barge, CurrentDateJalali = DateTime.Now.ToShortPersianDateString(), DueDateJalali = DateTime.Now.AddMonths(1).ToShortPersianDateString(), DiscountTypeId = memberInfo.DiscountId, FinalAmount = s.FinalAmount, DiscountAmount = s.Discount, PardN = s.Amount, PardG = 0, Sum = s.Amount, AmountItemId = s.Id, SiphonId = int.Parse(memberInfo.MainSiphon), UsageId = memberInfo.UsageId, IsRegister = false, TotalServicesAmount = subscriptionAmounts.ReportHeader.Amount, FirstInstallment = 0, //
 JGEST_FA = 0, PishFa = 0, InstallmentPercent = 100, //
 Operator = _ServiceLinkReturnDisconnectoperator, DomesticUnit = memberInfo.DomesticUnit, CommercialUnit = memberInfo.CommercialUnit, OtherUnit = memberInfo.OtherUnit, KartTypeId = _ServiceLinkReturnDisconnectkartTypeId, InsertedBy = _ServiceLinkReturnDisconnectinsertBy, BankDateJalali = string.Empty, Installment = 0, //
 InstallmentCount = 1, //
 MeterDiameterId = memberInfo.MeterDiameterId, Ser = 0, Type = _ServiceLinkReturnDisconnectreturnCategoryType, }).ToList();
        }

        private async Task<ICollection<RequestBillDetailsInsertDto>> ServiceLinkReturnDisconnectGetRequestBillDetailsInsertDto(IEnumerable<KartInsertDto> kartItems, MemberInfoGetDto memberInfo)
        {
            ModifyTypeGetDto modifyTypeDto = await _modifyTypeQueryService.GetByKarten75(_ServiceLinkReturnDisconnectreturnCategoryType);
            ICollection<RequestBillDetailsInsertDto> requestBills = new List<RequestBillDetailsInsertDto>();
            foreach (var k in kartItems)
            {
                requestBills.Add(new RequestBillDetailsInsertDto() { TrackNumber = k.StringTrackNumber, ZoneId = k.ZoneId, CustomerNumber = k.CustomerNumber, BillId = memberInfo.BillId, TypeId = modifyTypeDto.Title, TypeCode = modifyTypeDto.RequestBillDetailsId, ItemId = k.AmountItemId, ItemTitle = (await _t100QueryService.Get(k.AmountItemId, true)).Title, Amount = k.TotalServicesAmount, OffAmount = k.DiscountAmount, OffTitle = (await _discountTypeQueryService.Get((DiscountTypeEnum)k.DiscountTypeId)).Title, FinalAmount = k.FinalAmount * -1, //returnType must * -1
 RegisterDate = k.CurrentDateJalali, ZoneTitle = memberInfo.ZoneTitle, UsageId = memberInfo.UsageId, UsageTitle = memberInfo.UsageTitle, PayId = string.Empty, //
 CommercialCount = memberInfo.CommercialUnit, DomesticCount = memberInfo.DomesticUnit, OtherCount = memberInfo.OtherUnit, ContractualCapacity = memberInfo.ContractualCapacity, });
            }

            return requestBills;
        }

        public async Task ServiceLinkReturnAsync(ServiceLinkReturnInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await ServiceLinkReturnValidate(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumbere = await _commonMemberQueryService.Get(inputDto.BillId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumbere);
            decimal barge = await _variabService.GetAndRenew(memberInfo.ZoneId);
            KartInsertDto kartsInsertDto = ServiceLinkReturnGetKartInsertDto(inputDto, memberInfo, (int)barge);
            RequestBillDetailsInsertDto requestBillDetailsInsertDto = await ServiceLinkReturnGetRequestBillDetailsInsertDto(kartsInsertDto, memberInfo);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkReturnOpLog, inputDto.BillId, kartsInsertDto.FinalAmount);
            await ServiceLinkReturnSqlCommands(kartsInsertDto, requestBillDetailsInsertDto, appUser, opLogText);
        }

        private async Task ServiceLinkReturnSqlCommands(KartInsertDto kartsInsertDto, RequestBillDetailsInsertDto requestBillDetailsInsertDto, IAppUser appUser, string opLogText)
        {
            string dbName = GetDbName(kartsInsertDto.ZoneId);
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

        private async Task ServiceLinkReturnValidate(ServiceLinkReturnInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }

        private KartInsertDto ServiceLinkReturnGetKartInsertDto(ServiceLinkReturnInputDto input, MemberInfoGetDto memberInfo, int barge)
        {
            bool hasDiscountAmount = _ServiceLinkReturndiscountDescriptionCode == input.DescriptionCode;
            long amount = hasDiscountAmount ? 0 : input.Amount;
            long discountAmount = hasDiscountAmount ? input.Amount : 0;
            return new KartInsertDto()
            {
                ZoneId = memberInfo.ZoneId,
                CustomerNumber = memberInfo.CustomerNumber,
                ReadingNumber = memberInfo.ReadingNumber,
                StringTrackNumber = DateTime.Now.ToShortPersianDateString(),
                Serial = _ServiceLinkReturnmanualSerial,
                Barge = barge,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                DueDateJalali = DateTime.Now.AddMonths(1).ToShortPersianDateString(),
                DiscountTypeId = input.DiscountTypeId,
                FinalAmount = input.Amount,
                Amount = input.Amount, // amount,
                DiscountAmount = 0, //discountAmount,
                PardN = input.Amount, //amount,
                PardG = 0,
                Sum = input.Amount,
                AmountItemId = input.AmountItemId, //From T100
                SiphonId = int.Parse(memberInfo.MainSiphon),
                UsageId = memberInfo.UsageId,
                IsRegister = false,
                TotalServicesAmount = input.Amount,
                FirstInstallment = input.Amount,
                JGEST_FA = 0,
                PishFa = 0,
                InstallmentPercent = 100,
                Operator = _ServiceLinkReturnoperator,
                DomesticUnit = memberInfo.DomesticUnit,
                CommercialUnit = memberInfo.CommercialUnit,
                OtherUnit = memberInfo.OtherUnit,
                KartTypeId = _ServiceLinkReturnkartTypeId,
                InsertedBy = _ServiceLinkReturninsertBy,
                BankDateJalali = string.Empty,
                Installment = 0,
                InstallmentCount = 1,
                MeterDiameterId = memberInfo.MeterDiameterId,
                Ser = 0,
                Type = (int)input.CategoryType,
            };
        }

        private async Task<RequestBillDetailsInsertDto> ServiceLinkReturnGetRequestBillDetailsInsertDto(KartInsertDto item, MemberInfoGetDto memberInfo)
        {
            ModifyTypeGetDto modifyTypeInfo = await _modifyTypeQueryService.GetByKarten75(item.Type);
            long finalAmount = _ServiceLinkReturnallowedMultipleAmount.Contains(modifyTypeInfo.RequestBillDetailsId) ? -1 * item.TotalServicesAmount : item.TotalServicesAmount;
            return new RequestBillDetailsInsertDto()
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
                OffTitle = (await _discountTypeQueryService.Get((DiscountTypeEnum)item.DiscountTypeId)).Title,
                FinalAmount = finalAmount,
                RegisterDate = item.CurrentDateJalali,
                ZoneTitle = memberInfo.ZoneTitle,
                UsageId = memberInfo.UsageId,
                UsageTitle = memberInfo.UsageTitle,
                PayId = string.Empty, //
                CommercialCount = memberInfo.CommercialUnit,
                DomesticCount = memberInfo.DomesticUnit,
                OtherCount = memberInfo.OtherUnit,
                ContractualCapacity = memberInfo.ContractualCapacity,
            };
        }

        public IEnumerable<NumericDictionary> ServiceLinkReturnReturnCodesAsync()
        {
            return new List<NumericDictionary>()
            {
                new NumericDictionary(1, "از آب‌بها/نقل به"),
                new NumericDictionary(2, "اشتباه ثبت مبلغ"),
                new NumericDictionary(3, "تکرار مبلغ"),
                new NumericDictionary(4, "اشتباه ثبت ردیف"),
                new NumericDictionary(5, "صدور چک"),
                new NumericDictionary(6, "از قلم افتادگی"),
                new NumericDictionary(7, "از ردیف به ردیف"),
                new NumericDictionary(8, "اصلاح حساب"),
                new NumericDictionary(9, "عدم امکان فنی"),
                new NumericDictionary(10, "اصلاح ردیف نامعتبر"),
                new NumericDictionary(11, "پرداخت خسارت بخش آب"),
                new NumericDictionary(12, "پرداخت خسارت بخش فاضلاب"),
                new NumericDictionary(13, "برگشت چک"),
                new NumericDictionary(14, "تخفیف"),
            };
        }

        public async Task ServiceLinkReturnRemoveAsync(ServiceLinkReturnRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            RequestBillDetailGetDto requestBillDetailInfo = await _requestBillDetailQueryService.Get(inputDto.Id, inputDto.BillId);
            await ServiceLinkReturnRemoveDateValidate(requestBillDetailInfo);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkReturnRemoved, requestBillDetailInfo.BillId, requestBillDetailInfo.RegisterDateJalali, requestBillDetailInfo.Amount, requestBillDetailInfo.TypeTitle, requestBillDetailInfo.ItemTitle);
            RequestBillDetailsRemoveDto requestBillDetailRemoveDto = new(inputDto.Id, inputDto.BillId);
            ModifyTypeGetDto modifyType = await _modifyTypeQueryService.GetByRequestBillDetails(requestBillDetailInfo.TypeCode);
            KartRemoveByConditionDto kartRemoveDto = new(requestBillDetailInfo.ZoneId, requestBillDetailInfo.CustomerNumber, requestBillDetailInfo.Amount, requestBillDetailInfo.RegisterDateJalali, modifyType.Karten75Id, requestBillDetailInfo.ItemId);
            await ServiceLinkReturnRemoveSqlCommands(requestBillDetailRemoveDto, kartRemoveDto, appUser, opLogText);
        }

        private async Task ServiceLinkReturnRemoveSqlCommands(RequestBillDetailsRemoveDto requestBillDetailRemoveDto, KartRemoveByConditionDto kartRemoveDto, IAppUser appUser, string opLogText)
        {
            string dbName = GetDbName(kartRemoveDto.ZoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    KartCommandService kartCommandService = new(connection, transaction);
                    RequestBillDetailsCommandService requestBillDetailCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);
                    await kartCommandService.Remove(kartRemoveDto, dbName);
                    await requestBillDetailCommandService.Remove(requestBillDetailRemoveDto);
                    await opLogCommandService.Insert(opLogText, appUser);
                    transaction.Commit();
                }
            }
        }

        private async Task ServiceLinkReturnRemoveDateValidate(RequestBillDetailGetDto requestBillDetailInfo)
        {
            string dateCheck = await _variabService.GetDateCheck(requestBillDetailInfo.ZoneId);
            if (requestBillDetailInfo.RegisterDateJalali.CompareTo(dateCheck) <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidServiceLinkReturnRemoveBeforDateCheck);
            }

            if (DateTime.Now.AddDays(-7).ToShortPersianDateString().CompareTo(requestBillDetailInfo.RegisterDateJalali) > 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidServiceLinkReturnRemoveLessThan7Days);
            }
        }
    }
}
