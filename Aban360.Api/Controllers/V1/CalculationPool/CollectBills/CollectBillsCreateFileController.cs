using Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Commands.Implementations;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.CollectBills
{
    [Route("v1/collect-bills")]
    public class CollectBillsCreateFileController : BaseController
    {
        private readonly ICollectBillsFileCreateHandler _collectBillsFileCreateHandler;
        private string _zipContentType = "application/zip";
        public CollectBillsCreateFileController(ICollectBillsFileCreateHandler collectBillsFileCreateHandler)
        {
            _collectBillsFileCreateHandler = collectBillsFileCreateHandler;
            _collectBillsFileCreateHandler.NotNull(nameof(collectBillsFileCreateHandler));
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
    }
}
