using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("constructionType")]
    public class ConstructionTypeDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IConstructionTypeDeleteHandler _deleteHandler;
        public ConstructionTypeDeleteController(
            IUnitOfWork uow,
            IConstructionTypeDeleteHandler deleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _deleteHandler = deleteHandler;
            _deleteHandler.NotNull(nameof(_deleteHandler));
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] ConstructionTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _deleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
