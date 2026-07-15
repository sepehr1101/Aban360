using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterReadingNonReadCreateHandler : AbstractBaseConnection, IMeterReadingNonReadCreateHandler
    {
        private readonly IMeterReadingCreateBaseHandler _meterReadingCreateBaseHandler;
        private readonly IMeterFlowQueryService _meterFlowService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IBillQueryService _billQueryService;
        private readonly IValidator<MeterReadingNonReadInputDto> _validator;
        private string _reportTitle = ReportLiterals.MeterReadingNonReadCreate;
        private int _agentCode = 0;
        private short _nonReadCounterStateId = 8;
        private int _maxDayCondition = -15;
        public MeterReadingNonReadCreateHandler(
            IMeterReadingCreateBaseHandler meterReadingCreateBaseHandler,
           IMeterFlowQueryService meterFlowService,
           ICustomerInfoService customerInfoService,
           IBillQueryService billQueryService,
           IValidator<MeterReadingNonReadInputDto> validator,
           IConfiguration configuration)
           : base(configuration)
        {
            _meterReadingCreateBaseHandler = meterReadingCreateBaseHandler;
            _meterReadingCreateBaseHandler.NotNull(nameof(meterReadingCreateBaseHandler));

            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(_meterFlowService));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(_customerInfoService));

            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>> Handle(MeterReadingNonReadInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);
            string fileName = $"{ReportLiterals.NonRead}-{Guid.NewGuid()}";
            string dateJalaliCondition = ConvertDate.JalaliToDateTime(input.CurrentDateJalali).AddDays(_maxDayCondition).ToShortPersianDateString();

            IEnumerable<BillLatestListDataOutputDto> latestBills = await _billQueryService.GetLatestForNonRead(new BillLatestListInputDto(input.ZoneId, input.FromReadingNumber, input.ToReadingNumber), dateJalaliCondition);
            if (!latestBills.Any())
            {
                throw new ReadingException(ExceptionLiterals.NotFoundAnyCustomer);
            }
            IEnumerable<MeterReadingDetailCreateDto> readingDetails = await GetMeterReadingDetails(latestBills, input, appUser, fileName);
            FileCreateDto fileInfo = new(fileName, null, null);
            ICollection<MeterReadingDetailCreateDto> readingDetailsCreate = await _meterReadingCreateBaseHandler.GetReadingDetailCreateFinalNonRead(readingDetails, fileInfo, appUser, cancellationToken);

            await _meterReadingCreateBaseHandler.ExecSql(readingDetailsCreate, fileInfo, appUser);
            return _meterReadingCreateBaseHandler.GetReturnData(readingDetailsCreate, _reportTitle);
        }
        private async Task<IEnumerable<MeterReadingDetailCreateDto>> GetMeterReadingDetails(IEnumerable<BillLatestListDataOutputDto> latestBills, MeterReadingNonReadInputDto input, IAppUser appUser, string fileName)
        {
            string fromReadingNumber = latestBills?.Min(m => m.ReadingNumber) ?? string.Empty;
            string toReadingNumber = latestBills?.Max(m => m.ReadingNumber) ?? string.Empty;
            MeterFlowCreateDto importedMeterFlow = GetMeterFlowCreateDto(MeterFlowStepEnum.Imported, 0, fileName, input.ZoneId, fromReadingNumber, toReadingNumber, latestBills?.Count() ?? 0, appUser.UserId, string.Empty);
            CustomersInfoGetDto customersInfo;
            int meterFlowId = 0;

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterFlowCommandService meterflowCommandService = new(connection, transaction);

                    meterFlowId = await meterflowCommandService.Insert(importedMeterFlow);
                    await meterflowCommandService.Update(meterFlowId, meterFlowId);
                    customersInfo = await _customerInfoService.GetByBulkCopy(connection, transaction, input.ZoneId, latestBills.Select(m => m.CustomerNumber).ToList());

                    transaction.Commit();
                }
            }
            IEnumerable<MeterReadingDetailCreateDto> meterReadingsDetailCreate = GetReadingMeterDetails(latestBills, customersInfo, appUser, meterFlowId, input.CurrentDateJalali);

            return meterReadingsDetailCreate;
        }
        private IEnumerable<MeterReadingDetailCreateDto> GetReadingMeterDetails(IEnumerable<BillLatestListDataOutputDto> latestBills, CustomersInfoGetDto customersInfo, IAppUser appUser, int meterFlowId, string currentDateJalali)
        {
            return from latestBill in latestBills
                   join members in customersInfo.MembersInfo
                       on latestBill.CustomerNumber equals members.CustomerNumber
                       into membersJoin
                   from members in membersJoin.DefaultIfEmpty()
                   join taviz in customersInfo.TavizInfo
                       on members.CustomerNumber equals taviz.CustomerNumber
                       into tavizJoin
                   from taviz in tavizJoin.DefaultIfEmpty()
                   select new MeterReadingDetailCreateDto()
                   {
                       FlowImportedId = meterFlowId,
                       ZoneId = latestBill.ZoneId,
                       CustomerNumber = latestBill.CustomerNumber,
                       ReadingNumber = latestBill.ReadingNumber,
                       BillId = members.BillId,
                       AgentCode = _agentCode,
                       CurrentCounterStateCode = _nonReadCounterStateId,
                       PreviousDateJalali = latestBill.PreviousDateJalali,
                       CurrentDateJalali = currentDateJalali,
                       PreviousNumber = latestBill.PreviousNumber,
                       CurrentNumber = 0,//todo:true?
                       InsertByUserId = appUser.UserId,
                       InsertDateTime = DateTime.Now,

                       BranchTypeId = members.BranchTypeId,
                       UsageId = members.UsageId,
                       ConsumptionUsageId = members.ConsumptionUsageId,
                       DomesticUnit = members.DomesticUnit,
                       CommercialUnit = members.CommercialUnit,
                       OtherUnit = members.OtherUnit,
                       EmptyUnit = members.EmptyUnit,
                       WaterInstallationDateJalali = members.WaterInstallationDateJalali,
                       SewageInstallationDateJalali = members.SewageInstallationDateJalali,
                       WaterRegisterDate = members.WaterRegisterDate,
                       SewageRegisterDate = members.WaterRegisterDate,
                       WaterCount = members.WaterCount,
                       SewageCalcState = members.SewageCalcState,
                       ContractualCapacity = members.ContractualCapacity,
                       HouseholdDate = members.HouseholdDate,
                       HouseholdNumber = members.HouseholdNumber,
                       VillageId = members.VillageId,
                       IsSpecial = members.IsSpecial,
                       MeterDiameterId = members.MeterDiameterId,
                       VirtualCategoryId = members.VirtualCategoryId,
                       BodySerial = members.BodySerial,

                       TavizCause = taviz?.TavizCause,
                       TavizDateJalali = taviz?.TavizDateJalali,
                       TavizNumber = taviz?.TavizNumber,
                       TavizRegisterDateJalali = taviz?.TavizRegisterDateJalali,

                       MonthlyConsumption = latestBill.ConsumptionAverage,
                       LastMeterDateJalali = latestBill is null ? members.WaterInstallationDateJalali : latestBill.PreviousDateJalali,
                       LastMeterNumber = latestBill?.PreviousNumber ?? 0,
                       LastConsumption = latestBill?.Consumption ?? 0,
                       LastMonthlyConsumption = latestBill.ConsumptionAverage,
                       LastCounterStateCode = latestBill?.CounterStateCode ?? 0,
                       LastSumItems = latestBill?.PreviousSumItems ?? 0
                   };
        }
        private MeterFlowCreateDto GetMeterFlowCreateDto(MeterFlowStepEnum step, int firstFlowId, string fileName, int zoneId, string fromReadingNumber, string toReadingNumber, int primaryCount, Guid userId, string description)
        {
            return new MeterFlowCreateDto()
            {
                MeterFlowStepId = step,
                FirstFlowId = firstFlowId,
                FileName = fileName,
                ZoneId = zoneId,
                FromReadingNumber = fromReadingNumber,
                ToReadingNumber = toReadingNumber,
                PrimaryCount = primaryCount,
                InsertByUserId = userId,
                InsertDateTime = DateTime.Now,
                Description = description
            };
        }
        private async Task InputValidate(MeterReadingNonReadInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
