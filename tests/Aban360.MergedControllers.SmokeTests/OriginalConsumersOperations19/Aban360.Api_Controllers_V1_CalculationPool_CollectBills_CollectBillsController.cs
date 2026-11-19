using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Commands.Implementations;
using Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Queries.Implementations;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.CollectBills.Outputs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Controllers.V1.CalculationPool.CollectBills;
using Aban360.Api.Controllers.V1.CalculationPool.CollectBills;

namespace BeforeConsumersOperations19.Aban360.Api.Controllers.V1.CalculationPool.CollectBills
{
    [Route("v1/collect-bills")]
    public class CollectBillsController : BaseController
    {
        private readonly ICollectBillsFileCreateHandler _collectBillsFileCreateHandler;
        private readonly ICollectBillsDetailWithLastStepGetHadler _collectBillsDetailWithLastStepHandler;
        private readonly ICollectBillsDetailFileDownloadHandler _collectBillsDetailFileDownloadHandler;
        private string _zipContentType = DirectoryLiterals.ZipFileContentType;
        private readonly ICollectBillsDetailJobService _jobService;
        public CollectBillsController(ICollectBillsFileCreateHandler collectBillsFileCreateHandler, ICollectBillsDetailWithLastStepGetHadler collectBillsDetailWithLastStepHandler, ICollectBillsDetailFileDownloadHandler collectBillsDetailFileDownloadHandler, ICollectBillsDetailJobService jobService)
        {
            _collectBillsFileCreateHandler = collectBillsFileCreateHandler;
            _collectBillsFileCreateHandler.NotNull(nameof(collectBillsFileCreateHandler));
            _collectBillsDetailWithLastStepHandler = collectBillsDetailWithLastStepHandler;
            _collectBillsDetailWithLastStepHandler.NotNull(nameof(collectBillsDetailWithLastStepHandler));
            _collectBillsDetailFileDownloadHandler = collectBillsDetailFileDownloadHandler;
            _collectBillsDetailFileDownloadHandler.NotNull(nameof(collectBillsDetailFileDownloadHandler));
            _jobService = jobService;
            _jobService.NotNull(nameof(jobService));
        }

        /// <summary>ساخت و دانلود فایل ZIP اطلاعات یک روز مشخص</summary>
        /// <remarks>تاریخ شمسی با قالب yyyy/MM/dd در dateJalali ارسال شود. برای فرانت از POST با بدنه JSON استفاده کنید؛ ورودی GET فعلی نیز از بدنه خوانده می‌شود. پاسخ موفق خود فایل ZIP است، نه JSON؛ پاسخ را به‌صورت blob دریافت کنید.</remarks>
        [HttpPost, HttpGet]
        [Route("file-create")]
        public async Task<FileStreamResult> CreateFile(CollectBillsByDateJalaliInputDto inputDto, CancellationToken cancellationToken)
        {
            ConvertDate.JalaliToDateTime(inputDto.DateJalali, true);
            CollectBillsGetZipFileInfo zipFileInfo = await _collectBillsFileCreateHandler.Handle(inputDto.DateJalali, CurrentUser, cancellationToken);
            if (!System.IO.File.Exists(zipFileInfo.FilePath))
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotFoundFolder);
            }

            FileStream fileStream = new FileStream(zipFileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, _zipContentType, zipFileInfo.FileName);
        }

        /// <summary>گزارش وضعیت پردازش و ارسال فایل‌ها در بازه زمانی</summary>
        /// <remarks>fromDateJalali و toDateJalali با قالب yyyy/MM/dd در بدنه JSON ارسال شوند. پاسخ، گزارش JSON شامل آخرین مرحله، توضیحات و نام فایل است؛ این متد دانلود انجام نمی‌دهد.</remarks>
        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CollectBillsDetailWithLastStepHeaderOutputDto, CollectBillsDetailWithLastStepDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFile([FromBody] CollectBillsDetialReportInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<CollectBillsDetailWithLastStepHeaderOutputDto, CollectBillsDetailWithLastStepDataOutputDto> result = await _collectBillsDetailWithLastStepHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        /// <summary>گزارش وضعیت پردازش و ارسال فایل‌ها با Query String</summary>
        /// <remarks>fromDateJalali و toDateJalali با قالب yyyy/MM/dd در Query String ارسال شوند. خروجی همان گزارش JSON روش POST است.</remarks>
        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CollectBillsDetailWithLastStepHeaderOutputDto, CollectBillsDetailWithLastStepDataOutputDto>>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetFileByQuery([FromQuery] CollectBillsDetialReportInputDto inputDto, CancellationToken cancellationToken)
        {
            return GetFile(inputDto, cancellationToken);
        }

        /// <summary>دانلود فایل ZIP موجود با نام فایل</summary>
        /// <remarks>نام کامل فایل همراه پسوند zip در مسیر ارسال شود؛ می‌توان از FileName گزارش استفاده کرد. پاسخ موفق محتوای باینری ZIP است و باید به‌صورت blob خوانده شود. فایل ناموجود خطا برمی‌گرداند.</remarks>
        [HttpPost, HttpGet]
        [Route("download/{fileName}")]
        public FileStreamResult DownloadByFileName(string fileName, CancellationToken cancellationToken)
        {
            CollectBillsGetZipFileInfo zipFileInfo = _collectBillsDetailFileDownloadHandler.Handle(fileName, cancellationToken);
            if (!System.IO.File.Exists(zipFileInfo.FilePath))
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotFoundFolder);
            }

            FileStream fileStream = new FileStream(zipFileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, _zipContentType, zipFileInfo.FileName);
        }

        /// <summary>شروع پردازش و ارسال اطلاعات روز گذشته در پس‌زمینه</summary>
        /// <remarks>ورودی ندارد. پاسخ 200 بدون بدنه فقط ثبت کار در صف را نشان می‌دهد و به معنی اتمام ارسال نیست. وضعیت را از get پیگیری کنید. برای شروع عملیات از POST استفاده کنید.</remarks>
        [HttpPost, HttpGet]
        [Route("send-file")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendFile(CancellationToken cancellationToken)
        {
            await _jobService.Initialize(DateTime.Now.AddDays(-1), cancellationToken);
            return Ok();
        }

        /// <summary>منسوخ: شروع ارسال با فاصله روز نسبت به امروز</summary>
        /// <remarks>در توسعه جدید استفاده نشود. day فاصله از امروز است: منفی یک یعنی دیروز و صفر یعنی امروز. پاسخ 200 بدون بدنه فقط ثبت کار در صف است. برای ارسال روز گذشته از send-file و برای دانلود تاریخ مشخص از file-create استفاده کنید؛ file-create عملیات ارسال را انجام نمی‌دهد.</remarks>
        [Obsolete("در توسعه جدید استفاده نشود؛ برای ارسال دیروز از send-file و برای دانلود تاریخ مشخص از file-create استفاده کنید.")]
        [HttpPost, HttpGet]
        [Route("send-file/{day}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendFileByDay(int day, CancellationToken cancellationToken)
        {
            await _jobService.Initialize(DateTime.Now.AddDays(day), cancellationToken);
            return Ok();
        }
    }
}
