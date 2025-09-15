using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/bill-id-tag")]
    public class BillIdTagDeleteController : BaseController
    {
        private readonly IDeleteBillIdTagHandler _deleteHandler;

        public BillIdTagDeleteController(IDeleteBillIdTagHandler deleteHandler)
        {
            _deleteHandler = deleteHandler;
            _deleteHandler.NotNull(nameof(deleteHandler));
        }

        [HttpDelete, HttpPost]
        [Route("delete/{id:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _deleteHandler.Handle(id);
            if (!result)
                return NotFound();
            return Ok(new { Id = id });
        }
    }
}
