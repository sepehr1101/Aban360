using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        private string _basePath = @"AppData\CollectBills";
        public CollectBillsDetailJobService(
            IBackgroundJobClient backgroundJobClient,
            ICollectBillsQueryService collectBillsQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient.NotNull(nameof(backgroundJobClient));

            _collectBillsQueryService = collectBillsQueryService;
            _collectBillsQueryService.NotNull(nameof(collectBillsQueryService));
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

            IEnumerable<CollectBillsDataDto> data = await _collectBillsQueryService.Get();
            string zipFileName = await CreateZip(data.Select(s => s.Text).ToList());
            string description = $"فایل:{zipFileName} با تعداد سطر:{data?.Count() ?? 0} ایجاد شد.";
            CollectBillsDetailUpdateDto finalCreateFile = new(effectedId, description, DateTime.Now);
            await CollectBillsDetailUpdate(finalCreateFile);
        }
        public async Task Upload()
        {

        }
        public async Task<string> CreateZip(ICollection<string> data)
        {
            var timeNow = DateTime.Now.ToString("HH-mm-ss");
            var persianDate = DateTime
                                      .Now
                                      .ToShortPersianDateString()
                                      .Replace('/', '-')
                                      .Replace(':', '-')
                                      .Replace(' ', '_');
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
    }
}
