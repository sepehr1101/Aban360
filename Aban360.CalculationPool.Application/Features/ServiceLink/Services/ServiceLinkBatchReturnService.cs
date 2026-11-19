using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using FluentValidation;
using Aban360.Common.Exceptions;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Implementations;
using Aban360.CalculationPool.Application.Features.ServiceLink.Services;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Services
{
    public interface IServiceLinkBatchReturnService
    {
        Task ServiceLinkRemoveUnconfirmedAsync(ServiceLinkRemoveUnconfirmedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task ServiceLinkReturnConfirmeAsync(string billId, IAppUser appUser, CancellationToken cancellationToken);
        Task ServiceLinkTempReturnAsync(ServiceLinkTempReturnInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task<ReportOutput<ServiceLinkUnconfirmedHeaderOutputDto, ServiceLinkUnconfirmedDataOutputDto>> ServiceLinkUnconfirmedGetAsync(string billId, IAppUser appUser, CancellationToken cancellationToken);
    }

    internal sealed class ServiceLinkBatchReturnService : AbstractBaseConnection, IServiceLinkBatchReturnService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IKartQueryService _kartQueryService;
        private readonly IT100QueryService _t100QueryService;
        private readonly IDiscountTypeQueryService _discountTypeQueryService;
        private readonly IModifyTypeQueryService _modifyTypeQueryService;
        static int[] _ServiceLinkReturnConfirmeallowedMultipleAmount = [_ServiceLinkReturnConfirmereturnTypeCodeInRequestBillDetail, _ServiceLinkReturnConfirmecreditTypeCodeInRequestBillDetail];
        const int _ServiceLinkReturnConfirmereturnTypeCodeInRequestBillDetail = 4;
        const int _ServiceLinkReturnConfirmecreditTypeCodeInRequestBillDetail = 6;
        private readonly IVariabService _variabService;
        private readonly IValidator<ServiceLinkTempReturnInputDto> _validator;
        const string _ServiceLinkTempReturninsertBy = "Aban";
        const int _ServiceLinkTempReturnmanualSerial = 10000;
        const int _ServiceLinkTempReturnoperator = 666;
        const int _ServiceLinkTempReturnkartTypeId = 2;
        const int _ServiceLinkTempReturnnoDiscountTypeId = 0;
        private readonly IServiceLinkReturnService _serviceLinkReturnHandler;
        private string _ServiceLinkUnconfirmedGettitle = ReportLiterals.ServiceLinkUnconfirmed;
        public ServiceLinkBatchReturnService(IHttpContextAccessor contextAccessor, ICommonMemberQueryService commonMemberQueryService, ICommonZoneService commonZoneService, IKartQueryService kartQueryService, IConfiguration configuration, IT100QueryService t100QueryService, IDiscountTypeQueryService discountTypeQueryService, IModifyTypeQueryService modifyTypeQueryService, IVariabService variabService, IValidator<ServiceLinkTempReturnInputDto> validator, IServiceLinkReturnService serviceLinkReturnHandler) : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));
            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
            _commonZoneService = commonZoneService;
            _commonMemberQueryService.NotNull(nameof(commonZoneService));
            _kartQueryService = kartQueryService;
            _kartQueryService.NotNull(nameof(kartQueryService));
            _commonZoneService.NotNull(nameof(commonZoneService));
            _t100QueryService = t100QueryService;
            _t100QueryService.NotNull(nameof(t100QueryService));
            _discountTypeQueryService = discountTypeQueryService;
            _discountTypeQueryService.NotNull(nameof(discountTypeQueryService));
            _modifyTypeQueryService = modifyTypeQueryService;
            _modifyTypeQueryService.NotNull(nameof(modifyTypeQueryService));
            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
            _serviceLinkReturnHandler = serviceLinkReturnHandler;
            _serviceLinkReturnHandler.NotNull(nameof(serviceLinkReturnHandler));
        }

        public async Task ServiceLinkRemoveUnconfirmedAsync(ServiceLinkRemoveUnconfirmedInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
            KartRemoveByIdDto kartRemoveDto = new(inputDto.Id, inputDto.ZoneId, inputDto.CustomerNumber);
            await ServiceLinkRemoveUnconfirmedExecSql(kartRemoveDto);
        }

        private async Task ServiceLinkRemoveUnconfirmedExecSql(KartRemoveByIdDto kartRemoveDto)
        {
            //string dbName = "Atlas";
            string dbName = GetDbName(kartRemoveDto.ZoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    KartCommandService kartCommandService = new(connection, transaction);
                    await kartCommandService.Remove(kartRemoveDto, dbName);
                    transaction.Commit();
                }
            }
        }

        public async Task ServiceLinkReturnConfirmeAsync(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _commonZoneService.IsUserInZone(appUser, memberInfo.ZoneId);
            IEnumerable<KartGetDto> kartsInfo = await _kartQueryService.GetAll(memberInfo.CustomerNumber, memberInfo.ZoneId);
            IEnumerable<KartRemoveByIdDto> kartRemoveDto = kartsInfo.Select(k => new KartRemoveByIdDto(k.Id, k.ZoneId, k.CustomerNumber)).ToList();
            IEnumerable<KartInsertDto> kartsInsertDto = ServiceLinkReturnConfirmeGetKartInsertDto(kartsInfo);
            IEnumerable<RequestBillDetailsInsertDto> requestsBillDetailInsertDto = await ServiceLinkReturnConfirmeGetRequestBillDetailsInsertDto(kartsInsertDto, memberInfo);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkReturnConfirmedOpLog, memberInfo.BillId, kartsInsertDto?.Count() ?? 0);
            await ServiceLinkReturnConfirmeExecSql(kartRemoveDto, kartsInsertDto, requestsBillDetailInsertDto, appUser, opLogText);
        }

        private IEnumerable<KartInsertDto> ServiceLinkReturnConfirmeGetKartInsertDto(IEnumerable<KartGetDto> kartsInfo)
        {
            return kartsInfo.Select(k => new KartInsertDto() { ZoneId = k.ZoneId, CustomerNumber = k.CustomerNumber, ReadingNumber = k.ReadingNumber, StringTrackNumber = k.StringTrackNumber, Serial = k.Serial, Barge = k.Barge, CurrentDateJalali = k.CurrentDateJalali, DueDateJalali = k.DueDateJalali, DiscountTypeId = k.DiscountTypeId, FinalAmount = k.Amount, Amount = k.Amount, DiscountAmount = k.DiscountAmount, PardN = k.PardN, PardG = k.PardG, Sum = k.Sum, AmountItemId = k.AmountItemId, SiphonId = k.SiphonId, UsageId = k.UsageId, IsRegister = k.IsRegister, TotalServicesAmount = k.TotalServicesAmount, FirstInstallment = k.FirstInstallment, JGEST_FA = k.JGEST_FA, PishFa = k.PishFa, InstallmentPercent = k.InstallmentPercent, Operator = k.Operator, DomesticUnit = k.DomesticUnit, CommercialUnit = k.CommercialUnit, OtherUnit = k.OtherUnit, KartTypeId = k.KartTypeId, InsertedBy = k.InsertedBy, BankDateJalali = k.BankDateJalali, Installment = k.Installment, InstallmentCount = k.InstallmentCount, MeterDiameterId = k.MeterDiameterId, Ser = k.Ser, Type = k.Type, });
        }

        private async Task<IEnumerable<RequestBillDetailsInsertDto>> ServiceLinkReturnConfirmeGetRequestBillDetailsInsertDto(IEnumerable<KartInsertDto> items, MemberInfoGetDto memberInfo)
        {
            ICollection<RequestBillDetailsInsertDto> result = new List<RequestBillDetailsInsertDto>();
            foreach (var item in items)
            {
                ModifyTypeGetDto modifyTypeInfo = await _modifyTypeQueryService.GetByKarten75(item.Type);
                long finalAmount = _ServiceLinkReturnConfirmeallowedMultipleAmount.Contains(modifyTypeInfo.RequestBillDetailsId) ? -1 * item.TotalServicesAmount : item.TotalServicesAmount;
                result.Add(new RequestBillDetailsInsertDto() { TrackNumber = item.StringTrackNumber, ZoneId = item.ZoneId, CustomerNumber = item.CustomerNumber, BillId = memberInfo.BillId, TypeId = modifyTypeInfo.Title, TypeCode = modifyTypeInfo.RequestBillDetailsId, ItemId = item.AmountItemId, ItemTitle = (await _t100QueryService.Get(item.AmountItemId, true)).Title, Amount = item.FinalAmount, OffAmount = item.DiscountAmount, OffTitle = (await _discountTypeQueryService.Get((DiscountTypeEnum)item.DiscountTypeId)).Title, FinalAmount = finalAmount, RegisterDate = item.CurrentDateJalali, ZoneTitle = memberInfo.ZoneTitle, UsageId = memberInfo.UsageId, UsageTitle = memberInfo.UsageTitle, PayId = string.Empty, //
 CommercialCount = memberInfo.CommercialUnit, DomesticCount = memberInfo.DomesticUnit, OtherCount = memberInfo.OtherUnit, ContractualCapacity = memberInfo.ContractualCapacity, });
            }

            return result;
        }

        private async Task ServiceLinkReturnConfirmeExecSql(IEnumerable<KartRemoveByIdDto> kartRemoveDto, IEnumerable<KartInsertDto> kartsInsertDto, IEnumerable<RequestBillDetailsInsertDto> requestBillDetailsInsertDto, IAppUser appUser, string opLogText)
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
                    await kartCommandService.Remove(kartRemoveDto, dbName);
                    await requestBillDetailCommandService.Insert(requestBillDetailsInsertDto);
                    await opLogCommandService.Insert(opLogText, appUser);
                    transaction.Commit();
                }
            }
        }

        public async Task ServiceLinkTempReturnAsync(ServiceLinkTempReturnInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await ServiceLinkTempReturnValidate(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumbere = await _commonMemberQueryService.Get(inputDto.BillId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumbere);
            await _commonZoneService.IsUserInZone(appUser, memberInfo.ZoneId);
            decimal barge = await _variabService.GetAndRenew(memberInfo.ZoneId);
            KartInsertDto kartsInsertDto = ServiceLinkTempReturnGetKartInsertDto(inputDto, memberInfo, (int)barge);
            await ServiceLinkTempReturnExecSql(kartsInsertDto, appUser);
        }

        private async Task ServiceLinkTempReturnExecSql(KartInsertDto kartsInsertDto, IAppUser appUser)
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
                    await kartCommandService.Insert(kartsInsertDto, false, dbName);
                    transaction.Commit();
                }
            }
        }

        private async Task ServiceLinkTempReturnValidate(ServiceLinkTempReturnInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }

        private KartInsertDto ServiceLinkTempReturnGetKartInsertDto(ServiceLinkTempReturnInputDto input, MemberInfoGetDto memberInfo, int barge)
        {
            bool hasDiscount = input.DiscountTypeId == _ServiceLinkTempReturnnoDiscountTypeId ? false : true;
            return new KartInsertDto()
            {
                ZoneId = memberInfo.ZoneId,
                CustomerNumber = memberInfo.CustomerNumber,
                ReadingNumber = memberInfo.ReadingNumber,
                StringTrackNumber = DateTime.Now.ToShortPersianDateString(),
                Serial = _ServiceLinkTempReturnmanualSerial,
                Barge = barge,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                DueDateJalali = DateTime.Now.AddMonths(1).ToShortPersianDateString(),
                DiscountTypeId = hasDiscount ? input.DiscountTypeId : input.DescriptionCode,
                FinalAmount = input.Amount,
                Amount = input.Amount,
                DiscountAmount = 0,
                PardN = input.Amount,
                PardG = 0,
                Sum = input.Amount,
                AmountItemId = input.AmountItemId, //From T100
                SiphonId = ServiceLinkTempReturnGetSiphonId(int.Parse(memberInfo.MainSiphon)),
                UsageId = memberInfo.UsageId,
                IsRegister = false,
                TotalServicesAmount = input.Amount,
                FirstInstallment = input.Amount,
                JGEST_FA = 0,
                PishFa = 0,
                InstallmentPercent = 100,
                Operator = _ServiceLinkTempReturnoperator,
                DomesticUnit = memberInfo.DomesticUnit,
                CommercialUnit = memberInfo.CommercialUnit,
                OtherUnit = memberInfo.OtherUnit,
                KartTypeId = _ServiceLinkTempReturnkartTypeId,
                InsertedBy = _ServiceLinkTempReturninsertBy,
                BankDateJalali = string.Empty,
                Installment = 0,
                InstallmentCount = 1,
                MeterDiameterId = memberInfo.MeterDiameterId,
                Ser = 0,
                Type = (int)input.CategoryType,
            };
        }

        private int ServiceLinkTempReturnGetSiphonId(int mainSiphon)
        {
            return mainSiphon switch
            {
                100 => 1,
                125 => 2,
                150 => 3,
                200 => 4,
                _ => mainSiphon
            };
        }

        public async Task<ReportOutput<ServiceLinkUnconfirmedHeaderOutputDto, ServiceLinkUnconfirmedDataOutputDto>> ServiceLinkUnconfirmedGetAsync(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _commonZoneService.IsUserInZone(appUser, memberInfo.ZoneId);
            long amount = 0;
            IEnumerable<NumericDictionary> descriptionsInfo = _serviceLinkReturnHandler.ServiceLinkReturnReturnCodesAsync();
            IEnumerable<ServiceLinkUnconfirmedDataOutputDto> KartsData = await _kartQueryService.GetTodayInfoByCustomerNumber(memberInfo.CustomerNumber, memberInfo.ZoneId);
            KartsData.ForEach(k =>
            {
                NumericDictionary? descriptionInfo = descriptionsInfo.Where(d => d.Id == k.DescriptionCode).FirstOrDefault();
                k.DescriptionCode = descriptionInfo?.Id ?? 0;
                k.DescriptionTitle = descriptionInfo?.Title ?? string.Empty;
                long localAmount = k.Type == 4 ? (k.Amount) : (k.Amount * -1);
                amount += localAmount;
            });
            ServiceLinkUnconfirmedHeaderOutputDto header = new()
            {
                ZoneId = zoneIdAndCustomerNumber.ZoneId,
                ZoneTitle = memberInfo.ZoneTitle,
                CustomerNumber = memberInfo.CustomerNumber,
                BillId = billId,
                FullName = memberInfo.FullName,
                RecordCount = KartsData?.Count() ?? 0,
                Amount = amount,
                Title = _ServiceLinkUnconfirmedGettitle
            };
            return new ReportOutput<ServiceLinkUnconfirmedHeaderOutputDto, ServiceLinkUnconfirmedDataOutputDto>(_ServiceLinkUnconfirmedGettitle, header, KartsData);
        }
    }
}
