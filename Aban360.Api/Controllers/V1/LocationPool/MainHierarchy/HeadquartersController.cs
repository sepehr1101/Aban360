using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Delete.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.LocationPool.MainHierarchy
{
    [Route("v1/headquarters")]
    public class HeadquartersController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IHeadquarterCreateHandler _headquarterCreateHandler;
        private readonly IHeadquarterUpdateHandler _headquarterUpdateHandler;
        private readonly IHeadquarterDeleteHandler _headquarterDeleteHandler;
        private readonly IHeadquarterGetSingleHandler _headquarterGetSingleHandler;
        private readonly IHeadquarterGetAllHandler _headquarterGetAllHandler;

        public HeadquartersController(
            IUnitOfWork uow,
            IHeadquarterCreateHandler headquarterCreateHandler,
            IHeadquarterUpdateHandler headquarterUpdateHandler,
            IHeadquarterDeleteHandler headquarterDeleteHandler,
            IHeadquarterGetSingleHandler headquarterGetSingleHandler,
            IHeadquarterGetAllHandler headquarterGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _headquarterCreateHandler = headquarterCreateHandler;
            _headquarterCreateHandler.NotNull(nameof(headquarterCreateHandler));

            _headquarterUpdateHandler = headquarterUpdateHandler;
            _headquarterUpdateHandler.NotNull(nameof(headquarterUpdateHandler));

            _headquarterDeleteHandler = headquarterDeleteHandler;
            _headquarterDeleteHandler.NotNull(nameof(headquarterDeleteHandler));

            _headquarterGetSingleHandler = headquarterGetSingleHandler;
            _headquarterGetSingleHandler.NotNull(nameof(headquarterGetSingleHandler));

            _headquarterGetAllHandler = headquarterGetAllHandler;
            _headquarterGetAllHandler.NotNull(nameof(headquarterGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HeadquarterCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] HeadquarterCreateDto createDto, CancellationToken cancellationToken)
        {
            await _headquarterCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }


        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HeadquarterUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] HeadquarterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _headquarterUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HeadquarterDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] HeadquarterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _headquarterDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HeadquarterGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            HeadquarterGetDto result = await _headquarterGetSingleHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<HeadquarterGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<HeadquarterGetDto> result = await _headquarterGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
