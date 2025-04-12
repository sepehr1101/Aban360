using Aban360.Common.ApplicationUser;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Create.Contracts;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Commands;
using Aban360.ReportPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.DynamicGenerator.Commands
{
    [Route("v1/dynamic-Report")]
    public class DynamicReportCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDynamicReportCreateHandler _tariffConstantCreateHandler;
        public DynamicReportCreateController(
            IUnitOfWork uow,
            IDynamicReportCreateHandler tariffConstantCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffConstantCreateHandler = tariffConstantCreateHandler;
            _tariffConstantCreateHandler.NotNull(nameof(tariffConstantCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<DynamicReportCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] DynamicReportCreateDto createDto, CancellationToken cancellationToken)
        {
            IAppUser currentUser = CurrentUser;
            await _tariffConstantCreateHandler.Handle(currentUser, createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
