using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal sealed class SubscriptionSummaryInfoGetHandler : ISubscriptionSummaryInfoGetHandler
    {
        private readonly IConsumerSummaryQueryService _consumerSummeryQueryService;
        public SubscriptionSummaryInfoGetHandler(IConsumerSummaryQueryService consumerSummaryQueryService)
        {
            _consumerSummeryQueryService = consumerSummaryQueryService;
            _consumerSummeryQueryService.NotNull(nameof(_consumerSummeryQueryService));
        }

        public async Task<ConsumerSummaryDto> Handle(string billId, CancellationToken cancellationToken)
        {
            var consumerSummaryInfo = await _consumerSummeryQueryService.GetInfo(billId);
            consumerSummaryInfo.TotalUnitWater = consumerSummaryInfo.UnitDomesticWater + consumerSummaryInfo.UnitCommercialWater + consumerSummaryInfo.UnitOtherWater;
            consumerSummaryInfo.TotalUnitSewage = consumerSummaryInfo.UnitDomesticSewage + consumerSummaryInfo.UnitCommercialSewage + consumerSummaryInfo.UnitOtherSewage;

            return consumerSummaryInfo;
        }
    }
}
