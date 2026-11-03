using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class AssessmentResultCompletionHandler : AbstractBaseConnection, IAssessmentResultCompletionHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IExaminationQueryService _examinationQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private static int _registeredStatusId = (int)TrackingStatusEnum.Registered;
        private static int _amountConfirmedStatusId = (int)TrackingStatusEnum.AmountConfirmed;
        private static int _deletedStatusId = (int)TrackingStatusEnum.Deleted;
        private static int[] _invalidStatusIds = { _registeredStatusId, _amountConfirmedStatusId, _deletedStatusId };
        public AssessmentResultCompletionHandler(
            IHttpContextAccessor contextAccessor,
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IExaminationQueryService examinationQueryService,
            ICommonZoneService commonZoneService,
            IConfiguration configuration)
                : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _examinationQueryService = examinationQueryService;
            _examinationQueryService.NotNull(nameof(examinationQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<AssessmentResultInputDto> Handle(IAppUser appUser, CancellationToken cancellationToken)
        {
            string? requestBody = await new StreamReader(_contextAccessor.HttpContext.Request.Body).ReadToEndAsync();
            AssessmentResultInputDto inputDto = JsonOperation.Unmarshal<AssessmentResultInputDto>(requestBody);
            await Validate(inputDto.TrackNumber);

            AssessmentDataOutputDto latestAssessmentInfo = await _examinationQueryService.GetLatestByTrackNumber(inputDto.TrackNumber);
            AssessmentCompletionUpdateDto assessmentUpdateDto = new(latestAssessmentInfo.Id, requestBody);

            await ExecSql(assessmentUpdateDto);
            return inputDto;
        }
        private async Task ExecSql(AssessmentCompletionUpdateDto assessmentUpdateDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    ExaminationCommandService _assessmentCommandService = new(connection, transaction);
                    await _assessmentCommandService.Update(assessmentUpdateDto);

                    transaction.Commit();
                }
            }
        }
        private async Task Validate(int trackNumber)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(trackNumber, true);
            if (_invalidStatusIds.Contains(latestTrackingInfo.StatusId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }

            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(latestTrackingInfo.ZoneId, null, null, trackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            if (moshtrakInfo.IsRegistered)
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundAnyOpenTrack);
            }
        }
    }
}