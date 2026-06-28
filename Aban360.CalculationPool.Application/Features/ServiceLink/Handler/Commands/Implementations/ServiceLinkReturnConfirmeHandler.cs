using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class ServiceLinkReturnConfirmeHandler : AbstractBaseConnection, IServiceLinkReturnConfirmeHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonZoneService _commonZoneService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IKartQueryService _kartQueryService;
        private readonly IT100QueryService _t100QueryService;
        private readonly IDiscountTypeQueryService _discountTypeQueryService;
        private readonly IModifyTypeQueryService _modifyTypeQueryService;
        static int[] _allowedMultipleAmount = [_returnTypeCodeInRequestBillDetail, _creditTypeCodeInRequestBillDetail];
        const int _returnTypeCodeInRequestBillDetail = 4;
        const int _creditTypeCodeInRequestBillDetail = 6;
        public ServiceLinkReturnConfirmeHandler(
            IHttpContextAccessor contextAccessor,
            ICommonZoneService commonZoneService,
            ICommonMemberQueryService commonMemberQueryService,
            IKartQueryService kartQueryService,
            IT100QueryService t100QueryService,
            IDiscountTypeQueryService discountTypeQueryService,
            IModifyTypeQueryService modifyTypeQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _kartQueryService = kartQueryService;
            _kartQueryService.NotNull(nameof(kartQueryService));

            _t100QueryService = t100QueryService;
            _t100QueryService.NotNull(nameof(t100QueryService));

            _discountTypeQueryService = discountTypeQueryService;
            _discountTypeQueryService.NotNull(nameof(discountTypeQueryService));

            _modifyTypeQueryService = modifyTypeQueryService;
            _modifyTypeQueryService.NotNull(nameof(modifyTypeQueryService));

        }

        public async Task Handle(string billId, IAppUser appUser, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _commonZoneService.IsUserInZone(appUser, memberInfo.ZoneId);

            IEnumerable<KartGetDto> kartsInfo = await _kartQueryService.GetAll(memberInfo.CustomerNumber, memberInfo.ZoneId);
            IEnumerable<KartRemoveByIdDto> kartRemoveDto = kartsInfo.Select(k => new KartRemoveByIdDto(k.Id, k.ZoneId, k.CustomerNumber)).ToList();
            IEnumerable<KartInsertDto> kartsInsertDto = GetKartInsertDto(kartsInfo);
            IEnumerable<RequestBillDetailsInsertDto> requestsBillDetailInsertDto = await GetRequestBillDetailsInsertDto(kartsInsertDto, memberInfo);
            string opLogText = string.Format(OpLogLiterals.ServiceLinkReturnConfirmedOpLog, memberInfo.BillId, kartsInsertDto?.Count() ?? 0);

            await ExecSql(kartRemoveDto, kartsInsertDto, requestsBillDetailInsertDto, appUser, opLogText);
        }
        private IEnumerable<KartInsertDto> GetKartInsertDto(IEnumerable<KartGetDto> kartsInfo)
        {
            return kartsInfo.
                 Select(k => new KartInsertDto()
                 {
                     ZoneId = k.ZoneId,
                     CustomerNumber = k.CustomerNumber,
                     ReadingNumber = k.ReadingNumber,
                     StringTrackNumber = k.StringTrackNumber,
                     Serial = k.Serial,
                     Barge = k.Barge,
                     CurrentDateJalali = k.CurrentDateJalali,
                     DueDateJalali = k.DueDateJalali,
                     DiscountTypeId = k.DiscountTypeId,
                     FinalAmount = k.FinalAmount,
                     DiscountAmount = k.DiscountAmount,
                     PardN = k.PardN,
                     PardG = k.PardG,
                     Sum = k.Sum,
                     AmountItemId = k.AmountItemId,
                     SiphonId = k.SiphonId,
                     UsageId = k.UsageId,
                     IsRegister = k.IsRegister,
                     TotalServicesAmount = k.TotalServicesAmount,
                     FirstInstallment = k.FirstInstallment,
                     JGEST_FA = k.JGEST_FA,
                     PishFa = k.PishFa,
                     InstallmentPercent = k.InstallmentPercent,
                     Operator = k.Operator,
                     DomesticUnit = k.DomesticUnit,
                     CommercialUnit = k.CommercialUnit,
                     OtherUnit = k.OtherUnit,
                     KartTypeId = k.KartTypeId,
                     InsertedBy = k.InsertedBy,
                     BankDateJalali = k.BankDateJalali,
                     Installment = k.Installment,
                     InstallmentCount = k.InstallmentCount,
                     MeterDiameterId = k.MeterDiameterId,
                     Ser = k.Ser,
                     Type = k.Type,
                 });
        }
        private async Task<IEnumerable<RequestBillDetailsInsertDto>> GetRequestBillDetailsInsertDto(IEnumerable<KartInsertDto> items, MemberInfoGetDto memberInfo)
        {
            ICollection<RequestBillDetailsInsertDto> result = new List<RequestBillDetailsInsertDto>();
            foreach (var item in items)
            {
                ModifyTypeGetDto modifyTypeInfo = await _modifyTypeQueryService.GetByKarten75(item.Type);
                long finalAmount = _allowedMultipleAmount.Contains(modifyTypeInfo.RequestBillDetailsId) ? -1 * item.TotalServicesAmount : item.TotalServicesAmount;

                result.Add(new RequestBillDetailsInsertDto()
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
                    PayId = string.Empty,//
                    CommercialCount = memberInfo.CommercialUnit,
                    DomesticCount = memberInfo.DomesticUnit,
                    OtherCount = memberInfo.OtherUnit,
                    ContractualCapacity = memberInfo.ContractualCapacity,
                });
            }
            return result;
        }
        private async Task ExecSql(IEnumerable<KartRemoveByIdDto> kartRemoveDto, IEnumerable<KartInsertDto> kartsInsertDto, IEnumerable<RequestBillDetailsInsertDto> requestBillDetailsInsertDto, IAppUser appUser, string opLogText)
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
    }
}