namespace Aban360.Api.Controllers.V1.ReportPool.Dashboard
{    
    using global::Aban360.Common.Categories.ApiResponse;
    using global::Aban360.Common.Extensions;
    using global::Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
    using global::Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations;
    using global::Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
    using global::Aban360.ReportPool.Domain.Features.Dashboard.Entities;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    namespace Aban360.Api.Controllers.V1.SystemPool.SkeletonManager
    {
        [Route("v1/skeleton")]
        public class SkeletonManagerController : BaseController
        {
            private readonly IGetSkeletonDefinitionHandler _getDefinitionHandler;
            private readonly IGetSkeletonByIdHandler _getByIdHandler;
            private readonly ICreateSkeletonHandler _createHandler;
            private readonly IUpdateSkeletonHandler _updateHandler;
            private readonly IDeleteSkeletonHandler _deleteHandler;
            private readonly IGetSekletonByRoleHandler _getByRoleHanlder;

            public SkeletonManagerController(
                IGetSkeletonDefinitionHandler getDifinitionHandler,
                IGetSkeletonByIdHandler getByIdHandler,
                ICreateSkeletonHandler createHandler,
                IUpdateSkeletonHandler updateHandler,
                IDeleteSkeletonHandler deleteHandler,
                IGetSekletonByRoleHandler getByRoleHandler)
            {
                _getDefinitionHandler = getDifinitionHandler;
                _getDefinitionHandler.NotNull(nameof(_getDefinitionHandler));

                _getByIdHandler = getByIdHandler;
                _getByIdHandler.NotNull(nameof(_getByIdHandler));

                _createHandler = createHandler;
                _createHandler.NotNull(nameof(_createHandler));

                _updateHandler = updateHandler;
                _updateHandler.NotNull(nameof(_updateHandler));

                _deleteHandler = deleteHandler;
                _deleteHandler.NotNull(nameof(_deleteHandler));

                _getByRoleHanlder = getByRoleHandler;
                _getByRoleHanlder.NotNull(nameof(_getByRoleHanlder));
            }

            [HttpGet, HttpPost]
            [Route("all")]
            [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SkeletonDefinitionDto>>), StatusCodes.Status200OK)]
            public async Task<IActionResult> GetAll(CancellationToken cancellationToken) 
            {
                IEnumerable<SkeletonDefinitionDto> definitionDtos = await _getDefinitionHandler.Handle(cancellationToken);
                return Ok(definitionDtos);
            }

            [HttpGet, HttpPost]
            [Route("{id}")]
            [ProducesResponseType(typeof(ApiResponseEnvelope<Skeleton?>), StatusCodes.Status200OK)]
            public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
            {
                Skeleton? skeleton = await _getByIdHandler.Handle(id, cancellationToken);
                return Ok(skeleton);
            }

            [HttpGet, HttpPost]
            [Route("by-role/{role}")]
            [ProducesResponseType(typeof(ApiResponseEnvelope<Skeleton?>), StatusCodes.Status200OK)]
            public async Task<IActionResult> GetByRole(string role, CancellationToken cancellationToken)
            {
                Skeleton? skeleton = await _getByRoleHanlder.Handle(role, cancellationToken);
                return Ok(skeleton);
            }


            [HttpPost]
            [Route("create")]
            [ProducesResponseType(typeof(ApiResponseEnvelope<SkeletonDto>), StatusCodes.Status200OK)]
            public async Task<IActionResult> Create([FromBody] SkeletonDto dto, CancellationToken cancellationToken)
            {
                var id = await _createHandler.Handle(dto, CurrentUser, cancellationToken);
                return Ok(dto);
            }

            [HttpPut, HttpPost]
            [Route("update")]
            [ProducesResponseType(typeof(ApiResponseEnvelope<SkeletonDto>), StatusCodes.Status200OK)]
            public async Task<IActionResult> Update([FromBody] SkeletonDto dto, CancellationToken cancellationToken)
            {               
                bool success = await _updateHandler.Handle(dto, CurrentUser, cancellationToken);
                return Ok(dto);
            }

            [HttpDelete, HttpPost]
            [Route("delete/{id}")]
            [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
            public async Task<IActionResult> SoftDelete(int id, CancellationToken cancellationToken)
            {
                var success = await _deleteHandler.Handle(id, CurrentUser, cancellationToken);
                return Ok(id);
            }
        }
    }
}
