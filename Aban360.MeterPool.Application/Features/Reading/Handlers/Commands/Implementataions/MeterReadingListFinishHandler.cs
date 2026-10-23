using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Contracts;
using Aban360.MeterPool.Domain.Constants;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Commands;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Commands.Implementataions
{
    internal sealed class MeterReadingListFinishHandler : AbstractBaseConnection, IMeterReadingListFinishHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IDetailQueryService _detailQueryService;
        public MeterReadingListFinishHandler(
            ITrackingQueryService trackingQueryService,
            IDetailQueryService detailQueryService,
            IConfiguration configuration)
                : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _detailQueryService = detailQueryService;
            _detailQueryService.NotNull(nameof(detailQueryService));
        }

        public async Task Handle(long trackNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingGetDto trackingsInfo = await _trackingQueryService.Get(trackNumber);
            if (trackingsInfo.FlowStepId != (int)ReadingFlowStepEnum.StartReading)
            {
                throw new ReadingException(string.Format(ExceptionLiterals.InvalidTrackingStepFlowToFinish, trackingsInfo.FlowStepTitle));
            }

            IEnumerable<DetailGetDto> uncompleteDetailList = await _detailQueryService.GetUnComplete(trackNumber);
            if (uncompleteDetailList.Any())
            {
                throw new ReadingException(string.Format(ExceptionLiterals.InvalidFinishTrackingByUncompleteDetail, uncompleteDetailList?.Count() ?? 0));
            }

            TrackingDuplicateInsertDto newTrackingDto = new(new List<Guid>() { trackingsInfo.Id }.ToList(), (int)ReadingFlowStepEnum.Upload, appUser.UserId);
            await ExecSql(newTrackingDto);
        }
        private async Task ExecSql(TrackingDuplicateInsertDto newTrackingDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    await trackingCommandService.InsertDuplicate(newTrackingDto);

                    transaction.Commit();
                }
            }
        }
    }
}
