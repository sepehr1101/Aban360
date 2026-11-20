using Aban360.CalculationPool.Application.Features.CollectBills.Services;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Extensions;
using System.Runtime.InteropServices;

namespace Aban360.CalculationPool.Application.Features.Base
{
    // سازگاری با Jobهای قبلی Hangfire؛ منطق اجرایی فقط در سرویس یکپارچه قرار دارد.
    public interface ICollectBillsDetailJobService
    {
        Task Initialize([Optional] DateTime? inputDateTime, CancellationToken cancellationToken);
        Task<CollectBillsGetZipFileInfo> CreateZip(ICollection<string> data, string fromDateJalali, string toDateJalali);
    }

    // نام کامل کلاس و امضای متدها برای فعال‌سازی Jobهای ذخیره‌شده حفظ می‌شود.
    public sealed class CollectBillsDetailJobService : ICollectBillsDetailJobService
    {
        private readonly ICollectBillsApplicationService _service;
        public CollectBillsDetailJobService(ICollectBillsApplicationService service)
        {
            _service = service;
            _service.NotNull(nameof(service));
        }
        public Task Initialize(DateTime? inputDateTime, CancellationToken cancellationToken) => _service.Initialize(inputDateTime, cancellationToken);
        public Task<CollectBillsGetZipFileInfo> CreateZip(ICollection<string> data, string fromDateJalali, string toDateJalali) => _service.CreateZip(data, fromDateJalali, toDateJalali);
        public Task CreateFile(Guid groupingId, string inputDateJalali) => _service.CreateFile(groupingId, inputDateJalali);
        public Task Upload(Guid groupingId, string zipFileName, string inputDateJalali) => _service.Upload(groupingId, zipFileName, inputDateJalali);
        public Task SetFileDetail(Guid groupingId, string fileId, string zipFileName, string inputDateJalali) => _service.SetFileDetail(groupingId, fileId, zipFileName, inputDateJalali);
        public Task Confirm(Guid groupingId, string fileId, string zipFileName) => _service.Confirm(groupingId, fileId, zipFileName);
    }
}
