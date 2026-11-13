using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Commands.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.Common.BaseEntities;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging
{
    public class TagGroupController : BaseController
    {
        private readonly ICreateTagGroupHandler _c0_createHandler;
        private readonly IDeleteTagGroupHandler _c1_deleteHandler;
        private readonly IUpdateTagGroupHandler _c2_updateHandler;
        private readonly IGetTagGroupHandler _c3_getHandler;
        private readonly IGetTagGroupHandler _c4_getHandler;
        public TagGroupController(ICreateTagGroupHandler c0_createHandler, IDeleteTagGroupHandler c1_deleteHandler, IUpdateTagGroupHandler c2_updateHandler, IGetTagGroupHandler c3_getHandler, IGetTagGroupHandler c4_getHandler)
        {
            _c0_createHandler = c0_createHandler;
            _c0_createHandler.NotNull(nameof(c0_createHandler));
            _c1_deleteHandler = c1_deleteHandler;
            _c1_deleteHandler.NotNull(nameof(c1_deleteHandler));
            _c2_updateHandler = c2_updateHandler;
            _c2_updateHandler.NotNull(nameof(c2_updateHandler));
            _c3_getHandler = c3_getHandler;
            _c3_getHandler.NotNull(nameof(c3_getHandler));
            _c4_getHandler = c4_getHandler;
            _c4_getHandler.NotNull(nameof(c4_getHandler));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CreateTagGroupDto>), StatusCodes.Status200OK)]
        [Route("~/v1/tag-group/create")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateTagGroupDto dto)
        {
            var id = await _c0_createHandler.Handle(dto);
            return Ok(dto);
        }

        [HttpDelete, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        [Route("~/v1/tag-group/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _c1_deleteHandler.Handle(id);
            if (!result)
                return NotFound();
            return Ok(new { Id = id });
        }

        [HttpPatch, HttpPost]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UpdateTagGroupDto>), StatusCodes.Status200OK)]
        [Route("~/v1/tag-group/update")]
        public async Task<IActionResult> Update([FromBody] UpdateTagGroupDto dto)
        {
            var result = await _c2_updateHandler.Handle(dto);
            if (!result)
                return NotFound();
            return Ok(dto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TagGroupDto>), StatusCodes.Status200OK)]
        [Route("~/v1/tag-group/by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            TagGroupDto group = await _c3_getHandler.Handle(id);
            if (group == null)
                return NotFound();
            return Ok(group);
        }

        [HttpGet]
        [Route("~/v1/tag-group/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<TagGroupDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<TagGroupDto> groups = await _c4_getHandler.HandleAll();
            return Ok(groups);
        }

        [HttpGet]
        [Route("~/v1/tag-group/dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDictionary()
        {
            IEnumerable<TagGroupDto> groups = await _c4_getHandler.HandleAll();
            IEnumerable<NumericDictionary> dictionary = groups.OrderBy(g => g.MainTagGroupTitle).OrderBy(g => g.Title).Select(g => new NumericDictionary(g.Id, $"{g.MainTagGroupTitle}-{g.Title}"));
            return Ok(dictionary);
        }
    }
}
