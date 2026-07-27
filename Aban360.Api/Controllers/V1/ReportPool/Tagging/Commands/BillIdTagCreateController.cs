using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/bill-id-tag")]
    public class BillIdTagCreateController : BaseController
    {
        private readonly ICreateBillIdTagHandler _createHandler;
        private readonly IBillIdTagInsertExcelFileHandler _insertExcelFileHandler;
        public BillIdTagCreateController(
            ICreateBillIdTagHandler createHandler,
            IBillIdTagInsertExcelFileHandler insertExcelFileHandler)
        {
            _createHandler = createHandler;
            _createHandler.NotNull(nameof(createHandler));

            _insertExcelFileHandler = insertExcelFileHandler;
            _insertExcelFileHandler.NotNull(nameof(insertExcelFileHandler));
        }

        [Route("create")]
        [HttpPost, HttpPut]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreateBillIdTagDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateBillIdTagDto dto)
        {
            var id = await _createHandler.Handle(dto);
            return Ok(dto);
        }

        [Route("create-file")]
        [HttpPost, HttpPut]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreateBillIdTagDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateByFile([FromForm] BillIdTagInsertByExcelFileInputDto dto, CancellationToken cancellationToken)
        {
            await _insertExcelFileHandler.Handle(dto, CurrentUser, cancellationToken);
            return Ok(dto);
        }
    }
}
