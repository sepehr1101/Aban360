using Aban360.Api.Filters;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Application.Features.Rules.Services;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule
{
    [Route("v1/table1")]
    public class Table1Controller : BaseController
    {
        private readonly ITable1Service _table1Service;
        public Table1Controller(ITable1Service table1Service)
        {
            _table1Service = table1Service;
            _table1Service.NotNull(nameof(table1Service));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Table1CreateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("Table1Create")]
        public async Task<IActionResult> Create(Table1CreateDto createDto, CancellationToken cancellationToken)
        {
            await _table1Service.CreateAsync(createDto, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Table1UpdateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("Table1Update")]
        public async Task<IActionResult> Update(Table1UpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            await _table1Service.UpdateAsync(UpdateDto, cancellationToken);
            return Ok(UpdateDto);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<Table1GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<Table1GetDto> result = await _table1Service.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Table1GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            Table1GetDto result = await _table1Service.GetAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
