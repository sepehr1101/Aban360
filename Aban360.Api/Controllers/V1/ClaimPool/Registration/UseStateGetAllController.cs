using Aban360.ClaimPool.Application.Features.Registration.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Registration
{
    [Route("v1/use-state")]
    public class UseStateGetAllController:BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUseEstateGetAllHandler _useEstateHandler;
        public UseStateGetAllController(
            IUnitOfWork uow,
            IUseEstateGetAllHandler useEstateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _useEstateHandler = useEstateHandler;
            _useEstateHandler.NotNull(nameof(useEstateHandler));
        }

        [HttpGet,HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
        {
            var useEstate=await _useEstateHandler.Handle(cancellationToken);
            return Ok(useEstate);
        }
    }}
