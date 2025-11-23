using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using DotNetDBF;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterReadingFileCreateHandler : IMeterReadingFileCreateHandler
    {
        private readonly IMeterReadingDetailService _meterReadingFileService;
        private readonly IMeterFlowService _meterFlowService;
        private readonly ICustomerInfoService _customerInfoService;
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        private readonly IValidator<MeterReadingFileCreateDto> _validator;
        public MeterReadingFileCreateHandler(
            IMeterReadingDetailService meterReadingFileService,
            IMeterFlowService meterFlowService,
            ICustomerInfoService customerInfoService,
            IMeterReadingDetailService meterReadingDetailService,
            IValidator<MeterReadingFileCreateDto> validator)
        {
            _meterReadingFileService = meterReadingFileService;
            _meterReadingFileService.NotNull(nameof(_meterReadingFileService));

            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(_meterFlowService));

            _customerInfoService = customerInfoService;
            _customerInfoService.NotNull(nameof(_customerInfoService));

            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(_meterReadingDetailService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task Handle(MeterReadingFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await Validation(input, cancellationToken);
            ICollection<MeterReadingFileDetail> meterReadings = ReadDb(input.FilePath, appUser.UserId);
            MeterReadingFileDetail firstMeterDetail = meterReadings.FirstOrDefault();

            MeterFlowCreateDto meterFlow = CreateImportedMeterFlowStep(input.ReadingFile.FileName, firstMeterDetail.ZoneId, appUser.UserId, input.Description);
            int meterFlowId = await _meterFlowService.Create(meterFlow);


            CustomersInfoGetDto customersInfo = await _customerInfoService.Get(firstMeterDetail.ZoneId, meterReadings.Select(m => m.CustomerNumber).ToList());

            IEnumerable<MeterReadingDetailCreateDto> meterReadingsDetailCreate =
                from meterReading in meterReadings
                
                join members in customersInfo.MembersInfo
                    on meterReading.CustomerNumber equals members.CustomerNumber
                    into membersJoin
                from members in membersJoin.DefaultIfEmpty()

                join bedbes in customersInfo.BedBesInfo
                    on members.CustomerNumber equals bedbes.CustomerNumber
                    into bedbesJoin
                from bedbes in bedbesJoin.DefaultIfEmpty()

                join taviz in customersInfo.TavizInfo
                    on bedbes.CustomerNumber equals taviz.CustomerNumber
                    into tavizJoin
                from taviz in tavizJoin.DefaultIfEmpty()
                 select new MeterReadingDetailCreateDto()
                 {
                     FlowImportedId = meterFlowId,
                     ZoneId = meterReading.ZoneId,
                     CustomerNumber = meterReading.CustomerNumber,
                     ReadingNumber = meterReading.ReadingNumber,
                     BillId = members.BillId,
                     AgentCode = meterReading.AgentCode,
                     CurrentCounterStateCode = meterReading.CurrentCounterStateCode,
                     PreviousDateJalali = meterReading.PreviousDateJalali,
                     CurrentDateJalali = meterReading.CurrentDateJalali,
                     PreviousNumber = meterReading.PreviousNumber,
                     CurrentNumber = meterReading.CurrentNumber,
                     InsertByUserId = meterReading.InsertByUserId,
                     InsertDateTime = meterReading.InsertDateTime,

                     UsageId = members.UsageId,
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

                     TavizCause = taviz?.TavizCause,
                     TavizDateJalali = taviz?.TavizDateJalali,
                     TavizNumber = taviz?.TavizNumber,
                     TavizRegisterDateJalali = taviz?.TavizRegisterDateJalali,

                     LastMeterDateJalali = bedbes is null ? members.WaterInstallationDateJalali : bedbes.LastMeterDateJalali,
                     LastMeterNumber = bedbes is null ? 0 : bedbes.LastMeterNumber,
                     ConsumptionAverage = bedbes is null ? 0 : bedbes.ConsumptionAverage,
                     LastCounterStateCode = bedbes is null ? 0 : bedbes.LastCounterStateCode,
                 };
          
            await _meterReadingDetailService.Create(meterReadingsDetailCreate);
        }

        private async Task Validation(MeterReadingFileCreateDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        public MeterReadingFileDetail CreateMeterReading(int zoneId, int customerNumber, string readingNumber, int agentCode, short currentCounterStateCode, string previousDateJalali, string currentDateJalali, int previousNumber, int currentNumber, Guid userId)
        {
            return new MeterReadingFileDetail(
                zoneId: zoneId,
                customerNumber: customerNumber,
                readingNumber: readingNumber,
                agentCode: agentCode,
                currentCounterStateCode: currentCounterStateCode,
                previousDateJalali: previousDateJalali,
                currentDateJalali: currentDateJalali,
                previousNumber: previousNumber,
                currentNumber: currentNumber,
                insertByUserId: userId,
                insertDateTime: DateTime.Now
            );
        }
        private ICollection<MeterReadingFileDetail> ReadDb(string filePath, Guid userId)
        {
            var result = new List<Dictionary<string, object>>();

            FileStream stream = File.OpenRead(filePath);
            DBFReader reader = new DBFReader(stream);

            int recordCount = reader.RecordCount;
            DBFField[] fields = reader.Fields;

            ICollection<MeterReadingFileDetail> meterReadingFileDetail = new List<MeterReadingFileDetail>();
            object[] rowObjects;
            while ((rowObjects = reader.NextRecord()) != null)
            {
                //radif=0 eshterak=1 pridate=2 currentday=3 prinu=4 currentnu=5 codvas-counterstate=6 mamorcode=7 town=13
                int customerNumber = (int)(decimal)rowObjects[0];
                string readingNumber = (string)rowObjects[1];
                string previousDay = (string)rowObjects[2];
                string currentDay = (string)rowObjects[3];
                int previousNumber = (int)(decimal)rowObjects[4];
                int currentNumber = (int)(decimal)rowObjects[5];
                short counterStateCode = (short)(decimal)rowObjects[6];
                int agentCode = (int)(decimal)rowObjects[7];
                int zoneId = (int)(decimal)rowObjects[13];

                MeterReadingFileDetail meterDetail = CreateMeterReading(zoneId, customerNumber, readingNumber, agentCode, counterStateCode, previousDay, currentDay, previousNumber, currentNumber, userId);
                meterReadingFileDetail.Add(meterDetail);
            }

            return meterReadingFileDetail;
        }
        private MeterFlowCreateDto CreateImportedMeterFlowStep(string fileName, int zoneId, Guid userId, string description)
        {
            return new MeterFlowCreateDto()
            {
                MeterFlowStepId = MeterFlowStepEnum.Imported,
                FileName = fileName,
                ZoneId = zoneId,
                InsertByUserId = userId,
                InsertDateTime = DateTime.Now,
                Description = description
            };
        }

    }
}
