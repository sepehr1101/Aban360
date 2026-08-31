using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Tagging.Handlers.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Tagging
{
    [Route("v1/tag-group")]
    public class TagGroupGetController : BaseController
    {
        private readonly IGetTagGroupHandler _getHandler;

        public TagGroupGetController(IGetTagGroupHandler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<TagGroupDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<TagGroupDto> groups = await _getHandler.HandleAll();
            return Ok(groups);
        }

        [HttpGet]
        [Route("dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDictionary()
        {
            IEnumerable<TagGroupDto> groups = await _getHandler.HandleAll();
            IEnumerable<NumericDictionary> dictionary = groups
                 .OrderBy(g => g.MainTagGroupTitle)
                 .OrderBy(g=>g.Title)
                 .Select(g => new NumericDictionary(g.Id, $"{g.MainTagGroupTitle}-{g.Title}"));
            return Ok(dictionary);
        }
    }
}
