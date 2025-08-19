using Aban360.BlobPool.Application.Features.DmsServices.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Queries;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.BlobController.DmsServices.Queries
{
    [Route("v1/dms-user")]
    public class UserGetCotroller : BaseController
    {
        private readonly IUserGetHandler _UserGetHandler;
        public UserGetCotroller(IUserGetHandler UserGetHandler)
        {
            _UserGetHandler = UserGetHandler;
            _UserGetHandler.NotNull(nameof(UserGetHandler));
        }

        [HttpPost]
        [Route("get")]
        public async Task<IActionResult> Get(SearchUserInputDto input)
        {
            await _UserGetHandler.Handle(input);
            return Ok();
        }
    }
}
