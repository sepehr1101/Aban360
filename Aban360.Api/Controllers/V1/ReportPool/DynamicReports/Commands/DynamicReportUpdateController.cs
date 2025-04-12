using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Commands.Update.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Commands;
using Aban360.ReportPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.DynamicReports.Commands
{
    [Route("v1/dynamic-Report")]
    public class DynamicReportUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDynamicReportUpdateHandler _tariffConstantUpdateHandler;
        public DynamicReportUpdateController(
            IUnitOfWork uow,
            IDynamicReportUpdateHandler tariffConstantUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffConstantUpdateHandler = tariffConstantUpdateHandler;
            _tariffConstantUpdateHandler.NotNull(nameof(tariffConstantUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DynamicReportUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] DynamicReportUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _tariffConstantUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
