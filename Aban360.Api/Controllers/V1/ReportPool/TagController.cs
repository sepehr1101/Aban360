using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Mvc;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Microsoft.AspNetCore.Authorization;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.ReportPool
{
    public class TagController : BaseController
    {
        private readonly ISpecialConditionTagsInfoGetHandler _c0_specialConditionTagsInfoHandle;
        private readonly ICreateTagHandler _c1_createHandler;
        private readonly IDeleteTagHandler _c2_deleteHandler;
        private readonly IUpdateTagHandler _c3_updateHandler;
        private readonly IGetTagHandler _c4_getHandler;
        private readonly IGetTagHandler _c5_getHandler;
        public TagController(ISpecialConditionTagsInfoGetHandler c0_specialConditionTagsInfoHandle, ICreateTagHandler c1_createHandler, IDeleteTagHandler c2_deleteHandler, IUpdateTagHandler c3_updateHandler, IGetTagHandler c4_getHandler, IGetTagHandler c5_getHandler)
        {
            _c0_specialConditionTagsInfoHandle = c0_specialConditionTagsInfoHandle;
            _c0_specialConditionTagsInfoHandle.NotNull(nameof(c0_specialConditionTagsInfoHandle));
            _c1_createHandler = c1_createHandler;
            _c1_createHandler.NotNull(nameof(c1_createHandler));
            _c2_deleteHandler = c2_deleteHandler;
            _c2_deleteHandler.NotNull(nameof(c2_deleteHandler));
            _c3_updateHandler = c3_updateHandler;
            _c3_updateHandler.NotNull(nameof(c3_updateHandler));
            _c4_getHandler = c4_getHandler;
            _c4_getHandler.NotNull(nameof(c4_getHandler));
            _c5_getHandler = c5_getHandler;
            _c5_getHandler.NotNull(nameof(c5_getHandler));
        }

        [HttpPost]
        [Route("~/v1/tag/info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SpecialConditionTagsInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> info(SearchInput searchInput, CancellationToken cancellationToken)
        {
            SpecialConditionTagsInfoDto summary = await _c0_specialConditionTagsInfoHandle.Handle(searchInput.Input, cancellationToken);
            return Ok(summary);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreateTagDto>), StatusCodes.Status200OK)]
        [Route("~/v1/tag/create")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateTagDto dto)
        {
            var id = await _c1_createHandler.Handle(dto);
            return Ok(dto);
        }

        [HttpPost, HttpDelete]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        [Route("~/v1/tag/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _c2_deleteHandler.Handle(id);
            if (!result)
                return NotFound();
            return Ok(new { Id = id });
        }

        [HttpPut, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UpdateTagDto>), StatusCodes.Status200OK)]
        [Route("~/v1/tag/update")]
        public async Task<IActionResult> Update([FromBody] UpdateTagDto dto)
        {
            var result = await _c3_updateHandler.Handle(dto);
            if (!result)
                return NotFound();
            return Ok(dto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TagDto>), StatusCodes.Status200OK)]
        [Route("~/v1/tag/get/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            TagDto tag = await _c4_getHandler.Handle(id);
            if (tag == null)
                return NotFound();
            return Ok(tag);
        }

        [HttpGet]
        [Route("~/v1/tag/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<TagDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _c5_getHandler.HandleAll();
            return Ok(tags);
        }
    }
}
