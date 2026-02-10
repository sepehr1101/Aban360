using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.OpenKm.Queries
{
    [Route("v1/open-km")]
    public class GetFilesByBillIdController:BaseController
    {
        private readonly IGetFilesByBillId _getFilesByBillIdHandler;
        private readonly IGetFilesDiscount _discountHandler;

        public GetFilesByBillIdController(
            IGetFilesByBillId getFilesByBillIdHandler,
            IGetFilesDiscount getFilesDiscountHandler)
        {
            _getFilesByBillIdHandler = getFilesByBillIdHandler;
            _getFilesByBillIdHandler.NotNull(nameof(getFilesByBillIdHandler));

            _discountHandler= getFilesDiscountHandler;
            _discountHandler.NotNull(nameof(getFilesDiscountHandler));
        }

        [HttpGet]
        [Route("directory-tree")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FileListResponse>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDirectoryTree(string input, CancellationToken cancellation)
        {
            FileListResponse result = await _getFilesByBillIdHandler.Handle(input, cancellation);
            return Ok(result);
        }

        [HttpGet]
        [Route("discount-directory-tree")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<FileListResponse>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDiscountDirectoryTree(string input, CancellationToken cancellation)
        {
            FileListResponse result = await _discountHandler.Handle(input, cancellation);
            return Ok(result);
        }
    }
}
