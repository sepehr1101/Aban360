using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations
{
    internal sealed class ServiceLinkTempReturnHandler : AbstractBaseConnection, IServiceLinkTempReturnHandler
    {
        private readonly ICommonZoneService _commonZoneService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IVariabService _variabService;
        private readonly IValidator<ServiceLinkTempReturnInputDto> _validator;
        const string _insertBy = "Aban";
        const int _manualSerial = 10000;
        const int _operator = 666;
        const int _kartTypeId = 2;
        const int _discountDescriptionCode = 14;

        public ServiceLinkTempReturnHandler(
            ICommonZoneService commonZoneService,
            ICommonMemberQueryService commonMemberQueryService,
            IVariabService variabService,
            IValidator<ServiceLinkTempReturnInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _variabService = variabService;
            _variabService.NotNull(nameof(variabService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(ServiceLinkTempReturnInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validate(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumbere = await _commonMemberQueryService.Get(inputDto.BillId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumbere);
            decimal barge = await _variabService.GetAndRenew(memberInfo.ZoneId);

            KartInsertDto kartsInsertDto = GetKartInsertDto(inputDto, memberInfo, (int)barge);

            await ExecSql(kartsInsertDto, appUser);
        }
        private async Task ExecSql(KartInsertDto kartsInsertDto, IAppUser appUser)
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
        private async Task Validate(ServiceLinkTempReturnInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private KartInsertDto GetKartInsertDto(ServiceLinkTempReturnInputDto input, MemberInfoGetDto memberInfo, int barge)
        {
            bool hasDiscountAmount = _discountDescriptionCode == input.DescriptionCode;
            long amount = hasDiscountAmount ? 0 : input.Amount;
            long discountAmount = hasDiscountAmount ? input.Amount : 0;

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
                DiscountTypeId = input.DiscountTypeId,
                FinalAmount = input.Amount,// amount,
                DiscountAmount = 0,//discountAmount,
                PardN = input.Amount,//amount,
                PardG = 0,
                Sum = input.Amount,
                AmountItemId = input.AmountItemId,//From T100
                SiphonId = int.Parse(memberInfo.MainSiphon),
                UsageId = memberInfo.UsageId,
                IsRegister = false,
                TotalServicesAmount = input.Amount,
                FirstInstallment = input.Amount,
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
                Type = (int)input.CategoryType,
            };
        }
    }
}
