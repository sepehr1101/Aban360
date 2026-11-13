using Aban360.Api.Filters;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule
{
    [Route("v1/table1")]
    public class Table1Controller : BaseController
    {
        private readonly ITable1CreateHandler _table1CreateHandler;
        private readonly ITable1UpdateHandler _table1UpdateHandler;
        private readonly ITable1GetAllHandler _table1GetAllHandler;
        private readonly ITable1GetHandler _table1GetHandler;

        public Table1Controller(
            ITable1CreateHandler table1CreateHandler,
            ITable1UpdateHandler table1UpdateHandler,
            ITable1GetAllHandler table1GetAllHandler,
            ITable1GetHandler table1GetHandler)
        {
            _table1CreateHandler = table1CreateHandler;
            _table1CreateHandler.NotNull(nameof(table1CreateHandler));
            _table1UpdateHandler = table1UpdateHandler;
            _table1UpdateHandler.NotNull(nameof(table1UpdateHandler));
            _table1GetAllHandler = table1GetAllHandler;
            _table1GetAllHandler.NotNull(nameof(table1GetAllHandler));
            _table1GetHandler = table1GetHandler;
            _table1GetHandler.NotNull(nameof(table1GetHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Table1CreateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("Table1Create")]
        public async Task<IActionResult> Create(Table1CreateDto createDto, CancellationToken cancellationToken)
        {
            await _table1CreateHandler.Handle(createDto, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Table1UpdateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("Table1Update")]
        public async Task<IActionResult> Update(Table1UpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            await _table1UpdateHandler.Handle(UpdateDto, cancellationToken);
            return Ok(UpdateDto);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<Table1GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
        {
            IEnumerable<Table1GetDto> result = await _table1GetAllHandler.Handle( cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Table1GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            Table1GetDto result = await _table1GetHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
