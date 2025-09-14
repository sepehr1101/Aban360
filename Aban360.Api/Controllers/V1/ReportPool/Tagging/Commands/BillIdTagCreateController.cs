using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.CustomerWarehouse.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging.Commands
{
    [Route("v1/bill-id-tag")]
    public class BillIdTagCreateController : BaseController
    {
        private readonly ICreateBillIdTagHandler _createHandler;

        public BillIdTagCreateController(ICreateBillIdTagHandler createHandler)
        {
            _createHandler = createHandler;
            _createHandler.NotNull(nameof(createHandler));
        }

        [Route("create")]
        [HttpPost, HttpPut]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreateBillIdTagDto>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateBillIdTagDto dto)
        {
            var id = await _createHandler.Handle(dto);
            return Ok(dto);
        }
    }
}
