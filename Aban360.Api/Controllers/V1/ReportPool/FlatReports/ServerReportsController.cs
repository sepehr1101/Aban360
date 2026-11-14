using Aban360.ReportPool.Application.Features.FlatReports.Services;
using Aban360.Api.Exceptions;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.FlatReports
{
    public class ServerReportsController : BaseController
    {
        private readonly IServerReportsService _service;
        private readonly IConfiguration _c1_configuration;
        public ServerReportsController(IServerReportsService service, IConfiguration c1_configuration)
        {
            service.NotNull(nameof(service));
            _service = service;
            c1_configuration.NotNull(nameof(c1_configuration));
            _c1_configuration = c1_configuration;
        }

        [HttpGet]
        [Route("~/v1/server-reports/download/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ServerReportsGetFilePathDto>), StatusCodes.Status200OK)]
        public async Task<FileResult> Download(Guid id, CancellationToken cancellationToken)
        {
            ServerReportsGetFilePathDto result = await _service.GetFilePathAsync(id, cancellationToken);
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
            IEnumerable<ServerReportsGetAllDto> result = await _service.GetAllByThresholdAsync(CurrentUser.UserId, tresholdDay + 1, cancellationToken);
            return Ok(result);
        }

        [Obsolete]
        [HttpGet]
        [Route("~/v1/server-reports/by-user-id-old")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ServerReportsGetAllDto>>), StatusCodes.Status200OK)]
        [ActionName("GetRaw")]
        public async Task<IActionResult> GetRawFromServerReportsGetAll2(CancellationToken cancellationToken)
        {
            IEnumerable<ServerReportsGetAllDto> result = await _service.GetAllAsync(CurrentUser.UserId, cancellationToken);
            return Ok(result);
        }
    }
}
