using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy.Commands
{
    [Route("v1/municipality")]
    public class MunicipalityCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMunicipalityCreateHandler _municipalityCreateHandler;
        public MunicipalityCreateController(
            IUnitOfWork uow,
            IMunicipalityCreateHandler municipalityCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _municipalityCreateHandler = municipalityCreateHandler;
            _municipalityCreateHandler.NotNull(nameof(municipalityCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MunicipalityCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MunicipalityCreateDto createDto , CancellationToken cancellationToken)
        {
            await _municipalityCreateHandler.Handle(createDto, cancellationToken);  
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
