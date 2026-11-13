using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Delete.Contracts;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.AccessTree.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries;
using Aban360.Api.Filters;

namespace BeforeServiceConsumersWave20.Aban360.Api.Controllers.V1.UserPool.AccessTree
{
    public class SubModuleController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly ISubModuleCreateHandler _c0_subModuleCreateHandler;
        private readonly IUnitOfWork _c1_uow;
        private readonly ISubModuleDeleteHandler _c1_subModuleDeleteHandler;
        private readonly IUnitOfWork _c2_uow;
        private readonly ISubModuleUpdateHandler _c2_subModuleUpdateHandler;
        private readonly IUnitOfWork _c3_uow;
        private readonly ISubModuleGetAllHandler _c3_subModuleGetAllHandler;
        private readonly ISubModuleGetChildrensHandler _c4_subModuleGetChildrensHandler;
        private readonly IUnitOfWork _c5_uow;
        private readonly ISubModuleGetSingleHandler _c5_subModuleGetSingleHandler;
        public SubModuleController(IUnitOfWork c0_uow, ISubModuleCreateHandler c0_subModuleCreateHandler, IUnitOfWork c1_uow, ISubModuleDeleteHandler c1_subModuleDeleteHandler, IUnitOfWork c2_uow, ISubModuleUpdateHandler c2_subModuleUpdateHandler, IUnitOfWork c3_uow, ISubModuleGetAllHandler c3_subModuleGetAllHandler, ISubModuleGetChildrensHandler c4_subModuleGetChildrensHandler, IUnitOfWork c5_uow, ISubModuleGetSingleHandler c5_subModuleGetSingleHandler)
        {
            _c0_uow = c0_uow;
            _c0_uow.NotNull(nameof(c0_uow));
            _c0_subModuleCreateHandler = c0_subModuleCreateHandler;
            _c0_subModuleCreateHandler.NotNull(nameof(c0_subModuleCreateHandler));
            _c1_uow = c1_uow;
            _c1_uow.NotNull(nameof(c1_uow));
            _c1_subModuleDeleteHandler = c1_subModuleDeleteHandler;
            _c1_subModuleDeleteHandler.NotNull(nameof(c1_subModuleDeleteHandler));
            _c2_uow = c2_uow;
            _c2_uow.NotNull(nameof(c2_uow));
            _c2_subModuleUpdateHandler = c2_subModuleUpdateHandler;
            _c2_subModuleUpdateHandler.NotNull(nameof(c2_subModuleUpdateHandler));
            _c3_uow = c3_uow;
            _c3_uow.NotNull(nameof(c3_uow));
            _c3_subModuleGetAllHandler = c3_subModuleGetAllHandler;
            _c3_subModuleGetAllHandler.NotNull(nameof(c3_subModuleGetAllHandler));
            _c4_subModuleGetChildrensHandler = c4_subModuleGetChildrensHandler;
            _c4_subModuleGetChildrensHandler.NotNull(nameof(_c4_subModuleGetChildrensHandler));
            _c5_uow = c5_uow;
            _c5_uow.NotNull(nameof(c5_uow));
            _c5_subModuleGetSingleHandler = c5_subModuleGetSingleHandler;
            _c5_subModuleGetSingleHandler.NotNull(nameof(c5_subModuleGetSingleHandler));
        }

        [HttpPost]
        [Route("~/v1/sub-module/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SubModuleCreateDto createDto, CancellationToken cancellationToken)
        {
            await _c0_subModuleCreateHandler.Handle(createDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(createDto);
        }

        [HttpDelete, HttpPost]
        [Route("~/v1/sub-module/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SubModuleDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _c1_subModuleDeleteHandler.Handle(deleteDto, cancellationToken);
            await _c1_uow.SaveChangesAsync(cancellationToken);
            return Ok(deleteDto);
        }

        [HttpPatch, HttpPost]
        [Route("~/v1/sub-module/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SubModuleUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _c2_subModuleUpdateHandler.Handle(updateDto, cancellationToken);
            await _c2_uow.SaveChangesAsync(cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/sub-module/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SubModuleGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var subModule = await _c3_subModuleGetAllHandler.Handle(cancellationToken);
            return Ok(subModule);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/sub-module/childrens/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SubModuleWithEndPointGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChildrens(int id, CancellationToken cancellationToken)
        {
            IEnumerable<SubModuleWithEndPointGetDto> SubModules = await _c4_subModuleGetChildrensHandler.Handle(id, cancellationToken);
            return Ok(SubModules);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/sub-model/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SubModuleGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Single(int id, CancellationToken cancellationToken)
        {
            var subModule = await _c5_subModuleGetSingleHandler.Handle(id, cancellationToken);
            return Ok(subModule);
        }
    }
}
