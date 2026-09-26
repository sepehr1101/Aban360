using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.CommunicationPool.Domain.Features.Sms.Commands;
using Aban360.CommunicationPool.Persistence.Features.Sms.Commands.Implementations;
using DotNetDBF;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Aban360.Common.Extensions.IoExtensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterReadingFileCreateHandler : AbstractBaseConnection, IMeterReadingFileCreateHandler
    {
        private readonly IMeterReadingCreateBaseHandler _meterReadingCreateBaseHandler;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailQueryService;
        private readonly ISmsStateTemplateQueryService _stateTemplateQueryService;
        private readonly IValidator<MeterReadingFileCreateDto> _validator;
        private readonly IT51QueryService _zoneQueryService;
        private static string _dbfPath = DirectoryLiterals.DbfFolderPath;
        private int _smsGroupId = CommonLiterals.SmsStateGroupCollectBills_Close;
        private int _reminderSmsTypeId = CommonLiterals.SmsTypeReminder;
        public MeterReadingFileCreateHandler(
            IMeterReadingCreateBaseHandler meterReadingCreateBaseHandler,
            IMeterReadingDetailQueryService meterReadingDetailQueryService,
            ISmsStateTemplateQueryService stateTemplateQueryService,
            IValidator<MeterReadingFileCreateDto> validator,
            IT51QueryService zoneQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _meterReadingCreateBaseHandler = meterReadingCreateBaseHandler;
            _meterReadingCreateBaseHandler.NotNull(nameof(meterReadingCreateBaseHandler));

            _meterReadingDetailQueryService = meterReadingDetailQueryService;
            _meterReadingDetailQueryService.NotNull(nameof(meterReadingDetailQueryService));

            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));

            _stateTemplateQueryService = stateTemplateQueryService;
            _stateTemplateQueryService.NotNull(nameof(stateTemplateQueryService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task<MeterReadingFileCreateOutputDto> Handle(MeterReadingFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);
            await _meterReadingCreateBaseHandler.CheckDuplicateFile(input.ReadingFile.FileName, cancellationToken);

            string filePath = await SaveToDisk(input.ReadingFile, _dbfPath);
            IEnumerable<MeterReadingDetailCreateDto> readingDetails = await GetMeterReadingDetails(input, filePath, appUser.UserId);
            FileCreateDto fileCreateInfo = new(input.ReadingFile.FileName, filePath, input.Description);
            ICollection<MeterReadingDetailCreateDto> readingDetailsCreate = await _meterReadingCreateBaseHandler.GetReadingDetailCreateFinal(readingDetails, appUser, cancellationToken);

            await _meterReadingCreateBaseHandler.ExecSql(readingDetailsCreate, fileCreateInfo, appUser);
            return await InsertCloseSmsAndGetResult(readingDetailsCreate, appUser);
        }
        private async Task<IEnumerable<MeterReadingDetailCreateDto>> GetMeterReadingDetails(MeterReadingFileCreateDto meterFile, string filePath, Guid userId)
        {
            ICollection<MeterReadingFileDetail> meterReadings = ReadDb(filePath, userId);
            var (customersInfo, meterFlowId) = await _meterReadingCreateBaseHandler.GetCustomerInfoAndFirstFlowId(meterReadings, meterFile.ReadingFile.FileName, filePath, meterFile.Description, userId);
            IEnumerable<MeterReadingDetailCreateDto> meterReadingsDetailCreate = _meterReadingCreateBaseHandler.GetReadingMeterDetails(meterReadings, customersInfo, meterFlowId);

            return meterReadingsDetailCreate;
        }
        private ICollection<MeterReadingFileDetail> ReadDb(string filePath, Guid userId)
        {
            ICollection<MeterReadingFileDetail> meterReadingFileDetail = new List<MeterReadingFileDetail>();

            FileStream stream = File.OpenRead(filePath);
            try
            {
                DBFReader reader = new DBFReader(stream);
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

                    MeterReadingFileDetail meterDetail = _meterReadingCreateBaseHandler.CreateMeterReading(zoneId, customerNumber, readingNumber, agentCode, counterStateCode, previousDay, currentDay, previousNumber, currentNumber, userId);
                        meterReadingFileDetail.Add(meterDetail);
                    }
                }
            catch
            {
                throw new ReadingException(ExceptionLiterals.InvalidReadingFile);
            }

            ICollection<MeterReadingFileDetail> meterReadingDetailWithoutDuplicate = meterReadingFileDetail
                .GroupBy(s => s.CustomerNumber)
                .Select(m => m
                    .OrderByDescending(r => r.CurrentDateJalali)
                    .ThenByDescending(r => r.CurrentNumber)
                    .First())
                .ToList();

            return meterReadingDetailWithoutDuplicate;
        }
        private async Task<ICollection<SmsDraftInsertDto>> GetSmsDraftList(IEnumerable<MeterReadingDetailDataOutputDto> meterDetails, SmsStateTemplateGetDto smsTemplateInfo, int smsFlowId)
        {
            NumericDictionary zoneInfo = await _zoneQueryService.Get((meterDetails?.FirstOrDefault()?.ZoneId ?? 0), true);
            ICollection<SmsDraftInsertDto> newSmsDraftList = new List<SmsDraftInsertDto>();
            foreach (var item in meterDetails)
            {
                string smsText = string.Format(smsTemplateInfo.SmsText, zoneInfo.Title, item.BillId, Environment.NewLine);
                SmsDraftInsertDto newSmsDraftInsert = new(null, item.BillId, smsTemplateInfo.Id, smsText, item.MobileNumber, item.FlowImportedId.ToString(), item.Id.ToString());
                newSmsDraftList.Add(newSmsDraftInsert);
            }
            return newSmsDraftList;
        }
        public async Task<MeterReadingFileCreateOutputDto> InsertCloseSmsAndGetResult(ICollection<MeterReadingDetailCreateDto> readingDetailsCreate, IAppUser appUser)
        {
            int firstFlowId = readingDetailsCreate?.FirstOrDefault()?.FlowImportedId ?? 0;
            IEnumerable<MeterReadingDetailDataOutputDto> meterReadigInfo = await _meterReadingDetailQueryService.Get(firstFlowId, false);
            ICollection<MeterReadingDetailDataOutputDto> closeReadingDetailCreate = meterReadigInfo.Where(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Close).ToList();

            if ((closeReadingDetailCreate?.Count() ?? 0) != 0)
            {
                int newSmsFlowId = await GenerateAndInsertCloseSms(closeReadingDetailCreate, appUser);
                return new MeterReadingFileCreateOutputDto(firstFlowId, newSmsFlowId);
            }
            return new MeterReadingFileCreateOutputDto(0, 0);
        }
        private async Task<int> GenerateAndInsertCloseSms(ICollection<MeterReadingDetailDataOutputDto> closeReadingToSend, IAppUser appUser)
        {
            DateTime currentDate = DateTime.Now;
            SmsStateTemplateGetDto smsTemplateInfo = await _stateTemplateQueryService.GetFirst(_smsGroupId, _reminderSmsTypeId);
            SmsFlowInsertDto newSmsFlow = new()
            {
                FirstFlowId = closeReadingToSend?.FirstOrDefault()?.FlowImportedId ?? 0,
                SmsCount = closeReadingToSend?.Count ?? 0,
                SmsTemplateId = smsTemplateInfo.Id,
                InsertBy = appUser.UserId,
                InsertDateTime = currentDate,
                DueDateTime = currentDate.AddDays(smsTemplateInfo.DueDay)
            };
            int newFlowId = 0;
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    SmsFlowCommandService smsFlowCommandService = new(connection, transaction);
                    SmsDraftCommandService smsDraftCommandService = new(connection, transaction);

                    newFlowId = await smsFlowCommandService.Insert(newSmsFlow);
                    ICollection<SmsDraftInsertDto> newSmsDraftList = await GetSmsDraftList(closeReadingToSend, smsTemplateInfo, newFlowId);
                    await smsDraftCommandService.Insert(newSmsDraftList);

                    transaction.Commit();
                }
            }
            return newFlowId;
        }
        private async Task InputValidate(MeterReadingFileCreateDto input, CancellationToken cancellationToken)
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
