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
using Aban360.CalculationPool.Application.Features.Base;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.CalculationPool.CollectBills
{
    public class CollectBillsController : BaseController
    {
        private readonly ICollectBillsFileCreateHandler _c0_collectBillsFileCreateHandler;
        private readonly ICollectBillsDetailWithLastStepGetHadler _c0_collectBillsDetailWithLastStepHandler;
        private readonly ICollectBillsDetailFileDownloadHandler _c0_collectBillsDetailFileDownloadHandler;
        private string _c0_zipContentType = DirectoryLiterals.ZipFileContentType;
        private readonly ICollectBillsDetailJobService _c1_jobService;
        public CollectBillsController(ICollectBillsFileCreateHandler c0_collectBillsFileCreateHandler, ICollectBillsDetailWithLastStepGetHadler c0_collectBillsDetailWithLastStepHandler, ICollectBillsDetailFileDownloadHandler c0_collectBillsDetailFileDownloadHandler, ICollectBillsDetailJobService c1_jobService)
        {
            _c0_collectBillsFileCreateHandler = c0_collectBillsFileCreateHandler;
            _c0_collectBillsFileCreateHandler.NotNull(nameof(c0_collectBillsFileCreateHandler));
            _c0_collectBillsDetailWithLastStepHandler = c0_collectBillsDetailWithLastStepHandler;
            _c0_collectBillsDetailWithLastStepHandler.NotNull(nameof(c0_collectBillsDetailWithLastStepHandler));
            _c0_collectBillsDetailFileDownloadHandler = c0_collectBillsDetailFileDownloadHandler;
            _c0_collectBillsDetailFileDownloadHandler.NotNull(nameof(c0_collectBillsDetailFileDownloadHandler));
            _c1_jobService = c1_jobService;
            _c1_jobService.NotNull(nameof(c1_jobService));
        }

        [HttpPost, HttpGet]
        [Route("~/v1/collect-bills/file-create")]
        public async Task<FileStreamResult> CreateFile(CollectBillsByDateJalaliInputDto inputDto, CancellationToken cancellationToken)
        {
            ConvertDate.JalaliToDateTime(inputDto.DateJalali, true);
            CollectBillsGetZipFileInfo zipFileInfo = await _c0_collectBillsFileCreateHandler.Handle(inputDto.DateJalali, CurrentUser, cancellationToken);
            if (!System.IO.File.Exists(zipFileInfo.FilePath))
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotFoundFolder);
            }

            FileStream fileStream = new FileStream(zipFileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, _c0_zipContentType, zipFileInfo.FileName);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/collect-bills/get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CollectBillsDetailWithLastStepHeaderOutputDto, CollectBillsDetailWithLastStepDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFile(CollectBillsDetialReportInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<CollectBillsDetailWithLastStepHeaderOutputDto, CollectBillsDetailWithLastStepDataOutputDto> result = await _c0_collectBillsDetailWithLastStepHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/collect-bills/download/{fileName}")]
        public FileStreamResult DownloadByFileName(string fileName, CancellationToken cancellationToken)
        {
            CollectBillsGetZipFileInfo zipFileInfo = _c0_collectBillsDetailFileDownloadHandler.Handle(fileName, cancellationToken);
            if (!System.IO.File.Exists(zipFileInfo.FilePath))
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotFoundFolder);
            }

            FileStream fileStream = new FileStream(zipFileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, _c0_zipContentType, zipFileInfo.FileName);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/collect-bills/send-file")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<CollectBillsDetailGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendFile(CancellationToken cancellationToken)
        {
            await _c1_jobService.Initialize(DateTime.Now.AddDays(-1), cancellationToken);
            return Ok();
        }

        [HttpPost, HttpGet]
        [Route("~/v1/collect-bills/send-file/{day}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<CollectBillsDetailGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendFileByDay(int day, CancellationToken cancellationToken)
        {
            await _c1_jobService.Initialize(DateTime.Now.AddDays(day), cancellationToken);
            return Ok();
        }
    }
}
