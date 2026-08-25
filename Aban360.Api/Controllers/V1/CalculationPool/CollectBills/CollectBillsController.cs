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
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.CollectBills
{
    [Route("v1/collect-bills")]
    public class CollectBillsController : BaseController
    {
        private readonly ICollectBillsFileCreateHandler _collectBillsFileCreateHandler;
        private readonly ICollectBillsDetailWithLastStepGetHadler _collectBillsDetailWithLastStepHandler;
        private readonly ICollectBillsDetailFileDownloadHandler _collectBillsDetailFileDownloadHandler;
        private string _zipContentType = DirectoryLiterals.ZipFileContentType;
        public CollectBillsController(
            ICollectBillsFileCreateHandler collectBillsFileCreateHandler,
            ICollectBillsDetailWithLastStepGetHadler collectBillsDetailWithLastStepHandler,
            ICollectBillsDetailFileDownloadHandler collectBillsDetailFileDownloadHandler)
        {
            _collectBillsFileCreateHandler = collectBillsFileCreateHandler;
            _collectBillsFileCreateHandler.NotNull(nameof(collectBillsFileCreateHandler));

            _collectBillsDetailWithLastStepHandler = collectBillsDetailWithLastStepHandler;
            _collectBillsDetailWithLastStepHandler.NotNull(nameof(collectBillsDetailWithLastStepHandler));

            _collectBillsDetailFileDownloadHandler = collectBillsDetailFileDownloadHandler;
            _collectBillsDetailFileDownloadHandler.NotNull(nameof(collectBillsDetailFileDownloadHandler));
        }

        [HttpPost, HttpGet]
        [Route("file-create")]
        public async Task<FileStreamResult> CreateFile(string reportDateJalali, CancellationToken cancellationToken)
        {
            CollectBillsGetZipFileInfo zipFileInfo = await _collectBillsFileCreateHandler.Handle(reportDateJalali, CurrentUser, cancellationToken);
            if (!System.IO.File.Exists(zipFileInfo.FilePath))
            {
                throw new InvalidBillCommandException(ExceptionLiterals.NotFoundFolder);
            }

            FileStream fileStream = new FileStream(zipFileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(fileStream, _zipContentType, zipFileInfo.FileName);

        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CollectBillsDetailWithLastStepHeaderOutputDto, CollectBillsDetailWithLastStepDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendFile(CollectBillsDetialReportInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<CollectBillsDetailWithLastStepHeaderOutputDto, CollectBillsDetailWithLastStepDataOutputDto> result = await _collectBillsDetailWithLastStepHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }

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
    }
}
