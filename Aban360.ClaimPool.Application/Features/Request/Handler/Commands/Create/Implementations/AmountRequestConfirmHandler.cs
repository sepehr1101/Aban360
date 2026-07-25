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
    internal sealed class AmountRequestConfirmHandler : AbstractBaseConnection, IAmountRequestConfirmHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IGhestQueryService _ghestQueryService;
        static int _amountIsConfirmedStatus = 75;
        static int _calculationConfirmedStatus = 60;
        static int _requestOrigin = 12;

        public AmountRequestConfirmHandler(
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
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            IEnumerable<InstallmentRequestDataOutputDto> installments = await _ghestQueryService.Get(latestTrackingInfo.StringTrackNumber, latestTrackingInfo.ZoneId);
            Validate(installments, latestTrackingInfo.StatusId);

            TrackingInsertDuplicateDto trackingInsertDuplicateDto = new(latestTrackingInfo.TrackNumber, _amountIsConfirmedStatus, inputDto.Description, userCode, _requestOrigin, true, false);
            await ExecuteSqlCommand(trackingInsertDuplicateDto);
        }
        private void Validate(IEnumerable<InstallmentRequestDataOutputDto> installments, int previousStatusId)
        {
            if (!installments.Any())
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidEmpyInstallment);
            }
            if (previousStatusId != _calculationConfirmedStatus)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }
        private async Task ExecuteSqlCommand(TrackingInsertDuplicateDto trackingInsertDuplicateDto)
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
                    await trackingCommandService.UpdateIsConsiderdLatest(trackingInsertDuplicateDto.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertDuplicateDto);

                    transaction.Commit();
                }
            }
        }
    }
}
