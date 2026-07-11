using Aban360.ClaimPool.Application.Features.AssessmentApk.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.AssessmentApk.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.AssessmentApk
{
    [Route("v1/assessment")]
    public class AssessmentApkController : BaseController
    {
        private readonly IAssessmentApkValidateVersionHandler _validateVersionHandler;
        private readonly IAssessmentApkLatestDownloadHandler _latestDownloadHandler;
        private string _apkContentType = "application/vnd.android.package-archive";
        public AssessmentApkController(
            IAssessmentApkValidateVersionHandler validateVersionHandler,
            IAssessmentApkLatestDownloadHandler latestDownloadHandler)
        {
            _validateVersionHandler = validateVersionHandler;
            _validateVersionHandler.NotNull(nameof(validateVersionHandler));

            _latestDownloadHandler = latestDownloadHandler;
            _latestDownloadHandler.NotNull(nameof(latestDownloadHandler));
        }

        [HttpPost, HttpGet]
        [Route("apk-validate/{userVersion}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AssessmentApkValidateOutputDto>), StatusCodes.Status200OK)]
        public IActionResult ValidateVersion(string userVersion, CancellationToken cancellationToken)
        {
            AssessmentApkValidateOutputDto result = _validateVersionHandler.Handle(userVersion, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("apk-latest")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UsageGroup1InsertDto>), StatusCodes.Status200OK)]
        public FileResult GetLatestApk(CancellationToken cancellationToken)
        {
            var (fileStream, fileName) = _latestDownloadHandler.Handle(cancellationToken);
            return File(fileStream, _apkContentType, fileName, enableRangeProcessing: true);
        }
    }
}
