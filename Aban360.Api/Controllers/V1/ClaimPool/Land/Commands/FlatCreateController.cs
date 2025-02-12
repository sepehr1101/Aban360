using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("falt")]
    public class FlatCreateController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IFlatCreateHandler _flatHandler;
        public FlatCreateController(
            IUnitOfWork uow,
            IFlatCreateHandler flatHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _flatHandler = flatHandler;
            _flatHandler.NotNull(nameof(flatHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] FlatCreateDto createDto, CancellationToken cancellationToken)
        {
            await _flatHandler.Handle(createDto,cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
