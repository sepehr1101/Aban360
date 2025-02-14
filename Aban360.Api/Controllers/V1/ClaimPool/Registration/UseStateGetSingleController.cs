using Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Registration
{
    [Route("v1/use-state")]
    public class UseStateGetSingleController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUseEstateGetSingleHandler _useEstateHandler;
        public UseStateGetSingleController(
            IUnitOfWork uow,
            IUseEstateGetSingleHandler useEstateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _useEstateHandler = useEstateHandler;
            _useEstateHandler.NotNull(nameof(useEstateHandler));
        }

        [HttpGet,HttpPost]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var useEstate = await _useEstateHandler.Handle(id,cancellationToken);
            return Ok(useEstate);
        }
    }
}
