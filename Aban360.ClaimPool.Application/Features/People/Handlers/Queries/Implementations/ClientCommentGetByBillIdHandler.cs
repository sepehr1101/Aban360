using Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Implementations
{
    internal sealed class ClientCommentGetByBillIdHandler : IClientCommentGetByBillIdHandler
    {
        private readonly IClientCommentQueryService _clientCommentQueryService;
        public ClientCommentGetByBillIdHandler(IClientCommentQueryService userCommentQueryService)
        {
            _clientCommentQueryService = userCommentQueryService;
            _clientCommentQueryService.NotNull(nameof(userCommentQueryService));
        }

        public async Task<IEnumerable<CustomerCommentGetDto>> Handle(string billId, CancellationToken cancellationToken)
        {
            IEnumerable<CustomerCommentGetDto> result= await _clientCommentQueryService.Get(billId);
            result.ForEach(u => u.InsertDateJalali = u.InsertDateTime.ToShortPersianDateString());

            return result;
        }
    }
}
