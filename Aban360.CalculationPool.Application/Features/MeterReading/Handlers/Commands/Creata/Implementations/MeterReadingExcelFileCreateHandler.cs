using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Excel = MiniExcelLibs;
using System.Data;
using static Aban360.Common.Extensions.IoExtensions;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.Common.Literals;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Implementations
{
    internal sealed class MeterReadingExcelFileCreateHandler : AbstractBaseConnection, IMeterReadingExcelFileCreateHandler
    {
        private readonly IMeterReadingCreateBaseHandler _meterReadingCreateBaseHandler;
        private readonly IValidator<MeterReadingExcelFileCreateDto> _validator;
        private static string _reportTitle = ReportLiterals.MeterReadingCreateFile;
        private static string _dbfPath = ReportLiterals.MeterReadingFilePath;
        public MeterReadingExcelFileCreateHandler(
            IMeterReadingCreateBaseHandler meterReadingCreateBaseHandler,
            IValidator<MeterReadingExcelFileCreateDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _meterReadingCreateBaseHandler = meterReadingCreateBaseHandler;
            _meterReadingCreateBaseHandler.NotNull(nameof(meterReadingCreateBaseHandler));

            _validator = validator;
            _validator.NotNull(nameof(_validator));
        }

        public async Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>> Handle(MeterReadingExcelFileCreateDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(input, cancellationToken);
            await _meterReadingCreateBaseHandler.CheckDuplicateFile(input.ReadingFile.FileName, cancellationToken);

            string filePath = await SaveToDisk(input.ReadingFile, _dbfPath);
            IEnumerable<MeterReadingDetailCreateDto> readingDetails = await GetMeterReadingDetails(input, filePath, appUser.UserId);
            FileCreateDto fileCreateInfo = new(input.ReadingFile.FileName, filePath, input.Description);
            ICollection<MeterReadingDetailCreateDto> readingDetailsCreate = await _meterReadingCreateBaseHandler.GetReadingDetailCreateFinal(readingDetails, appUser, cancellationToken);

            await _meterReadingCreateBaseHandler.ExecSql(readingDetailsCreate, fileCreateInfo, appUser);
            return _meterReadingCreateBaseHandler.GetReturnData(readingDetailsCreate, _reportTitle);
        }
        private async Task<IEnumerable<MeterReadingDetailCreateDto>> GetMeterReadingDetails(MeterReadingExcelFileCreateDto meterFile, string filePath, Guid userId)
        {
            ICollection<MeterReadingFileDetail> meterReadings = ReadExcel(filePath, userId);
            var (customersInfo, meterFlowId) = await _meterReadingCreateBaseHandler.GetCustomerInfoAndFirstFlowId(meterReadings, meterFile.ReadingFile.FileName, filePath, meterFile.Description, userId);
            IEnumerable<MeterReadingDetailCreateDto> meterReadingsDetailCreate = _meterReadingCreateBaseHandler.GetReadingMeterDetails(meterReadings, customersInfo, meterFlowId);

            return meterReadingsDetailCreate;
        }
        private ICollection<MeterReadingFileDetail> ReadExcel(string filePath, Guid userId)
        {
            ICollection<MeterReadingFileDetail> meterReadingFileDetail = new List<MeterReadingFileDetail>();
            var rows = Excel.MiniExcel.Query(filePath, useHeaderRow: false, sheetName: ExceptionLiterals.Page(1));
            string errorMessage = ExceptionLiterals.InvalidReadingFile;
            int count = 0;

            try
            {
                foreach (var item in rows.Skip(1))
                {
                    var row = (IDictionary<string, object>)item;
                    count++;
                    try
                    {
                        //0:CurrentNumber 1:CurretnDate 2:CurrentCounterState 3:AgentCode 4:ZoneId 5:ZoneTitle
                        //6:CustomerNumber 7:BillId 8:ReadingNumber 9:PriNumber 10:PriDate 
                        int customerNumber = Convert.ToInt32(row.ElementAt(6).Value);
                        string readingNumber = row.ElementAt(8).Value.ToString();
                        string previousDay = row.ElementAt(10).Value.ToString();
                        string currentDay = row.ElementAt(1).Value.ToString();
                        int previousNumber = Convert.ToInt32(row.ElementAt(9).Value);
                        int currentNumber = Convert.ToInt32(row.ElementAt(0).Value);
                        short counterStateCode = Convert.ToInt16(row.ElementAt(2).Value);
                        int agentCode = Convert.ToInt32(row.ElementAt(3).Value);
                        int zoneId = Convert.ToInt32(row.ElementAt(4).Value);

                        MeterReadingFileDetail meterDetail = _meterReadingCreateBaseHandler.CreateMeterReading(zoneId, customerNumber, readingNumber, agentCode, counterStateCode, previousDay, currentDay, previousNumber, currentNumber, userId);
                        meterReadingFileDetail.Add(meterDetail);
                    }
                    catch
                    {
                        errorMessage = ExceptionLiterals.InvalidRecord(count);
                        throw new ReadingException(errorMessage);
                    }
                }
            }
            catch
            {
                throw new ReadingException(errorMessage);
            }
            return meterReadingFileDetail;
        }
        private async Task InputValidate(MeterReadingExcelFileCreateDto input, CancellationToken cancellationToken)
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
