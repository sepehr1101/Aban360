using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/app")]
    public class AppCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IAppCreateHandler _appCreateHandler;
        public AppCreateController(
            IUnitOfWork uow,
            IAppCreateHandler appCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _appCreateHandler = appCreateHandler;
            _appCreateHandler.NotNull(nameof(_appCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AppCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] AppCreateDto createDto, CancellationToken cancellationToken)
        {
            await _appCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
