using global::Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts;
using global::Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Contracts;
using global::Aban360.ClaimPool.Domain.Constants;
using global::Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using global::Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using global::Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using global::Aban360.Common.Db.Dapper;
using global::Aban360.Common.Exceptions;
using global::Aban360.Common.Extensions;
using global::Aban360.Common.Literals;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BeforeServiceWorkflow10.Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Delete.Implementations{
    internal sealed class MotherDeleteHandler : AbstractBaseConnection, IMotherDeleteHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        public MotherDeleteHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));
        }

        public async Task Handle(int trackNumber, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            string dbName = GetDbName(trackingInfo.ZoneId);
            await MoshtrakValdiation(trackingInfo.ZoneId, trackNumber);

            string stringTrackNumber = trackNumber.ToString().PadLeft(11, '0');

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MotherCommandService motherCommandService = new(connection, transaction);
                    await motherCommandService.Delete(stringTrackNumber, dbName);

                    transaction.Commit();
                }
            }
        }
        private async Task MoshtrakValdiation(int zoneId, int trackNumber)
        {
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(zoneId, null, null, trackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            if (moshtrakInfo.IsRegistered)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidMotherRequest);
            }
        }
    }
}
