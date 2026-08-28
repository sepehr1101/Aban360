using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;
using Aban360.CalculationPool.Infrastructure.Providers.CollectBills.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO.Compression;
using System.Text;

namespace Aban360.CalculationPool.Application.Features.Base
{
    public interface ICollectBillsDetailJobService
    {
        Task Initialize();
        Task Upload(Guid groupingId, string zipFileName);//todo: remove
        Task<CollectBillsGetZipFileInfo> CreateZip(ICollection<string> data, string fromDateJalali, string toDateJalali);
    }
    public sealed class CollectBillsDetailJobService : AbstractBaseConnection, ICollectBillsDetailJobService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ICollectBillsQueryService _collectBillsQueryService;
        private readonly ICollectBillsService _collectBillsService;
        private readonly IT51QueryService _zoneQueryService;
        private static DateTime _currentDateTime = DateTime.Now;
        private static string _currentDateJalali = _currentDateTime.ToShortPersianDateString();
        private int _currentYear = Convert.ToInt16(_currentDateJalali.Substring(0, 4));
        private int _currentMonth = Convert.ToInt16(_currentDateJalali.Substring(5, 2));
        private string _basePath = DirectoryLiterals.CollectBillsFolderPath;
        private string _cityCode = "1002031406";
        public CollectBillsDetailJobService(
            IBackgroundJobClient backgroundJobClient,
            ICollectBillsQueryService collectBillsQueryService,
            ICollectBillsService collectBillsService,
            IT51QueryService zoneQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient.NotNull(nameof(backgroundJobClient));

            _collectBillsQueryService = collectBillsQueryService;
            _collectBillsQueryService.NotNull(nameof(collectBillsQueryService));

            _collectBillsService = collectBillsService;
            _collectBillsService.NotNull(nameof(collectBillsService));

            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));
        }

        public async Task Initialize()
        {
            CollectBillsDetailInsertDto initializeLogDto = new(Guid.NewGuid(), (int)CollectBillStepEnum.Initialize, DateTime.Now, DateTime.Now, string.Empty);
            int effectedId = await CollectgBillsDetailInsert(initializeLogDto);

            _backgroundJobClient.Enqueue(() => CreateFile(initializeLogDto.GroupingId));
        }
        public async Task CreateFile(Guid groupingId)
        {
            CollectBillsDetailInsertDto createZipFileLogDto = new(groupingId, (int)CollectBillStepEnum.CreateZip, DateTime.Now, null, string.Empty);
            int effectedId = await CollectgBillsDetailInsert(createZipFileLogDto);

            CollectBillsGetDataToSendInputDto dtoToGenerateTxtFile = new(fromDateJalali: _currentDateJalali, toDateJalali: _currentDateJalali);
            IEnumerable<CollectBillsDataDto> customersDataToSend = await _collectBillsQueryService.Get(dtoToGenerateTxtFile);
            CollectBillsGetZipFileInfo zipFileInfo = await CreateZip(customersDataToSend.Select(s => s.Row).ToList(), dtoToGenerateTxtFile.FromDateJalali, dtoToGenerateTxtFile.FromDateJalali);
            string description = string.Format(ExceptionLiterals.CollectBillsCreateZipFile, zipFileInfo.FileName, customersDataToSend?.Count() ?? 0);
            CollectBillsDetailUpdateDto createZipFileUpdateLogDto = new(effectedId, zipFileInfo.FileName, description, DateTime.Now);
            await CollectBillsDetailUpdate(createZipFileUpdateLogDto);

            _backgroundJobClient.Enqueue(() => Upload(groupingId, zipFileInfo.FileName));
        }
        public async Task Upload(Guid groupingId, string zipFileName)
        {
            CollectBillsDetailInsertDto uploadInsertLogDto = new(groupingId, (int)CollectBillStepEnum.Upload, DateTime.Now, null, string.Empty);
            int effectedId = await CollectgBillsDetailInsert(uploadInsertLogDto);

            CollectBillsUploadInputDto uploadInputDto = await GetUploadInputDto(zipFileName);
            CollectBillsOutputDto<CollectBillsUploadOutputDto> result = await _collectBillsService.Upload(uploadInputDto);

            string description = result.Code == (int)CollectBillsResponseStatusEnum.Success ? string.Format(ExceptionLiterals.CollectBillsSuccessUploadedFileLog, result.Result.FileID, result.Message) : string.Format(ExceptionLiterals.CollectBillsUnsuccessUploadedFileLog, zipFileName, result.Code, result.Message);
            CollectBillsDetailUpdateDto finalCreateFile = new(effectedId, zipFileName, description, DateTime.Now);
            await CollectBillsDetailUpdate(finalCreateFile);

            if (result.Code == (int)CollectBillsResponseStatusEnum.Success)
            {
                result.Result = new CollectBillsUploadOutputDto("10000");//todo: remove this line
                _backgroundJobClient.Enqueue(() => SetFileDetail(groupingId, result.Result.FileID, zipFileName));
            }
        }
        public async Task SetFileDetail(Guid groupingId, string fileId, string zipFileName)
        {
            CollectBillsDetailInsertDto createfile = new(groupingId, (int)CollectBillStepEnum.AssingUploadedFile, DateTime.Now, null, string.Empty);
            int effectedId = await CollectgBillsDetailInsert(createfile);

            CollectBillsAssignUploadedFileInputDto assignUploadedFileDto = new(fileId, _currentYear.ToString(), _currentMonth.ToString(), string.Empty);
            CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto> result = await _collectBillsService.AssignUploadedFile(assignUploadedFileDto);
            //validate 

            string description = result.Code == (int)CollectBillsResponseStatusEnum.Success ? string.Format(ExceptionLiterals.CollectBillsAssignUploadedFileSeccessLog, fileId, assignUploadedFileDto.FileYear, assignUploadedFileDto.FileCycle, result.Code, result.Message) : string.Format(ExceptionLiterals.CollectBillsAssignUploadedFileUnseccessLog, fileId, assignUploadedFileDto.FileYear, assignUploadedFileDto.FileCycle, result.Code, result.Message);
            CollectBillsDetailUpdateDto finalCreateFile = new(effectedId, zipFileName, description, DateTime.Now);
            await CollectBillsDetailUpdate(finalCreateFile);

            if (result.Code == (int)CollectBillsResponseStatusEnum.Success)
            {
                _backgroundJobClient.Enqueue(() => Confirm(groupingId, fileId, zipFileName));
            }
        }
        public async Task Confirm(Guid groupingId, string fileId, string zipFileName)
        {
            CollectBillsDetailInsertDto createfile = new(groupingId, (int)CollectBillStepEnum.Confirm, DateTime.Now, null, string.Empty);
            int effectedId = await CollectgBillsDetailInsert(createfile);

            CollectBillsConfirmFileInputDto confirmFileDto = new(fileId);
            CollectBillsOutputDto<CollectBillsConfirmFileOutputDto> result = await _collectBillsService.ConfirmFileBills(confirmFileDto);
            //validate 

            string description = result.Code == (int)CollectBillsResponseStatusEnum.Success ? string.Format(ExceptionLiterals.CollectBillsConfirmSuccessLog, fileId, result.Code, result.Message) : string.Format(ExceptionLiterals.CollectBillsConfirmUnsuccessLog, fileId, result.Code, result.Message);
            CollectBillsDetailUpdateDto finalCreateFile = new(effectedId, zipFileName, description, DateTime.Now);
            await CollectBillsDetailUpdate(finalCreateFile);
            //get State
        }


        public async Task<CollectBillsGetZipFileInfo> CreateZip(ICollection<string> data, string fromDateJalali, string toDateJalali)
        {
            var timeNow = DateTime.Now.ToString("HH-mm-ss");
            var persianDate = fromDateJalali.Replace("/", "");

            string baseFileName = $"{persianDate}-{timeNow}-CollectBills";
            string txtFileName = $"{baseFileName}.txt";
            string zipFileName = $"{baseFileName}.zip";

            string txtPath = Path.Combine(_basePath, txtFileName);
            string zipPath = Path.Combine(_basePath, zipFileName);


            await File.WriteAllLinesAsync(txtPath, data, new UTF8Encoding(true));
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(txtPath, txtFileName);
            }

            return new CollectBillsGetZipFileInfo(zipPath, zipFileName);
        }
        private async Task<int> CollectgBillsDetailInsert(CollectBillsDetailInsertDto insertDto)
        {
            int effectedId = 0;
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    CollectBillsDetailCommandService collectBillsDetailCommandService = new(connection, transaction);
                    effectedId = await collectBillsDetailCommandService.Insert(insertDto);

                    transaction.Commit();
                }
            }
            return effectedId;
        }
        private async Task CollectBillsDetailUpdate(CollectBillsDetailUpdateDto updateDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    CollectBillsDetailCommandService collectBillsDetailCommandService = new(connection, transaction);
                    await collectBillsDetailCommandService.Update(updateDto);

                    transaction.Commit();
                }
            }
        }
        private async Task<CollectBillsUploadInputDto> GetUploadInputDto(string zipFileName)
        {
            string filePath = Path.Combine(_basePath, zipFileName);
            if (!File.Exists(filePath))
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotFoundFile);
            }
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            string base64 = Convert.ToBase64String(fileBytes);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            return new CollectBillsUploadInputDto(base64, extension, fileName, _cityCode);//todo:CityCode
        }
    }
}
