using Aban360.UserPool.Application.Features.AccessTree.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.AccessTree
{
    [Route("v1/module")]
    public class ModuleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IModuleService _service;
        public ModuleController(IUnitOfWork uow, IModuleService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ModuleCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ModuleCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPatch, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ModuleUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ModuleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpDelete, HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ModuleDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ModuleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ModuleGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Single(int id, CancellationToken cancellationToken)
        {
            ModuleGetDto module = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(module);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ModuleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ModuleGetDto> module = await _service.GetAllAsync(cancellationToken);
            return Ok(module);
        }

        [HttpGet, HttpPost]
        [Route("childrens/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ModuleWithSubModuleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChildrens(int id, CancellationToken cancellationToken)
        {
            IEnumerable<ModuleWithSubModuleGetDto> Modules = await _service.GetChildrenAsync(id, cancellationToken);
            return Ok(Modules);
        }
    }
}
