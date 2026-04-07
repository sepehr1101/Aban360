using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.People.Commands
{
    [Route("v1/customer-comment")]
    public class CustomerCommentController : BaseController
    {
        private readonly IClientCommentGetByBillIdHandler _clientCommentGetHandler;
        private readonly IClientCommentInsertHandler _clientCommentInsertHandler;
        public CustomerCommentController(
            IClientCommentGetByBillIdHandler clientCommentGetHandler,
            IClientCommentInsertHandler clientCommentInsertHandler)
        {
            _clientCommentGetHandler = clientCommentGetHandler;
            _clientCommentGetHandler.NotNull(nameof(clientCommentGetHandler));

            _clientCommentInsertHandler = clientCommentInsertHandler;
            _clientCommentInsertHandler.NotNull(nameof(clientCommentInsertHandler));
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<CustomerCommentInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddNewComment(CustomerCommentInputDto inputDto, CancellationToken cancellationToken)
        {
            await _clientCommentInsertHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<CustomerCommentGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillid(SearchInput inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<CustomerCommentGetDto> result= await _clientCommentGetHandler.Handle(inputDto.Input,  cancellationToken);
            return Ok(result);
        }
    }
}
