using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Auth.Commands
{
    [Route("v1/kartable-access")]
    public class KartableAccessUpdateController : BaseController
    {
        private readonly IKartableAccessUpdateHandler _kartableAccessUpdateHandler;
        private readonly IUnitOfWork _uow;
        public KartableAccessUpdateController(
            IKartableAccessUpdateHandler kartableAccessUpdateHandler,
           IUnitOfWork uow)
        {
            _kartableAccessUpdateHandler = kartableAccessUpdateHandler;
            _kartableAccessUpdateHandler.NotNull(nameof(kartableAccessUpdateHandler));

            _uow = uow;
            _uow.NotNull(nameof(uow));
        }

        [Route("update")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<KartableAccessUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] KartableAccessUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _kartableAccessUpdateHandler.Handle(inputDto,  cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(inputDto);
        }
    }
}
