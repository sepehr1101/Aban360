using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;
using Aban360.CalculationPool.Infrastructure.Providers.CollectBills.Contracts;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
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
    }
    public sealed class CollectBillsDetailJobService : AbstractBaseConnection, ICollectBillsDetailJobService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly ICollectBillsQueryService _collectBillsQueryService;
        private readonly ICollectBillsService _collectBillsService;
        private readonly IT51QueryService _zoneQueryService;
        private string currentDateJalali = DateTime.Now.ToShortPersianDateString();
        private string _basePath = @"AppData\CollectBills";
        private string _cityCode = "130000";
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
            CollectBillsDetailInsertDto insertDto = new(Guid.NewGuid(), (int)CollectBillStepEnum.Initialize, DateTime.Now, DateTime.Now, string.Empty);
            //Validate
            int effectedId = await CollectgBillsDetailInsert(insertDto);
            _backgroundJobClient.Enqueue(() => CreateFile(insertDto.GroupingId));

        }
        public async Task CreateFile(Guid groupingId)
        {
            CollectBillsDetailInsertDto createfile = new(groupingId, (int)CollectBillStepEnum.CreateZip, DateTime.Now, null, string.Empty);
            int effectedId = await CollectgBillsDetailInsert(createfile);


            CollectBillsGetDataToSendInputDto dtoToGenerateTxtFile = new(fromDateJalali: currentDateJalali, toDateJalali: currentDateJalali);
            IEnumerable<CollectBillsDataDto> data = await _collectBillsQueryService.Get(dtoToGenerateTxtFile);
            string zipFileName = await CreateZip(data.Select(s => s.Row).ToList(), dtoToGenerateTxtFile.FromDateJalali, dtoToGenerateTxtFile.FromDateJalali);
            string description = $"فایل:{zipFileName} با تعداد سطر:{data?.Count() ?? 0} ایجاد شد.";
            CollectBillsDetailUpdateDto finalCreateFile = new(effectedId, description, DateTime.Now);
            await CollectBillsDetailUpdate(finalCreateFile);

            _backgroundJobClient.Enqueue(() => Upload(groupingId, zipFileName));
        }
        public async Task Upload(Guid groupingId, string zipFileName)
        {
            CollectBillsDetailInsertDto createfile = new(groupingId, (int)CollectBillStepEnum.Upload, DateTime.Now, null, string.Empty);
            int effectedId = await CollectgBillsDetailInsert(createfile);

            CollectBillsUploadInputDto uploadInputDto = await GetUploadInputDto(zipFileName);
            CollectBillsOutputDto<CollectBillsUploadOutputDto> result = await _collectBillsService.Upload(uploadInputDto);
            //validate on result

            string description = string.Empty;// $"فایل آپلود شد. کد فایل:{result.Parameters.FileID}  کد وضعیت:{result.Status.Code}  توضیحات:{result.Status.Description}";
            CollectBillsDetailUpdateDto finalCreateFile = new(effectedId, description, DateTime.Now);
            await CollectBillsDetailUpdate(finalCreateFile);

            _backgroundJobClient.Enqueue(() => SetFileDetail(groupingId, "" /*result.Parameters.FileID*/));
        }
        public async Task SetFileDetail(Guid groupingId, string fileId)
        {
            CollectBillsDetailInsertDto createfile = new(groupingId, (int)CollectBillStepEnum.AssingUploadedFile, DateTime.Now, null, string.Empty);
            int effectedId = await CollectgBillsDetailInsert(createfile);

            CollectBillsAssignUploadedFileInputDto assignUploadedFileDto = new(fileId, "", "", string.Empty);//todo: 2params from GetServiceConfig -> how to use?
            CollectBillsOutputDto<CollectBillsAssignUploadedFileOutputDto> result = await _collectBillsService.AssignUploadedFile(assignUploadedFileDto);
            //validate 

            string description = $"اطلاعات تکمیلی به فایل آپلود شده اضافه شد.";
            CollectBillsDetailUpdateDto finalCreateFile = new(effectedId, description, DateTime.Now);
            await CollectBillsDetailUpdate(finalCreateFile);

            _backgroundJobClient.Enqueue(() => Confirm(groupingId, fileId));
        }
        public async Task Confirm(Guid groupingId, string fileId)
        {
            CollectBillsDetailInsertDto createfile = new(groupingId, (int)CollectBillStepEnum.Confirm, DateTime.Now, null, string.Empty);
            int effectedId = await CollectgBillsDetailInsert(createfile);

            CollectBillsConfirmFileInputDto confirmFileDto = new(fileId);
            CollectBillsOutputDto<CollectBillsConfirmFileOutputDto> result = await _collectBillsService.ConfirmFileBills(confirmFileDto);
            //validate 

            string description = $"تایید فایل انجام شد.";
            CollectBillsDetailUpdateDto finalCreateFile = new(effectedId, description, DateTime.Now);
            await CollectBillsDetailUpdate(finalCreateFile);

            //get State
        }


        private async Task<string> CreateZip(ICollection<string> data, string fromDateJalali, string toDateJalali)
        {
            var timeNow = DateTime.Now.ToString("HH-mm-ss");
            var persianDate = DateTime.Now.ToShortPersianDateString().Replace("/", "");

            string baseFileName = $"{persianDate}-{timeNow}-قبوض تجمیعی";
            string txtFileName = $"{baseFileName}.txt";
            string zipFileName = $"{baseFileName}.zip";

            string txtPath = Path.Combine(_basePath, txtFileName);
            string zipPath = Path.Combine(_basePath, zipFileName);


            await File.WriteAllLinesAsync(txtPath, data, new UTF8Encoding(true));
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(txtPath, txtFileName);
            }

            return zipFileName;
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
