using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Commands.Delete.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Commands;
using Aban360.ReportPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.DynamicReports.Commands
{
    [Route("v1/dynamic-Report")]
    public class DynamicReportDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDynamicReportDeleteHandler _tariffConstantDeleteHandler;
        public DynamicReportDeleteController(
            IUnitOfWork uow,
            IDynamicReportDeleteHandler tariffConstantDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffConstantDeleteHandler = tariffConstantDeleteHandler;
            _tariffConstantDeleteHandler.NotNull(nameof(tariffConstantDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DynamicReportDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] DynamicReportDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _tariffConstantDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
