using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Implementations
{
    internal sealed class ChangeDateBatchHandler : AbstractBaseConnection, IChangeDateBatchHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly IMeterReadingCreateBaseHandler _meterReadingCreateBaseHandler;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IValidator<ChangeDateBatchInputDto> _validator;
        public ChangeDateBatchHandler(
            IHttpContextAccessor contextAccessor,
            IMeterFlowQueryService meterFlowQueryService,
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            IMeterReadingCreateBaseHandler meterReadingCreateBaseHandler,
            ICommonZoneService commonZoneService,
            IValidator<ChangeDateBatchInputDto> validator,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));

            _meterReadingDetailQueryService = meterReadingDetailQueryService;
            _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));

            _meterReadingCreateBaseHandler = meterReadingCreateBaseHandler;
            _meterReadingCreateBaseHandler.NotNull(nameof(meterReadingCreateBaseHandler));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ChangeDateBatchOutputDto> Handle(ChangeDateBatchInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            MeterFlowGetDto meterFlowInfo = await _meterFlowQueryService.Get(inputDto.MeterFlowId);
            IEnumerable<MeterReadingDetailDataOutputDto> meterReadings = await _meterReadingDetailQueryService.Get(inputDto);
            if (!inputDto.IsConfirm)
            {
                return GetResult(meterReadings?.Count() ?? 0, inputDto);
            }
            ICollection<MeterReadingDetailCreateDto> meterReadingDetailCreate = meterReadings
                .Select(m => new MeterReadingDetailCreateDto()
                {
                    FlowImportedId = m.FlowImportedId,
                    ZoneId = m.ZoneId,
                    CustomerNumber = m.CustomerNumber,
                    ReadingNumber = m.ReadingNumber,
                    BillId = m.BillId,
                    AgentCode = m.AgentCode,
                    CurrentCounterStateCode = m.CurrentCounterStateCode,
                    PreviousDateJalali = m.PreviousDateJalali,
                    CurrentDateJalali = inputDto.DateJalali,
                    PreviousNumber = m.PreviousNumber,
                    CurrentNumber = m.CurrentNumber,
                    InsertByUserId = m.InsertByUserId,
                    InsertDateTime = m.InsertDateTime,
                    WaterDebt = m.WaterDebt,

                    BranchTypeId = m.BranchTypeId,
                    UsageId = m.UsageId,
                    ConsumptionUsageId = m.ConsumptionUsageId,
                    DomesticUnit = m.DomesticUnit,
                    CommercialUnit = m.CommercialUnit,
                    OtherUnit = m.OtherUnit,
                    EmptyUnit = m.EmptyUnit,
                    WaterInstallationDateJalali = m.WaterInstallationDateJalali,
                    SewageInstallationDateJalali = m.SewageInstallationDateJalali,
                    WaterRegisterDate = m.WaterRegisterDate,
                    SewageRegisterDate = m.SewageRegisterDate,
                    WaterCount = m.WaterCount,
                    SewageCalcState = m.SewageCalcState,
                    ContractualCapacity = m.ContractualCapacity,
                    HouseholdDate = m.HouseholdDate,
                    HouseholdNumber = m.HouseholdNumber,
                    VillageId = m.VillageId,
                    IsSpecial = m.IsSpecial,
                    MeterDiameterId = m.MeterDiameterId,
                    VirtualCategoryId = m.VirtualCategoryId,
                    BodySerial = m.BodySerial,

                    TavizCause = m.TavizCause,
                    TavizDateJalali = m.TavizDateJalali,
                    TavizNumber = m.TavizNumber,
                    TavizRegisterDateJalali = m.TavizRegisterDateJalali,

                    LastMeterDateJalali = m.LastMeterDateJalali,
                    LastMeterNumber = m.LastMeterNumber ?? 0,
                    LastConsumption = m.LastConsumption ?? 0,
                    LastMonthlyConsumption = m.LastMonthlyConsumption ?? 0,
                    LastCounterStateCode = m.LastCounterStateCode ?? 0,
                    LastSumItems = m.LastSumItems ?? 0,

                })
                .ToList();
            ICollection<MeterReadingDetailCreateDto> meterDetailFinalDto = await _meterReadingCreateBaseHandler.GetReadingDetailCreateFinal(meterReadingDetailCreate, appUser, cancellationToken);
            MeterReadingDetailByFromToReadingNumberDeleteDto meterDetailDeleteDto = new(meterFlowInfo.FirstFlowId, inputDto.FromReadingNumber, inputDto.ToReadingNumber, appUser.UserId, DateTime.Now, Domain.Constants.MeterReadingDetailRemovedType.EditReadingDate);
            string opLogText = string.Format(OpLogLiterals.ChangeDateBatchOpLog, meterFlowInfo.FirstFlowId, inputDto.FromReadingNumber, inputDto.ToReadingNumber, meterReadings?.Count() ?? 0, inputDto.DateJalali);

            await ExecSql(meterDetailFinalDto, meterDetailDeleteDto, appUser, opLogText);
            return GetResult(meterReadings?.Count() ?? 0, inputDto);
        }
        private async Task ExecSql(ICollection<MeterReadingDetailCreateDto> meterDetailFinalDto, MeterReadingDetailByFromToReadingNumberDeleteDto meterDetailDeleteDto, IAppUser appUser, string opLogText)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterReadingDetailCommandService meterReadingDetailCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await meterReadingDetailCommandService.Delete(meterDetailDeleteDto, meterDetailFinalDto?.Count() ?? 0);
                    await meterReadingDetailCommandService.Insert(meterDetailFinalDto);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private async Task InputValidate(ChangeDateBatchInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private ChangeDateBatchOutputDto GetResult(int count, ChangeDateBatchInputDto inputDto)
        {
            return new ChangeDateBatchOutputDto(inputDto.FromReadingNumber, inputDto.ToReadingNumber, inputDto.DateJalali, count);
        }
    }
}
