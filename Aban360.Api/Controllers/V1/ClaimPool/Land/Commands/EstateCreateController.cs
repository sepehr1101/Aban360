using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/estate")]
    public class EstateCreateController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateCreateHandler _createHandler;
        public EstateCreateController(
            IUnitOfWork uow,
            IEstateCreateHandler createHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _createHandler = createHandler;
            _createHandler.NotNull(nameof(_createHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] EstateCreateDto createDto, CancellationToken cancellationToken)
        {
            await _createHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
