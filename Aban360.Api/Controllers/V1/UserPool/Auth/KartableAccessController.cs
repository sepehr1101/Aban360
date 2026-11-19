using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Auth.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.BaseEntities;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.Api.Filters;
using Aban360.UserPool.Application.Features.Auth.Services;

namespace Aban360.Api.Controllers.V1.UserPool.Auth
{
    [Route("v1/kartable-access")]
    public class KartableAccessController : BaseController
    {
        private readonly IKartableAccessService _kartableAccessService;
        private readonly IUnitOfWork _c0_uow;
        public KartableAccessController(IKartableAccessService kartableAccessService, IUnitOfWork c0_uow)
        {
            _kartableAccessService = kartableAccessService;
            _kartableAccessService.NotNull(nameof(kartableAccessService));
            _c0_uow = c0_uow;
            _c0_uow.NotNull(nameof(c0_uow));
        }

        [Route("update")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<KartableAccessUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] KartableAccessUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _kartableAccessService.UpdateAsync(inputDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(inputDto);
        }

        [Route("get/{userId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SelectionDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid userId, CancellationToken cancellationToken)
        {
            IEnumerable<SelectionDto> result = await _kartableAccessService.GetByUserIdAsync(userId, cancellationToken);
            return Ok(result);
        }
    }
}
