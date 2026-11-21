using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Apk.Commands;
using Aban360.MeterPool.Domain.Features.Apk.Queries;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.MeterPool.Application.Features.Apk.Services;

namespace Aban360.Api.Controllers.V1.MeterPool.Apk.Commands
{
    [Route("v1/meter-apk-file")]
    public class MeterApkFileController : BaseController
    {
        private readonly IMeterApkFileService _meterApkFileService;
        private string _contentType = "application/vnd.android.package-archive";
        public MeterApkFileController(IMeterApkFileService meterApkFileService)
        {
            _meterApkFileService = meterApkFileService;
            _meterApkFileService.NotNull(nameof(meterApkFileService));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ApkInfoInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromForm] ApkInfoInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _meterApkFileService.CreateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-is-active/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UdpateIsActive(int id, CancellationToken cancellationToken)
        {
            await _meterApkFileService.SetActiveAsync(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<short>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(short id, CancellationToken cancellationToken)
        {
            await _meterApkFileService.DeleteAsync(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost, HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ApkInfoGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(short id, CancellationToken cancellationToken)
        {
            ApkInfoGetDto result = await _meterApkFileService.GetSingleAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ApkInfoGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<ApkInfoGetDto> result = await _meterApkFileService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("download/{id}")]
        public async Task<FileResult> Download(short id, CancellationToken cancellationToken)
        {
            ApkInfoGetDto result = await _meterApkFileService.GetSingleAsync(id, cancellationToken);
            var stream = new MemoryStream(result.FileContent);
            return File(stream, _contentType, result.Name);
        }

        [HttpPost, HttpGet]
        [Route("download/latest")]
        public async Task<FileResult> LatestDownload(CancellationToken cancellationToken)
        {
            ApkInfoGetDto? result = await _meterApkFileService.GetLatestAsync(cancellationToken);
            var stream = new MemoryStream(result.FileContent);
            return File(stream, _contentType, result.Name);
        }

        [HttpPost, HttpGet]
        [Route("validate/{version}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ApkInfoGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidateVersion(string version, CancellationToken cancellationToken)
        {
            MeterApkValidateOutputDto result = await _meterApkFileService.ValidateVersionAsync(version, cancellationToken);
            return Ok(result);
        }
    }
}
