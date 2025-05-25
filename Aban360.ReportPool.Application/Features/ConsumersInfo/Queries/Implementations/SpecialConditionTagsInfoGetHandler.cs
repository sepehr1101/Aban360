using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;

namespace Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations
{
    internal class SpecialConditionTagsInfoGetHandler : ISpecialConditionTagsInfoGetHandler
    {
        private readonly ISpecialConditionTagsInfoService _specialConditionTagsInfoService;
        public SpecialConditionTagsInfoGetHandler(ISpecialConditionTagsInfoService specialConditionTagsInfoService)
        {
            _specialConditionTagsInfoService = specialConditionTagsInfoService;
            _specialConditionTagsInfoService.NotNull(nameof(specialConditionTagsInfoService));
        }

        public async Task<SpecialConditionTagsInfoDto> Handle(string billId, CancellationToken cancellationToken)
        {
            var specialConditionTagsInfo = await _specialConditionTagsInfoService.GetInfo(billId);
            return specialConditionTagsInfo;
        }
    }
}
