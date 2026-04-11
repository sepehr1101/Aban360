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
    internal sealed class ArchiveRequestHandler : AbstractBaseConnection, IArchiveRequestHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        static int _archiveStatusId = 90003;
        public ArchiveRequestHandler(
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

        public async Task Handle(ArchiveRequestInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            Validation(inputDto.TrackNumber, moshtrakInfo.IsRegistered);

            TrackingInsertDuplicateDto trackingInsertDto = GetTrackingInsertDto(inputDto, userCode);
            string dbName = GetDbName(trackingInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);

                    await trackingCommandService.UpdateIsConsiderdLatest(inputDto.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertDto);

                    transaction.Commit();
                }
            }
        }
        private void Validation(int trackNumber, bool isRegistered)//todo: need Or not?
        {
            if (isRegistered == true)//and not Deleted
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }
        private TrackingInsertDuplicateDto GetTrackingInsertDto(ArchiveRequestInputDto inputDto, int userCode)
        {
            return new TrackingInsertDuplicateDto(inputDto.TrackNumber, inputDto.SendToArchive ? _archiveStatusId : inputDto.StatusId.Value, inputDto.Description, userCode);
        }
    }
}
