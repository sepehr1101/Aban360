using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using DNTPersianUtils.Core;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    public class ConnectDisconnectGetAllHandler : IConnectDisconnectGetAllHandler
    {
        private readonly IConnectDisconnectQueryService _queryService;
        private const int _disconnectTypeId = 0;
        private const int _connectTypeId = 1;
        public ConnectDisconnectGetAllHandler(IConnectDisconnectQueryService queryService)
        {
            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task<ReportOutput<ConnectDisconnectHeaderOutputDto, ConnectDisconnectDataOutputDto>> Handle(int zoneId, CancellationToken cancellationToken)
        {
            string title = ReportLiterals.NoResultConnectDisconnect;
            IEnumerable<ConnectDisconnectDataOutputDto> data = await _queryService.Get(zoneId, true, true);
            ConnectDisconnectHeaderOutputDto header = new()
            {
                ZoneId = data?.FirstOrDefault()?.ZoneId ?? 0,
                ZoneTitle = data?.FirstOrDefault()?.ZoneTitle ?? string.Empty,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                ConnectCount = data?.Where(d => d.TypeId == _connectTypeId)?.Count() ?? 0,
                DisconnectCount = data?.Where(d => d.TypeId == _disconnectTypeId)?.Count() ?? 0,
                Count = data?.Count() ?? 0,
                RecordCount = data?.Count() ?? 0,
                Title = title
            };

            return new ReportOutput<ConnectDisconnectHeaderOutputDto, ConnectDisconnectDataOutputDto>(title, header, data);
        }
    }
}
