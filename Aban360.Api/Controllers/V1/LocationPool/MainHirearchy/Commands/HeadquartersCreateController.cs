using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHirearchy.Commands
{
    [Route("v1/headquarters")]
    public class HeadquartersCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IHeadquarterCreateHandler _headquarterCreateHandler;
        public HeadquartersCreateController(
            IUnitOfWork uow,
            IHeadquarterCreateHandler headquarterCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _headquarterCreateHandler = headquarterCreateHandler;
            _headquarterCreateHandler.NotNull(nameof(headquarterCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] HeadquarterCreateDto createDto, CancellationToken cancellationToken)
        {
            await _headquarterCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

    }
}
