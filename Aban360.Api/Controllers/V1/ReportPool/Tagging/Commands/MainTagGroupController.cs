using Aban360.ReportPool.Application.Features.Tagging.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.MainTagGroupging.Commands
{
    [Route("v1/main-tag-group")]
    public class MainTagGroupController : BaseController
    {
        private readonly IMainTagGroupService _service;
        public MainTagGroupController(IMainTagGroupService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MainTagGroupInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MainTagGroupInsertInputDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok(dto);
        }

        [Route("update")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MainTagGroupUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] MainTagGroupUpdateDto input, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(input);
            return Ok(input);
        }

        [Route("delete/{id}")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(id);
            return Ok(id);
        }

        [Route("get")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<MainTagGroupGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<MainTagGroupGetDto> result = await _service.GetAllAsync();
            return Ok(result);
        }

        [Route("get/{id}")]
        [HttpGet, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MainTagGroupGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            MainTagGroupGetDto result = await _service.GetSingleAsync(id);
            return Ok(result);
        }
    }
}
