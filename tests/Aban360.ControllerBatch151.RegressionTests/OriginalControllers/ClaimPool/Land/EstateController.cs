using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BeforeBatch151.ClaimPool.Land
{
    [Route("v1/estate")]
    public class EstateController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEstateDeleteHandler _estateDeleteHandler;
        private readonly IEstateCreateHandler _estateCreateHandler;
        private readonly IEstateUpdateHandler _estateUpdateHandler;
        private readonly IEstateGetAllHandler _estateGetAllHandler;
        private readonly IEstateGetSingleHandler _estateGetSingleHandler;

        public EstateController(
            IUnitOfWork unitOfWork,
            IEstateDeleteHandler estateDeleteHandler,
            IEstateCreateHandler estateCreateHandler,
            IEstateUpdateHandler estateUpdateHandler,
            IEstateGetAllHandler estateGetAllHandler,
            IEstateGetSingleHandler estateGetSingleHandler)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.NotNull(nameof(_unitOfWork));
            _estateDeleteHandler = estateDeleteHandler;
            _estateDeleteHandler.NotNull(nameof(_estateDeleteHandler));
            _estateCreateHandler = estateCreateHandler;
            _estateCreateHandler.NotNull(nameof(_estateCreateHandler));
            _estateUpdateHandler = estateUpdateHandler;
            _estateUpdateHandler.NotNull(nameof(_estateUpdateHandler));
            _estateGetAllHandler = estateGetAllHandler;
            _estateGetAllHandler.NotNull(nameof(_estateGetAllHandler));
            _estateGetSingleHandler = estateGetSingleHandler;
            _estateGetSingleHandler.NotNull(nameof(_estateGetSingleHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EstateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _estateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EstateCreateDto createDto, CancellationToken cancellationToken)
        {
            await _estateCreateHandler.Handle(createDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EstateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _estateUpdateHandler.Handle(updateDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EstateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<EstateGetDto> estate = await _estateGetAllHandler.Handle(cancellationToken);
            return Ok(estate);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            EstateGetDto estate = await _estateGetSingleHandler.Handle(id, cancellationToken);
            return Ok(estate);
        }
    }
}
