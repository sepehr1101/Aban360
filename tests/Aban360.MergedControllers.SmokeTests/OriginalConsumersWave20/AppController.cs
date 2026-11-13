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
    [Route("v1/app")]
    public class AppController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IAppCreateHandler _appCreateHandler;
        private readonly IAppUpdateHandler _appUpdateHandler;
        private readonly IAppDeleteHandler _appDeleteHandler;
        private readonly IAppGetSingleHandler _appGetSingleHandler;
        private readonly IAppGetAllHandler _appGetAllHandler;
        private readonly IAppGetChildrensHandler _appGetChildrensHandler;

        public AppController(
            IUnitOfWork uow,
            IAppCreateHandler appCreateHandler,
            IAppUpdateHandler appUpdateHandler,
            IAppDeleteHandler appDeleteHandler,
            IAppGetSingleHandler appGetSingleHandler,
            IAppGetAllHandler appGetAllHandler,
            IAppGetChildrensHandler appGetChildrensHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _appCreateHandler = appCreateHandler;
            _appCreateHandler.NotNull(nameof(_appCreateHandler));

            _appUpdateHandler = appUpdateHandler;
            _appUpdateHandler.NotNull(nameof(_appUpdateHandler));

            _appDeleteHandler = appDeleteHandler;
            _appDeleteHandler.NotNull(nameof(_appDeleteHandler));

            _appGetSingleHandler = appGetSingleHandler;
            _appGetSingleHandler.NotNull(nameof(_appGetSingleHandler));

            _appGetAllHandler = appGetAllHandler;
            _appGetAllHandler.NotNull(nameof(_appGetAllHandler));

            _appGetChildrensHandler = appGetChildrensHandler;
            _appGetChildrensHandler.NotNull(nameof(_appGetChildrensHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AppCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] AppCreateDto createDto, CancellationToken cancellationToken)
        {
            await _appCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPatch, HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AppUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] AppUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _appUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpDelete, HttpPost]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AppDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] AppDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _appDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet,HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AppGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var app = await _appGetSingleHandler.Handle(id, cancellationToken);
            return Ok(app);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<AppGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var apps = await _appGetAllHandler.Handle(cancellationToken);
            return Ok(apps);
        }

        [HttpGet, HttpPost]
        [Route("childrens/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<AppWithModuleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChildrens(int id, CancellationToken cancellationToken)
        {
            IEnumerable<AppWithModuleGetDto> apps = await _appGetChildrensHandler.Handle(id, cancellationToken);
            return Ok(apps);
        }
    }
}
