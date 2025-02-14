using Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Registration.Commands
{
    [Route("v1/use-state")]
    public class UseStateDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUseEstateDeleteHandler _useEstateHandler;
        public UseStateDeleteController(
            IUnitOfWork uow,
            IUseEstateDeleteHandler useEstateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _useEstateHandler = useEstateHandler;
            _useEstateHandler.NotNull(nameof(useEstateHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] UseStateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _useEstateHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
