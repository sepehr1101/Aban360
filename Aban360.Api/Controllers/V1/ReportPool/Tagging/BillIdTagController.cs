using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging
{
    public class BillIdTagController : BaseController
    {
        private readonly ICreateBillIdTagHandler _c0_createHandler;
        private readonly IBillIdTagInsertExcelFileHandler _c0_insertExcelFileHandler;
        private readonly IDeleteBillIdTagHandler _c1_deleteHandler;
        private readonly IBillIdTagRemoveByTagIdsHandler _c1_billIdTagRemoveByTagIdsHandler;
        private readonly IGetBillIdTagHandler _c2_getHandler;
        public BillIdTagController(ICreateBillIdTagHandler c0_createHandler, IBillIdTagInsertExcelFileHandler c0_insertExcelFileHandler, IDeleteBillIdTagHandler c1_deleteHandler, IBillIdTagRemoveByTagIdsHandler c1_billIdTagRemoveByTagIdsHandler, IGetBillIdTagHandler c2_getHandler)
        {
            _c0_createHandler = c0_createHandler;
            _c0_createHandler.NotNull(nameof(c0_createHandler));
            _c0_insertExcelFileHandler = c0_insertExcelFileHandler;
            _c0_insertExcelFileHandler.NotNull(nameof(c0_insertExcelFileHandler));
            _c1_deleteHandler = c1_deleteHandler;
            _c1_deleteHandler.NotNull(nameof(c1_deleteHandler));
            _c1_billIdTagRemoveByTagIdsHandler = c1_billIdTagRemoveByTagIdsHandler;
            _c1_billIdTagRemoveByTagIdsHandler.NotNull(nameof(c1_billIdTagRemoveByTagIdsHandler));
            _c2_getHandler = c2_getHandler;
            _c2_getHandler.NotNull(nameof(c2_getHandler));
        }

        [Route("~/v1/bill-id-tag/create")]
        [HttpPost, HttpPut]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreateBillIdTagDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateBillIdTagDto dto)
        {
            var id = await _c0_createHandler.Handle(dto);
            return Ok(dto);
        }

        [Route("~/v1/bill-id-tag/create-file")]
        [HttpPost, HttpPut]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreateBillIdTagDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateByFile([FromForm] BillIdTagInsertByExcelFileInputDto dto, CancellationToken cancellationToken)
        {
            await _c0_insertExcelFileHandler.Handle(dto, CurrentUser, cancellationToken);
            return Ok(dto);
        }

        [HttpDelete, HttpPost]
        [Route("~/v1/bill-id-tag/delete/{id:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<long>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _c1_deleteHandler.Handle(id);
            if (!result)
                return NotFound();
            return Ok(new { Id = id });
        }

        [HttpDelete, HttpPost]
        [Route("~/v1/bill-id-tag/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillIdTagRemoveByTagIdsOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteByTagIds([FromBody] BillIdTagRemoveByTagIdsInputDto inputDto, CancellationToken cancellationToken)
        {
            BillIdTagRemoveByTagIdsOutputDto result = await _c1_billIdTagRemoveByTagIdsHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/bill-id-tag/get/{billId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<BillIdTagDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillId(string billId)
        {
            var tags = await _c2_getHandler.Handle(billId);
            return Ok(tags);
        }
    }
}
