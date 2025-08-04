using Aban360.Api.Exceptions;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.FlatReports.Queries
{
    [Route("v1/server-reports")]
    public class ServerReportsDownloadController : BaseController
    {
        private readonly IServerReportsGetFilePathHandler _serverReportsGetFilePathandler;
        public ServerReportsDownloadController(IServerReportsGetFilePathHandler serverReportsGetFilePathHandler)
        {
            _serverReportsGetFilePathandler = serverReportsGetFilePathHandler;
            _serverReportsGetFilePathandler.NotNull(nameof(_serverReportsGetFilePathandler));
        }

        [HttpGet]
        [Route("download/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServerReportsGetFilePathDto>), StatusCodes.Status200OK)]
        public async Task<FileResult> Download(Guid id,CancellationToken cancellationToken)
        {
            ServerReportsGetFilePathDto result = await _serverReportsGetFilePathandler.Handle(id, cancellationToken);
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            if (!System.IO.File.Exists(result.ReportPath))
            {
                throw new InvalidFilePathException(ExceptionLiterals.NotFoundFile);
            }

            string filePath = Path.GetFullPath(result.ReportPath);
            string fileName = Path.GetFileName(filePath);

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, excelContentType, fileName, enableRangeProcessing: true); // support resume and stream
           
        }
    }
}
