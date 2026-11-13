using Aban360.UserPool.Application.Features.AccessTree.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree
{
    public class SubModuleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISubModuleService _service;
        public SubModuleController(IUnitOfWork uow, ISubModuleService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("~/v1/sub-module/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SubModuleCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(createDto);
        }

        [HttpDelete, HttpPost]
        [Route("~/v1/sub-module/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SubModuleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(deleteDto);
        }

        [HttpPatch, HttpPost]
        [Route("~/v1/sub-module/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SubModuleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/sub-module/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SubModuleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var subModule = await _service.GetAllAsync(cancellationToken);
            return Ok(subModule);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/sub-module/childrens/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SubModuleWithEndPointGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChildrens(int id, CancellationToken cancellationToken)
        {
            IEnumerable<SubModuleWithEndPointGetDto> SubModules = await _service.GetChildrenAsync(id, cancellationToken);
            return Ok(SubModules);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/sub-model/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Single(int id, CancellationToken cancellationToken)
        {
            var subModule = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(subModule);
        }
    }
}
