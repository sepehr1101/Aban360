using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.ReportPool.Application.Features.Tagging.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.MainTagGroupging.Commands
{
    [Route("v1/main-tag-group")]
    public class MainTagGroupController : BaseController
    {
        private readonly IMainTagGroupCreateHandler _createHandler;
        private readonly IMainTagGroupUpdateHandler _updateHandler;
        private readonly IMainTagGroupDeleteHandler _deleteHandler;
        private readonly IMainTagGroupGetAllHandler _getAllHandler;
        private readonly IMainTagGroupGetHandler _getByIdHandler;
        public MainTagGroupController(
            IMainTagGroupCreateHandler createHandler,
            IMainTagGroupUpdateHandler updateHandler,
            IMainTagGroupDeleteHandler deleteHandler,
            IMainTagGroupGetAllHandler getAllHandler,
            IMainTagGroupGetHandler getByIdHandler)
        {
            _createHandler = createHandler;
            _createHandler.NotNull(nameof(createHandler));

            _updateHandler = updateHandler;
            _updateHandler.NotNull(nameof(updateHandler));

            _deleteHandler = deleteHandler;
            _deleteHandler.NotNull(nameof(deleteHandler));

            _getAllHandler = getAllHandler;
            _getAllHandler.NotNull(nameof(getAllHandler));

            _getByIdHandler = getByIdHandler;
            _getByIdHandler.NotNull(nameof(getByIdHandler));

        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MainTagGroupInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MainTagGroupInsertInputDto dto)
        {
            await _createHandler.Handle(dto);
            return Ok(dto);
        }

        [Route("update")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MainTagGroupUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] MainTagGroupUpdateDto input, CancellationToken cancellationToken)
        {
            await _updateHandler.Handle(input);
            return Ok(input);
        }

        [Route("delete/{id}")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _deleteHandler.Handle(id);
            return Ok(id);
        }

        [Route("get")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MainTagGroupGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<MainTagGroupGetDto> result = await _getAllHandler.Handle();
            return Ok(result);
        }

        [Route("get/{id}")]
        [HttpGet, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MainTagGroupGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            MainTagGroupGetDto result = await _getByIdHandler.Handle(id);
            return Ok(result);
        }
    }
}
