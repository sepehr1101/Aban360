using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
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
using Aban360.ReportPool.Domain.Base;
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
        private readonly IT51QueryService _zoneQueryService;
        private readonly IValidator<MeterReadingFileCreateDto> _validator;
        private static string _reportTitle = ReportLiterals.MeterReadingCreateFile;
        private static string _dbfPath = DirectoryLiterals.DbfFolderPath;
        public MeterReadingFileCreateHandler(
            IMeterReadingCreateBaseHandler meterReadingCreateBaseHandler,
            IT51QueryService zoneQueryService,
            IValidator<MeterReadingFileCreateDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _meterReadingCreateBaseHandler = meterReadingCreateBaseHandler;
            _meterReadingCreateBaseHandler.NotNull(nameof(meterReadingCreateBaseHandler));

            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>> Handle(MeterReadingFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);
            await _meterReadingCreateBaseHandler.CheckDuplicateFile(input.ReadingFile.FileName, cancellationToken);

            string filePath = await SaveToDisk(input.ReadingFile, _dbfPath);
            IEnumerable<MeterReadingDetailCreateDto> readingDetails = await GetMeterReadingDetails(input, filePath, appUser.UserId);
            FileCreateDto fileCreateInfo = new(input.ReadingFile.FileName, filePath, input.Description);
            ICollection<MeterReadingDetailCreateDto> readingDetailsCreate = await _meterReadingCreateBaseHandler.GetReadingDetailCreateFinal(readingDetails, appUser, cancellationToken);

            await _meterReadingCreateBaseHandler.ExecSql(readingDetailsCreate, fileCreateInfo, appUser);

            ICollection<MeterReadingDetailCreateDto> closeReadingDerailCreate = readingDetailsCreate.Where(r => r.CurrentCounterStateCode == (int)CounterStateCodeEnum.Close).ToList();
            ICollection<SmsDraftInsertDto> newSmsDraftList = await GetSmsDraftList(closeReadingDerailCreate);
            await SmsDraftExecSql(newSmsDraftList);

            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = _meterReadingCreateBaseHandler.GetReturnData(readingDetailsCreate, _reportTitle);
            return result;
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
        private async Task<ICollection<SmsDraftInsertDto>> GetSmsDraftList(IEnumerable<MeterReadingDetailCreateDto> meterDetails)
        {
            NumericDictionary zoneInfo = await _zoneQueryService.Get((meterDetails?.FirstOrDefault()?.ZoneId ?? 0), true);
            ICollection<SmsDraftInsertDto> newSmsDraftList = new List<SmsDraftInsertDto>();
            foreach (var item in meterDetails)
            {
                string smsText = string.Format(SmsTemplates.ClosedBill, zoneInfo.Title, item.BillId, Environment.NewLine);
                SmsDraftInsertDto newSmsDraftInsert = new(null, item.BillId, CommonLiterals.MeterReadingBatchSmsDraftTypeId, smsText, item.MobileNumber, item.FlowImportedId.ToString());
                newSmsDraftList.Add(newSmsDraftInsert);
            }
            return newSmsDraftList;
        }
        private async Task SmsDraftExecSql(ICollection<SmsDraftInsertDto> newSmsDraftList)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    SmsDraftCommandService smsDraftCommandService = new(connection, transaction);
                    await smsDraftCommandService.Insert(newSmsDraftList);

                    transaction.Commit();
                }
            }
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
