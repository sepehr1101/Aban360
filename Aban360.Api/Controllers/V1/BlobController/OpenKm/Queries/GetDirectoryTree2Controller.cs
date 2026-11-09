using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations;
using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Queries
{
    [Route("v1/open-km")]
    public sealed class GetDirectoryTree2Controller : BaseController
    {
        private readonly IGetFilesByBillIdWithTitles _handler;
        private readonly IGetFilesDiscount _discountHandler;
        private readonly IGetFilesRequest _requestHandler;
        private readonly IDisplayRemovedFilesHandler _removedHandler;
        private readonly IArchiveFileTitleEnricher _titles;

        public GetDirectoryTree2Controller(IGetFilesByBillIdWithTitles handler, IGetFilesDiscount discountHandler,
            IGetFilesRequest requestHandler, IDisplayRemovedFilesHandler removedHandler, IArchiveFileTitleEnricher titles)
        {
            _handler = handler;
            _discountHandler = discountHandler;
            _requestHandler = requestHandler;
            _removedHandler = removedHandler;
            _titles = titles;
        }

        [HttpGet]
        [Route("directory-tree2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FileListWithTitlesResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string input, [Optional] string? trackNumber, CancellationToken cancellation)
        {
            return Ok(await _handler.Handle(input, trackNumber, CurrentUser, cancellation));
        }

        [HttpGet]
        [Route("discount-directory-tree2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FileListWithTitlesResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDiscountDirectoryTree(string input, CancellationToken cancellation)
        {
            var files = await _discountHandler.Handle(input, cancellation);
            return Ok(await _titles.Enrich(files, cancellation));
        }

        [HttpGet]
        [Route("request-directory-tree2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FileListWithTitlesResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRequestDirectoryTree(string input, CancellationToken cancellation)
        {
            var files = await _requestHandler.Handle(input, cancellation);
            return Ok(await _titles.Enrich(files, cancellation));
        }

        [HttpGet]
        [Route("reading-directory-tree2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FileListWithTitlesResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReadingDirectoryTree(string billId,
            [FromServices] IReadingArchiveService handler, CancellationToken cancellation)
        {
            return Ok(await handler.Get(billId, CurrentUser, cancellation));
        }

        [HttpPost]
        [Route("removed-directory-tree2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FileListWithTitlesResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRemovedTree([FromBody] RemovedFilesInput input, CancellationToken cancellation)
        {
            var files = await _removedHandler.Handle(input, cancellation);
            return Ok(await _titles.Enrich(files, cancellation));
        }
    }
}
