using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Implementations
{
    internal sealed class ServiceLinkPaidGetHandler : IServiceLinkPaidGetHandler
    {
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneQueryServcice;
        private readonly IVosolEnQueryService _vosolEnQueryService;
        private string _title = ReportLiterals.ServiceLinkPaid;
        public ServiceLinkPaidGetHandler(
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneQueryService,
            IVosolEnQueryService vosolEnQueryService)
        {
            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneQueryServcice = commonZoneQueryService;
            _commonZoneQueryServcice.NotNull(nameof(commonZoneQueryService));

            _vosolEnQueryService = vosolEnQueryService;
            _vosolEnQueryService.NotNull(nameof(vosolEnQueryService));
        }

        public async Task<ReportOutput<ServiceLinkPaidHeaderOutputDto, ServiceLinkPaidDataOutputDto>> Handle(ServiceLinkPaidInputDto input, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _commonZoneQueryServcice.IsUserInZone(appUser, input.ZoneId);
            IEnumerable<ServiceLinkPaidDataOutputDto> data = await _vosolEnQueryService.Get(input);
            ServiceLinkPaidHeaderOutputDto header = new()
            {
                ZoneId = input.ZoneId,
                ZoneTitle = data?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                Amount = data?.Sum(a => a.Amount) ?? 0,
                RecordCount = data?.Count() ?? 0,
                Title = _title
            };

            return new ReportOutput<ServiceLinkPaidHeaderOutputDto, ServiceLinkPaidDataOutputDto>(_title, header, data);
        }
    }
}
