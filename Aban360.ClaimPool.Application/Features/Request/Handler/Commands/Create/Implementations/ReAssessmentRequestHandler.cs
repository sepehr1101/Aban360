using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class ReAssessmentRequestHandler : AbstractBaseConnection, IReAssessmentRequestHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IAssessmentQueryService _assessmentQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        static int _reAssessmentStatusId = 15;
        static int _deletedSatatus = 90000;
        static int _requestOrigin = 12;
        public ReAssessmentRequestHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IAssessmentQueryService assessmentQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _assessmentQueryService = assessmentQueryService;
            _assessmentQueryService.NotNull(nameof(assessmentQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        }

        public async Task Handle(TrackNumberWithDescriptionInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(latestTrackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            Validation(inputDto.TrackNumber, latestTrackingInfo.StatusId, moshtrakInfo.IsRegistered);

            TrackingInsertDuplicateDto trackingInsertDto = GetTrackingInsertDto(inputDto, userCode);
            string dbName = GetDbName(latestTrackingInfo.ZoneId);

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
        private void Validation(int trackNumber, int statusId, bool isRegistered)//todo: need Or not?
        {
            if (isRegistered == true || statusId == _deletedSatatus)//and not Deleted
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
        }
        private TrackingInsertDuplicateDto GetTrackingInsertDto(TrackNumberWithDescriptionInputDto inputDto, int userCode)
        {
            return new TrackingInsertDuplicateDto(inputDto.TrackNumber, _reAssessmentStatusId, inputDto.Description, userCode, _requestOrigin);
        }
    }
}
