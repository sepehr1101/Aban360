using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class CalculationRequestInsertManualHandler : AbstractBaseConnection, ICalculationRequestInsertManualHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IValidator<KartInsertManualInputDto> _validator;
        static string _insertBy = "Aban";
        public CalculationRequestInsertManualHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IValidator<KartInsertManualInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(KartInsertManualInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            KartInsertDto kartInsertDto = GetKartInsertDto(inputDto, moshtrakInfo, userCode);
            string dbName = GetDbName(trackingInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    KartCommandService kartCommandService = new(connection, transaction);
                    await kartCommandService.Insert(kartInsertDto, dbName);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidation(KartInsertManualInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private KartInsertDto GetKartInsertDto(KartInsertManualInputDto inputDto, MoshtrakOutputDto moshtrakInfo, int userCode)//todo
        {
            long amount = inputDto.CategoryType == KartCategoryTypeEnum.Debtor ? inputDto.Amount : -1 * inputDto.Amount;
            return new KartInsertDto()
            {
                ZoneId = moshtrakInfo.ZoneId,
                CustomerNumber = moshtrakInfo.CustomerNumber,
                ReadingNumber = moshtrakInfo.ReadingNumber,
                StringTrackNumber = moshtrakInfo.TrackNumber.ToString().PadLeft(11, '0'),
                Serial = 0,
                Barge = 0,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                DueDateJalali = DateTime.Now.AddMonths(1).ToShortPersianDateString(),
                DiscountTypeId = 0,
                FinalAmount = amount,
                DiscountAmount = 0,
                PardN = amount,
                PardG = 0,
                Sum = amount,
                ServiceSelectedId = inputDto.ServiceGroupId,//todo: check has same id
                SiphonId = moshtrakInfo.MainSiphon,
                UsageId = moshtrakInfo.UsageId,
                IsRegister = false,
                TotalServicesAmount = amount,
                FirstInstallment = amount,
                JGEST_FA = 0,
                PishFa = 0,
                InstallmentPercent = 100,
                Operator = userCode,
                DomesticUnit = moshtrakInfo.DomesticUnit,
                CommercialUnit = moshtrakInfo.CommercialUnit,
                OtherUnit = moshtrakInfo.OtherUnit,
                KartTypeId = (int)inputDto.CategoryType,//todo,
                InsertedBy = _insertBy,
                BankDateJalali = string.Empty,
                Installment = 0,//todo
                InstallmentCount = 1,
                MeterDiameterId = moshtrakInfo.MeterDiameterId,
                Ser = 0,
                Type = 0,//todo
            };
        }
    }
}
