using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class CloseRequestHandler : AbstractBaseConnection, ICloseRequestHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        public CloseRequestHandler(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task<RequestCloseOuputDto> Handle(int tracknumber, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetFirstStep(tracknumber);
            await Validation(trackingInfo.ZoneId, tracknumber);
            string dbName = GetDbName(trackingInfo.ZoneId);
            MoshtrakSabtUpdateDto moshtrakSabtUpdate = new(tracknumber, true);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MoshtrakCommandService _moshtrackCommandService = new(connection, transaction);
                    await _moshtrackCommandService.UpdateSabt(moshtrakSabtUpdate, dbName);
                    transaction.Commit();
                }
            }

            return new RequestCloseOuputDto(trackingInfo.ZoneId, trackingInfo.ZoneTitle, tracknumber);
        }
        private async Task Validation(int zoneId, int trackNumber)
        {
            MoshtrakGetDto moshtrakSearch = new(zoneId, null, null, trackNumber);
            IEnumerable<MoshtrakOutputDto> moshtrakListInfo = await _moshtrakQueryService.Get(moshtrakSearch, MoshtrakSearchTypeEnum.ByTrackNumber);
            if (!moshtrakListInfo.Where(m => m.IsRegistered == false).Any())
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFountOpenRequest);
            }
        }
    }
}
