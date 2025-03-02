using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/profession")]
    public class ProfessionCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProfessionCreateHandler _professionHandler;
        public ProfessionCreateController(
            IUnitOfWork uow,
            IProfessionCreateHandler professionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _professionHandler = professionHandler;
            _professionHandler.NotNull(nameof(professionHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ProfessionCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ProfessionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _professionHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
