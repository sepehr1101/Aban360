using Aban360.Common.Categories.ApiResponse;
using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Dashboard
{

    [Route("v1/tile-script")]
    public class TileScriptController : BaseController
    {
        private readonly ICreateTileScriptHandler _createHandler;
        private readonly IGetTileScriptByIdHandler _getByIdHandler;
        private readonly IGetAllTileScriptsHandler _getAllHandler;
        private readonly IUpdateTileScriptHandler _updateHandler;
        private readonly IDeleteTileScriptHandler _deleteHandler;
        private readonly IGetReportByTileScriptContentHandler _reportHandler;
        public TileScriptController(
            ICreateTileScriptHandler createHandler,
            IGetTileScriptByIdHandler getByIdHandler,
            IGetAllTileScriptsHandler getAllHandler,
            IUpdateTileScriptHandler updateHandler,
            IDeleteTileScriptHandler deleteHandler,
            IGetReportByTileScriptContentHandler reportHandler)
        {
            _createHandler = createHandler;
            _getByIdHandler = getByIdHandler;
            _getAllHandler = getAllHandler;
            _updateHandler = updateHandler;
            _deleteHandler = deleteHandler;
            _reportHandler = reportHandler;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TileScriptDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TileScriptDto dto, CancellationToken cancellationToken)
        {
            await _createHandler.Handle(dto, CurrentUser, cancellationToken);
            return Ok(dto);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<TileScript>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<TileScript> result = await _getAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TileScript?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            TileScript? result = await _getByIdHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("{id}/report")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<TileScriptReportDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> report(int id, CancellationToken cancellationToken)
        {
            IEnumerable<TileScriptReportDto> result = await _reportHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TileScriptDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TileScriptDto dto, CancellationToken cancellationToken)
        {
            await _updateHandler.Handle(dto, cancellationToken);
            return Ok(dto);
        }

        [HttpDelete, HttpPost]
        [Route("delete/{id:int}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _deleteHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }
    }
}
