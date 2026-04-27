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
    internal sealed class CalculationRequestConfirmHandler : AbstractBaseConnection, ICalculationRequestConfirmHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IGhestQueryService _ghestQueryService;
        static int _calculationConfirmedStatus = 60;
        static int _requestOrigin = 12;
        static int _setAssessmentResult = 110;
        static int _reCalculateRequired = 65;
        static int[] _allowedStatus = { _reCalculateRequired, _setAssessmentResult };

        public CalculationRequestConfirmHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IGhestQueryService ghestQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _ghestQueryService = ghestQueryService;
            _ghestQueryService.NotNull(nameof(ghestQueryService));
        }

        public async Task Handle(SetCalculationRequestInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(latestTrackingInfo.ZoneId, null, null, latestTrackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            IEnumerable<InstallmentRequestDataOutputDto> installments = await _ghestQueryService.Get(latestTrackingInfo.StringTrackNumber, latestTrackingInfo.ZoneId);
            Validation(installments, latestTrackingInfo, moshtrakInfo.CustomerNumber);

            TrackingInsertDuplicateDto trackingInsertDuplicateDto = new(latestTrackingInfo.TrackNumber, _calculationConfirmedStatus, inputDto.Description, userCode, _requestOrigin, true, false);
            //TrackingInsertDto trackingInsertDto = GetTrackingCreateDto(inputDto, latestTrackingInfo, userCode);
            await ExecuteSqlCommand(trackingInsertDuplicateDto);
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
        private void Validation(IEnumerable<InstallmentRequestDataOutputDto> installments, TrackingOutputDto latestTrackingInfo, int customerNumber)
        {
            if (!installments.Any())
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidCalculationConfirmed);
            }
            if (customerNumber == 0 || string.IsNullOrWhiteSpace(latestTrackingInfo.BillId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.SetBillId);
            }
            if (!_allowedStatus.Contains(latestTrackingInfo.StatusId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }
    }
}
