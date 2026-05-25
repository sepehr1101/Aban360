using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.OldCalcPool.Application.Constant;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Index.HPRtree;
using System.Collections.Generic;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class ServiceLinkReturnHandler : AbstractBaseConnection, IServiceLinkReturnHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IVariabService _variabService;
        private readonly IT100QueryService _t100QueryService;
        private readonly IDiscountTypeQueryService _discountTypeQueryService;
        private readonly IModifyTypeQueryService _modifyTypeQueryService;
        private readonly IValidator<ServiceLinkReturnInputDto> _validator;
        static string _insertBy = "Aban";
        static int[] _allowedMultipleAmount = { 2, 3, 5 };
        static int _manualSerial = 10000;
        static int _operator = 666;
        static int _kartTypeId = 8;
        public ServiceLinkReturnHandler(
            IHttpContextAccessor contextAccessor,
            ICommonMemberQueryService commonMemberQueryService,
            IVariabService variabService,
            IT100QueryService t100QueryService,
            IDiscountTypeQueryService discountTypeQueryService,
            IModifyTypeQueryService modifyTypeQueryService,
            IValidator<ServiceLinkReturnInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _t100QueryService = t100QueryService;
            _t100QueryService.NotNull(nameof(t100QueryService));

            _discountTypeQueryService = discountTypeQueryService;
            _discountTypeQueryService.NotNull(nameof(discountTypeQueryService));

            _modifyTypeQueryService = modifyTypeQueryService;
            _modifyTypeQueryService.NotNull(nameof(modifyTypeQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(ServiceLinkReturnInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumbere = await _commonMemberQueryService.Get(inputDto.BillId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumbere);
            decimal barge = await _variabService.GetAndRenew(memberInfo.ZoneId);

            IEnumerable<KartInsertDto> kartsInsertDto = GetKartInsertDto(inputDto.ReturnItems, memberInfo, (int)barge);
            IEnumerable<RequestBillDetailsInsertDto> requestBillDetailsInsertDto = await GetRequestBillDetailsInsertDto(kartsInsertDto, memberInfo);
            string opLogText = string.Format(Literals.RequestOfferingReturnOpLog, inputDto.BillId, inputDto.ReturnItems?.Count() ?? 0, kartsInsertDto?.Sum(k => k.FinalAmount) ?? 0);
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

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidation(ServiceLinkReturnInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private IEnumerable<KartInsertDto> GetKartInsertDto(IEnumerable<ServiceLinkReturnItemDto> items, MemberInfoGetDto memberInfo, int barge)
        {
            return items.Select(i => new KartInsertDto()
            {
                ZoneId = memberInfo.ZoneId,
                CustomerNumber = memberInfo.CustomerNumber,
                ReadingNumber = memberInfo.ReadingNumber,
                StringTrackNumber = DateTime.Now.ToShortPersianDateString(),
                Serial = _manualSerial,
                Barge = barge,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                DueDateJalali = DateTime.Now.AddMonths(1).ToShortPersianDateString(),
                DiscountTypeId = 0,
                FinalAmount = i.Amount,
                DiscountAmount = 0,
                PardN = i.Amount,
                PardG = 0,
                Sum = i.Amount,
                AmountItemId = i.AmountItemId,//From T100
                SiphonId = int.Parse(memberInfo.MainSiphon),
                UsageId = memberInfo.UsageId,
                IsRegister = false,
                TotalServicesAmount = i.Amount,
                FirstInstallment = i.Amount,
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
                Type = (int)i.CategoryType,
            }).ToList();
        }
        private async Task<IEnumerable<RequestBillDetailsInsertDto>> GetRequestBillDetailsInsertDto(IEnumerable<KartInsertDto> items, MemberInfoGetDto memberInfo)
        {
            //amount
            ICollection<RequestBillDetailsInsertDto> result = new List<RequestBillDetailsInsertDto>();
            foreach (var item in items)
            {
                ModifyTypeGetDto modifyTypeInfo = await _modifyTypeQueryService.GetByKarten75(item.Type);
                long finalAmount = _allowedMultipleAmount.Contains(item.Type) ? -1 * item.FinalAmount : item.FinalAmount;
                result.Add(new RequestBillDetailsInsertDto()
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
                });
            }
            return result;
        }
    }
}
