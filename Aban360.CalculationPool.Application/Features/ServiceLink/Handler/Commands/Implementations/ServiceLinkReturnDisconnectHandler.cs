using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Constant;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class ServiceLinkReturnDisconnectHandler : AbstractBaseConnection, IServiceLinkReturnDisconnectHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly ISaleGetHandler _saleGetHandler;
        private readonly IVariabService _variabService;
        private readonly IModifyTypeQueryService _modifyTypeQueryService;
        private readonly IDiscountTypeQueryService _discountTypeQueryService;
        private readonly IT100QueryService _t100QueryService;
        static int[] _allowedMultipleAmount = { 2, 3, 5 };
        private string _insertBy = "Aban";
        private float _returnMultiplier = 0.5f;
        private short _waterSubscriptionId = 101;
        private short _sewageSubscriptionId = 201;//todo: need?
        private int _manualSerial = 10000;
        private int _operator = 666;
        private int _kartTypeId = 8;
        private int _categoryType = -1;//todo

        public ServiceLinkReturnDisconnectHandler(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            ISaleGetHandler saleGetHandler,
            IVariabService variabService,
            IModifyTypeQueryService modifyTypeQueryService,
            IDiscountTypeQueryService discountTypeQueryService,
            IT100QueryService t100QueryService,
            IConfiguration configuration)
            : base(configuration)
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
        }

        public async Task<ServiceLinkReturnDisconnectOutputDto> Handle(ServiceLinkReturnDisconnectInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);

            long subscriptionAmount = await GetSubscriptionAmount(memberInfo, cancellationToken);
            long returnAmount = (long)(subscriptionAmount * _returnMultiplier);
            if (!inputDto.IsConfirm)
            {
                return GetResult(inputDto, returnAmount);
            }
            decimal barge = await _variabService.GetAndRenew(memberInfo.ZoneId);

            KartInsertDto kartsInsertDto = GetKartInsertDto(inputDto, memberInfo, returnAmount, (int)barge);
            RequestBillDetailsInsertDto requestBillDetailsInsertDto = await GetRequestBillDetailsInsertDto(kartsInsertDto, memberInfo);
            string opLogText = string.Format(Literals.ServiceLinkReturnDisconnectOpLog, inputDto.BillId, kartsInsertDto.FinalAmount);
            await SqlCommands(kartsInsertDto, requestBillDetailsInsertDto, appUser, opLogText);

            return GetResult(inputDto, returnAmount);
        }
        private async Task<long> GetSubscriptionAmount(MemberInfoGetDto memberInfo, CancellationToken cancellationToken)
        {
            SaleInputDto saleInputDto = new()
            {
                WaterDiameterId = (short)memberInfo.MeterDiameterId,
                SiphonDiameterId = short.Parse(memberInfo.MainSiphon),
                Block = memberInfo.BlockCode,
                CommertialUnit = memberInfo.CommercialUnit,
                DomesticUnit = memberInfo.DomesticUnit,
                OtherUnit = memberInfo.OtherUnit,
                ContractualCapacity = memberInfo.ContractualCapacity,
                DiscountCount = memberInfo.DiscountCount,
                DiscountTypeId = memberInfo.DiscountId,
                HasSewageArticle11 = false,
                HasWaterArticle11 = false,
                HasWaterBroker = false,
                IsSewageDiscount = false,
                IsWaterDiscount = memberInfo.DiscountId != 0 ? true : false,
                UsageId = memberInfo.UsageId,
                ZoneId = memberInfo.ZoneId,
            };
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> saleResult = await _saleGetHandler.Handle(saleInputDto, cancellationToken);

            return saleResult.ReportData?.Where(s => s.Id == _waterSubscriptionId).FirstOrDefault()?.FinalAmount ?? 0;
        }
        private ServiceLinkReturnDisconnectOutputDto GetResult(ServiceLinkReturnDisconnectInputDto inputDto, long amount)
        {
            return new ServiceLinkReturnDisconnectOutputDto()
            {
                BillId = inputDto.BillId,
                Amount = amount,
            };
        }
        private async Task SqlCommands(KartInsertDto kartsInsertDto, RequestBillDetailsInsertDto requestBillDetailsInsertDto, IAppUser appUser, string opLogText)
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
                    OpLogCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await kartCommandService.Insert(kartsInsertDto, true, dbName);
                    await requestBillDetailCommandService.Insert(requestBillDetailsInsertDto);
                    await opLogCommandService.Insert(opLogText, appUser);
                    //members->deletionStat=5

                    transaction.Commit();
                }
            }
        }
        private KartInsertDto GetKartInsertDto(ServiceLinkReturnDisconnectInputDto input, MemberInfoGetDto memberInfo, long returnAmount, int barge)
        {
            return new KartInsertDto()
            {
                ZoneId = memberInfo.ZoneId,
                CustomerNumber = memberInfo.CustomerNumber,
                ReadingNumber = memberInfo.ReadingNumber,
                StringTrackNumber = DateTime.Now.ToShortPersianDateString(),
                Serial = _manualSerial,
                Barge = barge,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                DueDateJalali = DateTime.Now.AddMonths(1).ToShortPersianDateString(),
                DiscountTypeId = memberInfo.DiscountId,
                FinalAmount = returnAmount,
                DiscountAmount = 0,
                PardN = returnAmount,
                PardG = 0,
                Sum = returnAmount,
                AmountItemId = _waterSubscriptionId,//todo: waterOrSewage?
                SiphonId = int.Parse(memberInfo.MainSiphon),
                UsageId = memberInfo.UsageId,
                IsRegister = false,
                TotalServicesAmount = returnAmount,
                FirstInstallment = returnAmount,
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
                Type = _categoryType,//todo
            };
        }
        private async Task<RequestBillDetailsInsertDto> GetRequestBillDetailsInsertDto(KartInsertDto item, MemberInfoGetDto memberInfo)
        {
            ModifyTypeGetDto modifyTypeInfo = await _modifyTypeQueryService.GetByKarten75(item.Type);
            long finalAmount = _allowedMultipleAmount.Contains(item.Type) ? -1 * item.FinalAmount : item.FinalAmount;
            return new RequestBillDetailsInsertDto()
            {
                TrackNumber = item.StringTrackNumber,
                ZoneId = item.ZoneId,
                CustomerNumber = item.CustomerNumber,
                BillId = memberInfo.BillId,
                TypeId = modifyTypeInfo.Title,
                TypeCode = modifyTypeInfo.RequestBillDetailsId,
                ItemId = item.AmountItemId,
                ItemTitle = (await _t100QueryService.Get(item.AmountItemId)).Title,
                Amount = item.TotalServicesAmount,
                OffAmount = item.DiscountAmount,
                OffTitle = (await _discountTypeQueryService.Get((DiscountTypeEnum)item.DiscountTypeId)).Title,
                FinalAmount = finalAmount,
                RegisterDate = item.CurrentDateJalali,
                ZoneTitle = memberInfo.ZoneTitle,
                UsageId = memberInfo.UsageId,
                UsageTitle = memberInfo.UsageTitle,
                PayId = string.Empty,//
                CommercialCount = memberInfo.CommercialUnit,
                DomesticCount = memberInfo.DomesticUnit,
                OtherCount = memberInfo.OtherUnit,
                ContractualCapacity = memberInfo.ContractualCapacity,
            };
        }
    }
}
