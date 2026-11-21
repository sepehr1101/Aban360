using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.ClaimPool.Application.Features.People.Services;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/customer-comment")]
    public class CustomerCommentController : BaseController
    {
        private readonly IClientCommentService _clientCommentService;
        public CustomerCommentController(IClientCommentService clientCommentService)
        {
            _clientCommentService = clientCommentService;
            _clientCommentService.NotNull(nameof(clientCommentService));
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerCommentInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddNewComment(CustomerCommentInputDto inputDto, CancellationToken cancellationToken)
        {
            await _clientCommentService.InsertAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<CustomerCommentGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillid(SearchInput inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<CustomerCommentGetDto> result = await _clientCommentService.GetByBillIdAsync(inputDto.Input, cancellationToken);
            return Ok(result);
        }
    }
}
