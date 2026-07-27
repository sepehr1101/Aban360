using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/bill-id-tag")]
    public class BillIdTagDeleteController : BaseController
    {
        private readonly IDeleteBillIdTagHandler _deleteHandler;
        private readonly IBillIdTagRemoveByTagIdsHandler _billIdTagRemoveByTagIdsHandler;
        public BillIdTagDeleteController(
            IDeleteBillIdTagHandler deleteHandler,
            IBillIdTagRemoveByTagIdsHandler billIdTagRemoveByTagIdsHandler)
        {
            _deleteHandler = deleteHandler;
            _deleteHandler.NotNull(nameof(deleteHandler));

            _billIdTagRemoveByTagIdsHandler = billIdTagRemoveByTagIdsHandler;
            _billIdTagRemoveByTagIdsHandler.NotNull(nameof(billIdTagRemoveByTagIdsHandler));
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

        [HttpDelete, HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillIdTagRemoveByTagIdsOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteByTagIds([FromBody] BillIdTagRemoveByTagIdsInputDto inputDto, CancellationToken cancellationToken)
        {
            BillIdTagRemoveByTagIdsOutputDto result = await _billIdTagRemoveByTagIdsHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
