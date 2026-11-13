using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.BaseEntities;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.UserPool.Auth
{
    public class KartableAccessController : BaseController
    {
        private readonly IKartableAccessUpdateHandler _c0_kartableAccessUpdateHandler;
        private readonly IUnitOfWork _c0_uow;
        private readonly IRequestKartableGetListByUserIdHandler _c1_requestKartableGetListByUserIdHandler;
        public KartableAccessController(IKartableAccessUpdateHandler c0_kartableAccessUpdateHandler, IUnitOfWork c0_uow, IRequestKartableGetListByUserIdHandler c1_requestKartableGetListByUserIdHandler)
        {
            _c0_kartableAccessUpdateHandler = c0_kartableAccessUpdateHandler;
            _c0_kartableAccessUpdateHandler.NotNull(nameof(c0_kartableAccessUpdateHandler));
            _c0_uow = c0_uow;
            _c0_uow.NotNull(nameof(c0_uow));
            _c1_requestKartableGetListByUserIdHandler = c1_requestKartableGetListByUserIdHandler;
            _c1_requestKartableGetListByUserIdHandler.NotNull(nameof(c1_requestKartableGetListByUserIdHandler));
        }

        [Route("~/v1/kartable-access/update")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<KartableAccessUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] KartableAccessUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _c0_kartableAccessUpdateHandler.Handle(inputDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(inputDto);
        }

        [Route("~/v1/kartable-access/get/{userId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SelectionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid userId, CancellationToken cancellationToken)
        {
            IEnumerable<SelectionDto> result = await _c1_requestKartableGetListByUserIdHandler.Handle(userId, cancellationToken);
            return Ok(result);
        }
    }
}
