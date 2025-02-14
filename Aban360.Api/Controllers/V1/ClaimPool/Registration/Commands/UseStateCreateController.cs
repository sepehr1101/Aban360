using Aban360.ClaimPool.Application.Features.Registration.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Registration.Dto.Command;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Registration.Commands
{
    [Route("v1/use-state")]
    public class UseStateCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUseStateCreateHandler _useEstateHandler;
        public UseStateCreateController(
            IUnitOfWork uow,
            IUseStateCreateHandler useEstateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _useEstateHandler = useEstateHandler;
            _useEstateHandler.NotNull(nameof(useEstateHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] UseStateCreateDto createDto, CancellationToken cancellationToken)
        {
            await _useEstateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
