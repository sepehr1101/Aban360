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

            ICollection<MeterReadingDetailCreateDto> meterReadingsDetailCreate = new List<MeterReadingDetailCreateDto>();
            foreach (var meterReadingInfo in meterReadings)
            {
                CustomerInfoGetDto customerInfo = await _customerInfoService.Get(meterReadingInfo.ZoneId, meterReadingInfo.CustomerNumber);
                MeterReadingDetailCreateDto meterReadingDetail = GetMeterReadingDetail(customerInfo, meterReadingInfo, meterFlowId);
                meterReadingsDetailCreate.Add(meterReadingDetail);
            }

            //save meterReadingsDetailCreate in metereadingDetail
            await _meterReadingDetailService.Create(meterReadingsDetailCreate);
        }

        private MeterReadingDetailCreateDto GetMeterReadingDetail(CustomerInfoGetDto customerInfo, MeterReadingFileDetail meterReading, int flowId)
        {
            try
            {


                return new MeterReadingDetailCreateDto()
                {
                    FlowImportedId = flowId,
                    ZoneId = meterReading.ZoneId,
                    CustomerNumber = meterReading.CustomerNumber,
                    ReadingNumber = meterReading.ReadingNumber,
                    BillId = customerInfo.MembersInfo.BillId,
                    AgentCode = meterReading.AgentCode,
                    CurrentCounterStateCode = meterReading.CurrentCounterStateCode,
                    PreviousDateJalali = meterReading.PreviousDateJalali,
                    CurrentDateJalali = meterReading.CurrentDateJalali,
                    PreviousNumber = meterReading.PreviousNumber,
                    CurrentNumber = meterReading.CurrentNumber,
                    InsertByUserId = meterReading.InsertByUserId,
                    InsertDateTime = meterReading.InsertDateTime,

                    UsageId = customerInfo.MembersInfo.UsageId,
                    DomesticUnit = customerInfo.MembersInfo.DomesticUnit,
                    CommercialUnit = customerInfo.MembersInfo.CommercialUnit,
                    OtherUnit = customerInfo.MembersInfo.OtherUnit,
                    EmptyUnit = customerInfo.MembersInfo.EmptyUnit,
                    WaterInstallationDateJalali = customerInfo.MembersInfo.WaterInstallationDateJalali,
                    SewageInstallationDateJalali = customerInfo.MembersInfo.SewageInstallationDateJalali,
                    WaterRegisterDate = customerInfo.MembersInfo.WaterRegisterDate,
                    SewageRegisterDate = customerInfo.MembersInfo.WaterRegisterDate,
                    WaterCount = customerInfo.MembersInfo.WaterCount,
                    SewageCalcState = customerInfo.MembersInfo.SewageCalcState,
                    ContractualCapacity = customerInfo.MembersInfo.ContractualCapacity,
                    HouseholdDate = customerInfo.MembersInfo.HouseholdDate,
                    HouseholdNumber = customerInfo.MembersInfo.HouseholdNumber,
                    VillageId = customerInfo.MembersInfo.VillageId,
                    IsSpecial = customerInfo.MembersInfo.IsSpecial,
                    MeterDiameterId = customerInfo.MembersInfo.MeterDiameterId,
                    VirtualCategoryId = customerInfo.MembersInfo.VirtualCategoryId,

                    TavizCause = customerInfo.TavizInfo?.TavizCause,
                    TavizDateJalali = customerInfo.TavizInfo?.TavizDateJalali,
                    TavizNumber = customerInfo.TavizInfo?.TavizNumber,
                    TavizRegisterDateJalali = customerInfo.TavizInfo?.TavizRegisterDateJalali,

                    LastMeterDateJalali = customerInfo.BedBesInfo is null ? customerInfo.MembersInfo.WaterInstallationDateJalali : customerInfo.BedBesInfo.LastMeterDateJalali,
                    LastMeterNumber = customerInfo.BedBesInfo is null ? 0 : customerInfo.BedBesInfo.LastMeterNumber,
                    ConsumptionAverage = customerInfo.BedBesInfo is null ? 0 : customerInfo.BedBesInfo.ConsumptionAverage,
                    LastCounterStateCode = customerInfo.BedBesInfo is null ? 0 : customerInfo.BedBesInfo.LastCounterStateCode,
                };
            }
            catch (Exception en)
            {
                throw new InvalidDataException(en + "   " + meterReading.CustomerNumber + " -- " + meterReading.ZoneId);
            }
        }
        private void CreateMeterReadingDetail(MeterReadingFileDetail meterReading, int meterFlowId)
        {
            MeterReadingDetailCreateDto met = new MeterReadingDetailCreateDto()
            {
                FlowImportedId = meterFlowId,
                ZoneId = meterReading.ZoneId,
                CustomerNumber = meterReading.CustomerNumber,
                ReadingNumber = meterReading.ReadingNumber,
                BillId = "----",//todo
                AgentCode = meterReading.AgentCode,
                CurrentCounterStateCode = meterReading.CurrentCounterStateCode,
                PreviousDateJalali = meterReading.PreviousDateJalali,
                CurrentDateJalali = meterReading.CurrentDateJalali,
                PreviousNumber = meterReading.PreviousNumber,
                CurrentNumber = meterReading.CurrentNumber,

                ExcludedByUserId = null,
                ExcludedDateTime = null,

                InsertByUserId = meterReading.InsertByUserId,
                InsertDateTime = meterReading.InsertDateTime,
                RemovedByUserId = null,
                RemovedDateTime = null,


            };
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
