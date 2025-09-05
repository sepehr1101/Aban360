using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
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
        public GetFilesByBillIdController(IGetFilesByBillId getFilesByBillIdHandler)
        {
            _getFilesByBillIdHandler = getFilesByBillIdHandler;
            _getFilesByBillIdHandler.NotNull(nameof(getFilesByBillIdHandler));
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
    }
}
