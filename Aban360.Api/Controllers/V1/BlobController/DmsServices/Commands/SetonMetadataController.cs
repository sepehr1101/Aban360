using Aban360.BlobPool.Application.Features.DmsServices.Handlers.Commands.Create.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Queries;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DmsServices.Commands
{
    [Route("v1/set-on-mete-data")]
    public class SetonMetadataController : BaseController
    {
        private readonly ISetOnMetadataHandler _setOnMetadataHandler;
        public SetonMetadataController(ISetOnMetadataHandler setOnMetadataHandler)
        {
            _setOnMetadataHandler = setOnMetadataHandler;
            _setOnMetadataHandler.NotNull(nameof(setOnMetadataHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> create(SetOnMetadataInputDto input)
        {
            await _setOnMetadataHandler.Handle(input);
            return Ok();
        }
    }
}
