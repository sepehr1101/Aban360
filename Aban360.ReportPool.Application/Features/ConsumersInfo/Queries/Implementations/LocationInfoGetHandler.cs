using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class LocationInfoGetHandler : ILocationInfoGetHandler
    {
        private readonly ILocationInfoService _branchSpecificationSummaryInfoService;
        public LocationInfoGetHandler(ILocationInfoService branchSpecificationSummaryInfoService)
        {
            _branchSpecificationSummaryInfoService = branchSpecificationSummaryInfoService;
            _branchSpecificationSummaryInfoService.NotNull(nameof(branchSpecificationSummaryInfoService));
        }

        public async Task<LocationInfoDto> Handle(string billId, CancellationToken cancellationToken)
        {
            var branchSpecificationSummaryInfo = await _branchSpecificationSummaryInfoService.GetInfo(billId);
            return branchSpecificationSummaryInfo;
        }
    }
}
