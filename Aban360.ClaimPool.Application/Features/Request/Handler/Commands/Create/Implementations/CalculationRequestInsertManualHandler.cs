using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Constant;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class CalculationRequestInsertManualHandler : AbstractBaseConnection, ICalculationRequestInsertManualHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IT100QueryService _t100QueryService;
        private readonly IValidator<KartInsertManualInputDto> _validator;
        static string _insertBy = "Aban";
        static int _manualSerial = 10000;
        static int _operator = 666;
        static int _kartTypeId = 8;
        public CalculationRequestInsertManualHandler(
            IHttpContextAccessor contextAccessor,
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IT100QueryService t100QueryService,
            IValidator<KartInsertManualInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _t100QueryService = t100QueryService;
            _t100QueryService.NotNull(nameof(t100QueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<SaleAndAfterSaleDataOutputDto> Handle(KartInsertManualInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            KartInsertDto kartInsertDto = GetKartInsertDto(inputDto, moshtrakInfo);
            GhestUpdateDto ghestInsertDto = new(trackingInfo.StringTrackNumber, kartInsertDto.FinalAmount);
            NumericDictionary amountItemInfo = await _t100QueryService.Get(inputDto.AmountItemId, true);
            string opLogText = string.Format(Literals.RequestOfferingInsertOpLog, amountItemInfo.Title, inputDto.TrackNumber, kartInsertDto.FinalAmount, inputDto.CategoryType.ToString());//todo: CategoryType not persian -> user dateBase
            string dbName = GetDbName(trackingInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    KartCommandService kartCommandService = new(connection, transaction);
                    GhestCommandService ghestCommandService = new(connection, transaction);
                    OpLogCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await kartCommandService.Insert(kartInsertDto, false, dbName);
                    await ghestCommandService.Update(ghestInsertDto, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
            return new SaleAndAfterSaleDataOutputDto((short)kartInsertDto.AmountItemId, amountItemInfo.Title, kartInsertDto.FinalAmount + kartInsertDto.DiscountAmount, kartInsertDto.DiscountAmount, kartInsertDto.FinalAmount, kartInsertDto.DiscountTypeId, true);
        }
        private async Task InputValidate(KartInsertManualInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private KartInsertDto GetKartInsertDto(KartInsertManualInputDto inputDto, MoshtrakOutputDto moshtrakInfo)//todo
        {
            return new KartInsertDto()
            {
                ZoneId = moshtrakInfo.ZoneId,
                CustomerNumber = moshtrakInfo.CustomerNumber,
                ReadingNumber = moshtrakInfo.ReadingNumber,
                StringTrackNumber = moshtrakInfo.TrackNumber.ToString().PadLeft(11, '0'),
                Serial = _manualSerial,
                Barge = 0,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                DueDateJalali = DateTime.Now.AddMonths(1).ToShortPersianDateString(),
                DiscountTypeId = 0,
                FinalAmount = inputDto.Amount,
                DiscountAmount = 0,
                PardN = inputDto.Amount,
                PardG = 0,
                Sum = inputDto.Amount,
                AmountItemId = inputDto.AmountItemId,//From T100
                SiphonId = moshtrakInfo.MainSiphon,
                UsageId = moshtrakInfo.UsageId,
                IsRegister = false,
                TotalServicesAmount = inputDto.Amount,
                FirstInstallment = inputDto.Amount,
                JGEST_FA = 0,
                PishFa = 0,
                InstallmentPercent = 100,
                Operator = _operator,
                DomesticUnit = moshtrakInfo.DomesticUnit,
                CommercialUnit = moshtrakInfo.CommercialUnit,
                OtherUnit = moshtrakInfo.OtherUnit,
                KartTypeId = _kartTypeId,
                InsertedBy = _insertBy,
                BankDateJalali = string.Empty,
                Installment = 0,
                InstallmentCount = 1,
                MeterDiameterId = moshtrakInfo.MeterDiameterId,
                Ser = 0,
                Type = (int)inputDto.CategoryType,
            };
        }
    }
}
