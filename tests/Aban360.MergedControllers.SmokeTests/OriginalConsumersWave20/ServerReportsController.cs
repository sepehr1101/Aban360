using Aban360.Api.Controllers.V1;
using Aban360.Api.Exceptions;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.FlatReports.Handler.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Filters;

namespace BeforeServiceConsumersWave20.Aban360.Api.Controllers.V1.ReportPool.FlatReports
{
    public class ServerReportsController : BaseController
    {
        private readonly IServerReportsGetFilePathHandler _c0_serverReportsGetFilePathandler;
        private readonly IServerReportsGetAllByThresholdHandler _c1_serverReportsGetAllByThresholdHandler;
        private readonly IConfiguration _c1_configuration;
        private readonly IServerReportsGetAllHandler _c2_serverReportsGetAllHandler;
        public ServerReportsController(IServerReportsGetFilePathHandler c0_serverReportsGetFilePathHandler, IServerReportsGetAllByThresholdHandler c1_serverReportsGetAllByThresholdHandler, IConfiguration c1_configuration, IServerReportsGetAllHandler c2_serverReportsGetAllHandler)
        {
            _c0_serverReportsGetFilePathandler = c0_serverReportsGetFilePathHandler;
            _c0_serverReportsGetFilePathandler.NotNull(nameof(_c0_serverReportsGetFilePathandler));
            _c1_serverReportsGetAllByThresholdHandler = c1_serverReportsGetAllByThresholdHandler;
            _c1_serverReportsGetAllByThresholdHandler.NotNull(nameof(_c1_serverReportsGetAllByThresholdHandler));
            _c1_configuration = c1_configuration;
            _c1_configuration.NotNull(nameof(_c1_configuration));
            _c2_serverReportsGetAllHandler = c2_serverReportsGetAllHandler;
            _c2_serverReportsGetAllHandler.NotNull(nameof(_c2_serverReportsGetAllHandler));
        }

        [HttpGet]
        [Route("~/v1/server-reports/download/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServerReportsGetFilePathDto>), StatusCodes.Status200OK)]
        public async Task<FileResult> Download(Guid id, CancellationToken cancellationToken)
        {
            ServerReportsGetFilePathDto result = await _c0_serverReportsGetFilePathandler.Handle(id, cancellationToken);
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

        [HttpGet]
        [Route("~/v1/server-reports/by-user-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ServerReportsGetAllDto>>), StatusCodes.Status200OK)]
        [ActionName("GetRaw")]
        public async Task<IActionResult> GetRawFromServerReportsGetAllByThreshold1(CancellationToken cancellationToken)
        {
            int tresholdDay = int.Parse(_c1_configuration["FileManagement:ExcelExpireDay"]);
            IEnumerable<ServerReportsGetAllDto> result = await _c1_serverReportsGetAllByThresholdHandler.Handle(CurrentUser.UserId, tresholdDay + 1, cancellationToken);
            return Ok(result);
        }

        [Obsolete]
        [HttpGet]
        [Route("~/v1/server-reports/by-user-id-old")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ServerReportsGetAllDto>>), StatusCodes.Status200OK)]
        [ActionName("GetRaw")]
        public async Task<IActionResult> GetRawFromServerReportsGetAll2(CancellationToken cancellationToken)
        {
            IEnumerable<ServerReportsGetAllDto> result = await _c2_serverReportsGetAllHandler.Handle(CurrentUser.UserId, cancellationToken);
            return Ok(result);
        }
    }
}
