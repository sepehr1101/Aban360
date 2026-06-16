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
        static string _insertBy = "Aban";
        static string _title = "برگشتی حق انشعاب-جمع آوری انشعاب";
        static float _returnMultiplier = 0.5f;
        static short _waterSubscriptionId = 101;
        static short _sewageSubscriptionId = 201;
        static short[] _allowedOfferingIds = { _waterSubscriptionId, _sewageSubscriptionId };
        static int _manualSerial = 10000;
        static int _operator = 666;
        static int _kartTypeId = 8;
        static int _returnCategoryType = 3;//returnCategoryId=3
        static int _collectBranchDeletionState = 1;

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

        public async Task<ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto>> Handle(ServiceLinkReturnDisconnectInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(inputDto.BillId);
            await _commonZoneService.IsUserInZone(appUser, zoneIdAndCustomerNumber.ZoneId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            memberInfo.DiscountId = inputDto.DiscountTypeId;
            memberInfo.DiscountCount = inputDto.DiscountCount;

            ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto> subscriptionAmounts = await GetSubscriptionAmount(memberInfo, inputDto, cancellationToken);
            if (!inputDto.IsConfirm)
            {
                return subscriptionAmounts;
            }
            decimal barge = await _variabService.GetAndRenew(memberInfo.ZoneId);

            IEnumerable<KartInsertDto> kartsInsertDto = GetKartInsertDto(subscriptionAmounts, inputDto, memberInfo, (int)barge);
            ICollection<RequestBillDetailsInsertDto> requestBillDetailsInsertDto = await GetRequestBillDetailsInsertDto(kartsInsertDto, memberInfo);
            CustomerDeletionStateUpdateDto customerUpdateDto = new(memberInfo.Id, memberInfo.ZoneId, memberInfo.CustomerNumber, memberInfo.BillId, _collectBranchDeletionState);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkReturnDisconnectOpLog, inputDto.BillId, subscriptionAmounts.ReportHeader.Amount);
            await SqlCommands(kartsInsertDto, requestBillDetailsInsertDto, customerUpdateDto, zoneIdAndCustomerNumber, appUser, opLogText);

            return subscriptionAmounts;
        }
        private async Task<ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto>> GetSubscriptionAmount(MemberInfoGetDto memberInfo, ServiceLinkReturnDisconnectInputDto inputDto, CancellationToken cancellationToken)
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

            IEnumerable<ServiceLinkReturnDisconnectDataOutputDto> data = saleResult.ReportData?.Where(s => _allowedOfferingIds.Contains(s.Id)).Select(s => new ServiceLinkReturnDisconnectDataOutputDto(s.Id, s.Title, s.Amount, s.Discount, (long)(s.FinalAmount * _returnMultiplier), s.DiscountTypeId)).ToList();
            ServiceLinkReturnDisconnectHeaderOutputDto header = new(inputDto.BillId, _title, data?.Count() ?? 0, data?.Sum(d => d.FinalAmount) ?? 0);
            return new ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto>(_title, header, data);
        }
        private async Task SqlCommands(IEnumerable<KartInsertDto> kartsInsertDto, IEnumerable<RequestBillDetailsInsertDto> requestBillDetailsInsertDto, CustomerDeletionStateUpdateDto customerUpdateDto, ZoneIdAndCustomerNumber zoneIdAndCustomerNumber, IAppUser appUser, string opLogText)
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
        private IEnumerable<KartInsertDto> GetKartInsertDto(ReportOutput<ServiceLinkReturnDisconnectHeaderOutputDto, ServiceLinkReturnDisconnectDataOutputDto> subscriptionAmounts, ServiceLinkReturnDisconnectInputDto input, MemberInfoGetDto memberInfo, int barge)
        {
            return subscriptionAmounts.ReportData.Select(s => new KartInsertDto()
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
                FinalAmount = s.FinalAmount,
                DiscountAmount = s.Discount,
                PardN = s.Amount,
                PardG = 0,
                Sum = s.Amount,
                AmountItemId = s.Id,
                SiphonId = int.Parse(memberInfo.MainSiphon),
                UsageId = memberInfo.UsageId,
                IsRegister = false,
                TotalServicesAmount = subscriptionAmounts.ReportHeader.Amount,
                FirstInstallment = 0,//
                JGEST_FA = 0,
                PishFa = 0,
                InstallmentPercent = 100,//
                Operator = _operator,
                DomesticUnit = memberInfo.DomesticUnit,
                CommercialUnit = memberInfo.CommercialUnit,
                OtherUnit = memberInfo.OtherUnit,
                KartTypeId = _kartTypeId,
                InsertedBy = _insertBy,
                BankDateJalali = string.Empty,
                Installment = 0,//
                InstallmentCount = 1,//
                MeterDiameterId = memberInfo.MeterDiameterId,
                Ser = 0,
                Type = _returnCategoryType,
            }).ToList();
        }
        private async Task<ICollection<RequestBillDetailsInsertDto>> GetRequestBillDetailsInsertDto(IEnumerable<KartInsertDto> kartItems, MemberInfoGetDto memberInfo)
        {
            ModifyTypeGetDto modifyTypeDto = await _modifyTypeQueryService.GetByKarten75(_returnCategoryType);
            ICollection<RequestBillDetailsInsertDto> requestBills = new List<RequestBillDetailsInsertDto>();
            foreach (var k in kartItems)
            {
                requestBills.Add(new RequestBillDetailsInsertDto()
                {
                    TrackNumber = k.StringTrackNumber,
                    ZoneId = k.ZoneId,
                    CustomerNumber = k.CustomerNumber,
                    BillId = memberInfo.BillId,
                    TypeId = modifyTypeDto.Title,
                    TypeCode = modifyTypeDto.RequestBillDetailsId,
                    ItemId = k.AmountItemId,
                    ItemTitle = (await _t100QueryService.Get(k.AmountItemId, true)).Title,
                    Amount = k.TotalServicesAmount,
                    OffAmount = k.DiscountAmount,
                    OffTitle = (await _discountTypeQueryService.Get((DiscountTypeEnum)k.DiscountTypeId)).Title,
                    FinalAmount = k.FinalAmount * -1,//returnType must * -1
                    RegisterDate = k.CurrentDateJalali,
                    ZoneTitle = memberInfo.ZoneTitle,
                    UsageId = memberInfo.UsageId,
                    UsageTitle = memberInfo.UsageTitle,
                    PayId = string.Empty,//
                    CommercialCount = memberInfo.CommercialUnit,
                    DomesticCount = memberInfo.DomesticUnit,
                    OtherCount = memberInfo.OtherUnit,
                    ContractualCapacity = memberInfo.ContractualCapacity,
                });
            }
            return requestBills;
        }

    }
}
