using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree.Commands
{
    [Route("v1/app")]
    public class AppDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IAppDeleteHandler _appDeleteHandler;
        public AppDeleteController(
            IUnitOfWork uow,
            IAppDeleteHandler appDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _appDeleteHandler = appDeleteHandler;
            _appDeleteHandler.NotNull(nameof(_appDeleteHandler));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] AppDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _appDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
