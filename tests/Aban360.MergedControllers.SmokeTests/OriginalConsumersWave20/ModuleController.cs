using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumersWave20.Aban360.Api.Controllers.V1.UserPool.AccessTree
{
    [Route("v1/module")]
    public class ModuleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IModuleCreateHandler _moduleCreateHandler;
        private readonly IModuleUpdateHandler _moduleUpdateHandler;
        private readonly IModuleDeleteHandler _moduleDeleteHandler;
        private readonly IModuleGetSingleHandler _moduleGetSingleHandler;
        private readonly IModuleGetAllHandler _moduleGetAllHandler;
        private readonly IModuleGetChildrensHandler _moduleGetChildrensHandler;

        public ModuleController(
            IUnitOfWork uow,
            IModuleCreateHandler moduleCreateHandler,
            IModuleUpdateHandler moduleUpdateHandler,
            IModuleDeleteHandler moduleDeleteHandler,
            IModuleGetSingleHandler moduleGetSingleHandler,
            IModuleGetAllHandler moduleGetAllHandler,
            IModuleGetChildrensHandler moduleGetChildrensHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _moduleCreateHandler = moduleCreateHandler;
            _moduleCreateHandler.NotNull(nameof(moduleCreateHandler));

            _moduleUpdateHandler = moduleUpdateHandler;
            _moduleUpdateHandler.NotNull(nameof(moduleUpdateHandler));

            _moduleDeleteHandler = moduleDeleteHandler;
            _moduleDeleteHandler.NotNull(nameof(moduleDeleteHandler));

            _moduleGetSingleHandler = moduleGetSingleHandler;
            _moduleGetSingleHandler.NotNull(nameof(moduleGetSingleHandler));

            _moduleGetAllHandler = moduleGetAllHandler;
            _moduleGetAllHandler.NotNull(nameof(moduleGetAllHandler));

            _moduleGetChildrensHandler = moduleGetChildrensHandler;
            _moduleGetChildrensHandler.NotNull(nameof(_moduleGetChildrensHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ModuleCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ModuleCreateDto createDto, CancellationToken cancellationToken)
        {
            await _moduleCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPatch, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ModuleUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] ModuleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _moduleUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpDelete, HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ModuleDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] ModuleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _moduleDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ModuleGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Single(int id, CancellationToken cancellationToken)
        {
            ModuleGetDto module = await _moduleGetSingleHandler.Handle(id, cancellationToken);
            return Ok(module);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ModuleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ModuleGetDto> module = await _moduleGetAllHandler.Handle(cancellationToken);
            return Ok(module);
        }

        [HttpGet, HttpPost]
        [Route("childrens/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ModuleWithSubModuleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChildrens(int id, CancellationToken cancellationToken)
        {
            IEnumerable<ModuleWithSubModuleGetDto> Modules = await _moduleGetChildrensHandler.Handle(id, cancellationToken);
            return Ok(Modules);
        }
    }
}
