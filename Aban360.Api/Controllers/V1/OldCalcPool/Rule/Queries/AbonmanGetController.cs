using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/Abonman")]
    public class AbonmanGetController : BaseController
    {
        private readonly IAbonmanGetAllHandler _AbonmanGetAllHandler;
        private readonly IAbonmanGetHandler _AbonmanGetHandler;
        public AbonmanGetController(
            IAbonmanGetAllHandler AbonmanGetAllHandler,
            IAbonmanGetHandler abonmanGetHandler)
        {
            _AbonmanGetAllHandler = AbonmanGetAllHandler;
            _AbonmanGetAllHandler.NotNull(nameof(AbonmanGetAllHandler));

            _AbonmanGetHandler = abonmanGetHandler;
            _AbonmanGetHandler.NotNull(nameof(abonmanGetHandler));
        }

        [HttpGet, HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<AbonmanGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<AbonmanGetDto> result = await _AbonmanGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbonmanGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            AbonmanGetDto result = await _AbonmanGetHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
