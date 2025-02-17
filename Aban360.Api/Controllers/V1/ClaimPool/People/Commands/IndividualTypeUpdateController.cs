using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/individual-type")]
    public class IndividualTypeUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IIndividualTypeUpdateHandler _individualTypeHandler;
        public IndividualTypeUpdateController(
            IUnitOfWork uow,
            IIndividualTypeUpdateHandler individualTypeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _individualTypeHandler = individualTypeHandler;
            _individualTypeHandler.NotNull(nameof(individualTypeHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] IndividualTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _individualTypeHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
