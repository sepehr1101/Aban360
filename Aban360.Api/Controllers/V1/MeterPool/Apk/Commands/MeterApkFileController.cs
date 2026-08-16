using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Apk.Handlers.Command.Create.Contracts;
using Aban360.MeterPool.Application.Features.Apk.Handlers.Command.Delete.Contracts;
using Aban360.MeterPool.Application.Features.Apk.Handlers.Command.Update.Contracts;
using Aban360.MeterPool.Application.Features.Apk.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Apk.Commands;
using Aban360.MeterPool.Domain.Features.Apk.Queries;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.MeterPool.Apk.Commands
{
    [Route("v1/meter-apk-file")]
    public class MeterApkFileController : BaseController
    {
        private readonly IMeterApkFileInsertHandler _meterApkFileDeleteHandler;
        private readonly IMeterApkFileSetIsActiveHandler _meterApkFileSetIsActiveHandler;
        private readonly IMeterApkFileRemoveHandler _meterApkFileRemoveHandler;
        private readonly IMeterApkInfoGetAllHandler _meterApkInfoGetAllHandler;
        private readonly IMeterApkInfoGetByIdHandler _meterApkInfoGetByIdHandler;
        private readonly IMeterApkDownloadGetByIdHandler _meterApkDownloadGetByIdHandler;
        private readonly IMeterApkInfoGetLatestHandler _meterApkInfoGetLatestHandler;
        private readonly IMeterApkInfoValidationHandler _meterApkInfoValidationHandler;
        private string _contentType = "application/vnd.android.package-archive";
        public MeterApkFileController(
            IMeterApkFileInsertHandler meterApkFileDeleteHandler,
            IMeterApkFileSetIsActiveHandler meterApkFileSetIsActiveHandler,
            IMeterApkFileRemoveHandler meterApkFileRemoveHandler,
            IMeterApkInfoGetAllHandler meterApkInfoGetAllHandler,
            IMeterApkInfoGetByIdHandler meterApkInfoGetByIdHandler,
            IMeterApkDownloadGetByIdHandler meterApkDownloadGetByIdHandler,
            IMeterApkInfoGetLatestHandler meterApkInfoGetLatestHandler,
            IMeterApkInfoValidationHandler meterApkInfoValidationHandler)
        {
            _meterApkFileDeleteHandler = meterApkFileDeleteHandler;
            _meterApkFileDeleteHandler.NotNull(nameof(meterApkFileDeleteHandler));

            _meterApkFileSetIsActiveHandler = meterApkFileSetIsActiveHandler;
            _meterApkFileSetIsActiveHandler.NotNull(nameof(meterApkFileSetIsActiveHandler));

            _meterApkFileRemoveHandler = meterApkFileRemoveHandler;
            _meterApkFileRemoveHandler.NotNull(nameof(meterApkFileRemoveHandler));

            _meterApkInfoGetAllHandler = meterApkInfoGetAllHandler;
            _meterApkInfoGetAllHandler.NotNull(nameof(meterApkInfoGetAllHandler));

            _meterApkInfoGetByIdHandler = meterApkInfoGetByIdHandler;
            _meterApkInfoGetByIdHandler.NotNull(nameof(meterApkInfoGetByIdHandler));

            _meterApkDownloadGetByIdHandler = meterApkDownloadGetByIdHandler;
            _meterApkDownloadGetByIdHandler.NotNull(nameof(meterApkDownloadGetByIdHandler));

            _meterApkInfoGetLatestHandler = meterApkInfoGetLatestHandler;
            _meterApkInfoGetLatestHandler.NotNull(nameof(meterApkInfoGetLatestHandler));

            _meterApkInfoValidationHandler = meterApkInfoValidationHandler;
            _meterApkInfoValidationHandler.NotNull(nameof(meterApkInfoValidationHandler));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ApkInfoInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromForm] ApkInfoInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _meterApkFileDeleteHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update-is-active/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UdpateIsActive(int id, CancellationToken cancellationToken)
        {
            await _meterApkFileSetIsActiveHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<short>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(short id, CancellationToken cancellationToken)
        {
            await _meterApkFileRemoveHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost, HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ApkInfoGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(short id, CancellationToken cancellationToken)
        {
            ApkInfoGetDto result = await _meterApkInfoGetByIdHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ApkInfoGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<ApkInfoGetDto> result = await _meterApkInfoGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("download/{id}")]
        public async Task<FileResult> Download(short id, CancellationToken cancellationToken)
        {
            ApkInfoGetDto result = await _meterApkInfoGetByIdHandler.Handle(id, cancellationToken);
            var stream = new MemoryStream(result.FileContent);
            return File(stream, _contentType, result.Name);

        }

        [HttpPost, HttpGet]
        [Route("download/latest")]
        public async Task<FileResult> LatestDownload(CancellationToken cancellationToken)
        {
            ApkInfoGetDto result = await _meterApkInfoGetLatestHandler.Handle(cancellationToken);
            var stream = new MemoryStream(result.FileContent);
            return File(stream, _contentType, result.Name);

        }

        [HttpPost, HttpGet]
        [Route("validate/{version}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ApkInfoGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidateVersion(string version, CancellationToken cancellationToken)
        {
            MeterApkValidateOutputDto result = await _meterApkInfoValidationHandler.Handle(version, cancellationToken);
            return Ok(result);
        }
    }
}
