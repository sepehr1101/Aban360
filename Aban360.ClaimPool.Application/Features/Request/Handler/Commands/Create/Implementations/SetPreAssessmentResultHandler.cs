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
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class SetPreAssessmentResultHandler : AbstractBaseConnection, ISetPreAssessmentResultHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IExaminationQueryService _assessmentQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IValidator<PreAssessmentResultInputDto> _validator;
        private static int _setAssessmentResultStatus = 110;
        private static int _seenByAssessmentStatus = 150;
        private static int _archiveStats = 90003;
        static int _assessmentSetTime = 10;
        static int _requestOrigin = 12;
        public SetPreAssessmentResultHandler(
            IHttpContextAccessor contextAccessor,
            ITrackingQueryService trackingQueryService,
            IExaminationQueryService assessmentQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IValidator<PreAssessmentResultInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _assessmentQueryService = assessmentQueryService;
            _assessmentQueryService.NotNull(nameof(assessmentQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(PreAssessmentResultInputDto inputDto, int assessmentCode, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.Get(inputDto.TrackId);
            await Validatoin(latestTrackingInfo.TrackId, latestTrackingInfo.StatusId);

            TrackingInsertDuplicateDto trackingInsertSeenAssessmentDto = new(latestTrackingInfo.TrackNumber, _seenByAssessmentStatus, inputDto.Description, assessmentCode, _requestOrigin, true, true);
            TrackingInsertDuplicateDto trackingInsertSetAssessmentResultDto = new(latestTrackingInfo.TrackNumber, _setAssessmentResultStatus, inputDto.Description, assessmentCode, _requestOrigin, true, true, 1);
            TrackingInsertDuplicateDto trackingInserSetArchiveDto = new(latestTrackingInfo.TrackNumber, _archiveStats, inputDto.Description, assessmentCode, _requestOrigin, true, false, 2);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(latestTrackingInfo.ZoneId, null, null, latestTrackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            AssessmentUpdateDto assessmentUpdateDto = await GetAssessmentUpdateDto(inputDto, latestTrackingInfo, moshtrakInfo, assessmentCode, trackingInsertSetAssessmentResultDto.TrackId);

            await ExecuteSqlCommand(latestTrackingInfo.ZoneId, trackingInsertSetAssessmentResultDto, trackingInsertSeenAssessmentDto, trackingInserSetArchiveDto, assessmentUpdateDto);
        }
        private async Task InputValidation(PreAssessmentResultInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task Validatoin(Guid trackId, int previousStatusId)
        {
            if (_assessmentSetTime != previousStatusId)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }


            bool hasResult = await _assessmentQueryService.HasResultByTrackId(trackId);
            if (hasResult)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidSetResultDuplicate);
            }
        }
        private async Task<AssessmentUpdateDto> GetAssessmentUpdateDto(PreAssessmentResultInputDto inputDto, TrackingOutputDto latestTrackingInfo, MoshtrakOutputDto moshtrakInfo, int assessmentCode, Guid trackIdResult)
        {
            string body = await new StreamReader(_contextAccessor.HttpContext.Request.Body).ReadToEndAsync();
            AssessmentGetDto assessmentData = await _assessmentQueryService.Get(assessmentCode);

            return new AssessmentUpdateDto()
            {
                ResultId = inputDto.ResultId,
                Description = inputDto.Description,
                TrackId = latestTrackingInfo.TrackId,
                TrackIdResult = trackIdResult,
                SetResultDateTime = DateTime.Now,
                X1 = "0",
                Y1 = "0",
                X2 = "0",
                Y2 = "0",
                TrenchLenS = 0,
                TrenchLenW = 0,
                AsphaltLenW = 0,
                AsphaltLenS = 0,
                RockyLenS = 0,
                RockyLenW = 0,
                OtherLenS = 0,
                OtherLenW = 0,
                BasementDepth = 0,
                HasMap = false,
                ReadingNumber = moshtrakInfo.ReadingNumber ?? string.Empty,
                Premises = 0,
                HouseValue = 0,
                UsageId = moshtrakInfo.UsageId,
                Accuracy = string.Empty,
                AllInJson = body //JsonSerializer.Serialize<PreAssessmentResultInputDto>(inputDto)
            };
        }
        private async Task ExecuteSqlCommand(int zoneId, TrackingInsertDuplicateDto trackingInsertSetAssessmentResultDto, TrackingInsertDuplicateDto trackingInsertSeenAssessmentDto, TrackingInsertDuplicateDto trackingInserSetArchiveDto, AssessmentUpdateDto assessmentUpdateDto)
        {
            string dbName = GetDbName(zoneId);
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    TrackingCommandService _trackingCommandService = new(connection, transaction);
                    ExaminationCommandService _examinationCommandService = new(connection, transaction);

                    await _trackingCommandService.UpdateIsConsiderdLatest(trackingInsertSetAssessmentResultDto.TrackNumber, true);
                    await _trackingCommandService.InsertDuplicate(trackingInsertSeenAssessmentDto);
                    await _trackingCommandService.InsertDuplicate(trackingInsertSetAssessmentResultDto);
                    await _trackingCommandService.InsertDuplicate(trackingInserSetArchiveDto);
                    await _examinationCommandService.Update(assessmentUpdateDto);

                    transaction.Commit();
                }
            }
        }
    }
}
