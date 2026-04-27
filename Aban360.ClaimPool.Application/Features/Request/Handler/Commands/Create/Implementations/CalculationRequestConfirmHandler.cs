using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
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
    internal sealed class CalculationRequestConfirmHandler : AbstractBaseConnection, ICalculationRequestConfirmHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IGhestQueryService _ghestQueryService;
        static int _calculationConfirmedStatus = 60;
        static int _requestOrigin = 12;

        public CalculationRequestConfirmHandler(
            ITrackingQueryService trackingQueryService,
            IGhestQueryService ghestQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _ghestQueryService = ghestQueryService;
            _ghestQueryService.NotNull(nameof(ghestQueryService));
        }

        public async Task Handle(SetCalculationRequestInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            IEnumerable<InstallmentRequestDataOutputDto> installments = await _ghestQueryService.Get(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);
            if (!installments.Any())
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidCalculationConfirmed);
            }

            TrackingInsertDto trackingInsertDto = GetTrackingCreateDto(inputDto, trackingInfo, userCode);
            await ExecuteSqlCommand(trackingInsertDto);
        }
        private TrackingInsertDto GetTrackingCreateDto(SetCalculationRequestInputDto inputDto, TrackingOutputDto latestTrackingInfo, int userCode)
        {
            return new TrackingInsertDto()
            {
                TrackNumber = latestTrackingInfo.TrackNumber,
                ZoneId = latestTrackingInfo.ZoneId,
                BillId = latestTrackingInfo.BillId,
                ServiceGroupId = latestTrackingInfo.ServiceGroupId,
                StatusId = _calculationConfirmedStatus,
                InsertByUserId = userCode,
                Description = inputDto.Description,
                NotificationMobile = latestTrackingInfo.NotificationMobile,
                NeighbourBillId = latestTrackingInfo.NeighbourBillId,
                RequestOrigin = _requestOrigin,
            };
        }
        private async Task ExecuteSqlCommand(TrackingInsertDto trackingInsertDto)
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

                    await trackingCommandService.UpdateIsConsiderdLatest(trackingInsertDto.TrackNumber, true);
                    await trackingCommandService.Insert(trackingInsertDto);
                    
                    transaction.Commit();
                }
            }
        }
    }
}
