using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Microsoft.Extensions.Configuration;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Implementataions
{
    public interface IMeterReadingListOffLoadHandler
    {
        Task Handle(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
    internal sealed class MeterReadingListOffLoadHandler : AbstractBaseConnection, IMeterReadingListOffLoadHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IDetailQueryService _detailQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IUserQueryService _userQueryService;
        public MeterReadingListOffLoadHandler(
            ITrackingQueryService trackingQueryService,
            IDetailQueryService detailQueryService,
            ICommonZoneService commonZoneService,
            IUserQueryService userQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _detailQueryService = detailQueryService;
            _detailQueryService.NotNull(nameof(detailQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _userQueryService = userQueryService;
            _userQueryService.NotNull(nameof(userQueryService));
        }

        public async Task Handle(IEnumerable<MeterReadingListOffLoadInputDto> inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<DetailGetDto> previousDetailsInfo = await _detailQueryService.Get(inputDto.Select(details => details.Id).ToList());

        }
    }
}
