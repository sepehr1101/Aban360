using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class HandoverHandler : IHandoverHandler
    {
        private readonly IHandoverQueryService _handoverQueryService;
        public HandoverHandler(IHandoverQueryService HandoverQueryService)
        {
            _handoverQueryService = HandoverQueryService;
            _handoverQueryService.NotNull(nameof(HandoverQueryService));
        }

        public async Task<IEnumerable<HandoverQueryDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<HandoverQueryDto> handover = await _handoverQueryService.Get();
            return handover;
        }
    }
}
