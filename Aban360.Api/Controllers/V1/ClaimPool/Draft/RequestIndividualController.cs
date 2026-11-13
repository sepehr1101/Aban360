using Aban360.ClaimPool.Application.Features.Draft.Services;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft
{
    [Route("v1/request-individual")]
    public class RequestIndividualController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestIndividualService _service;
        public RequestIndividualController(IUnitOfWork uow, IRequestIndividualService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] IndividualRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(CurrentUser,createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] IndividualRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(CurrentUser,updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IndividualRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] IndividualRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
