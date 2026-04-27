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
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class SetLightAssessmentResultHandler : AbstractBaseConnection, ISetLightAssessmentResultHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IAssessmentQueryService _assessmentQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IValidator<LightAssessmentResultInputDto> _validator;
        private static int _setAssessmentResultStatus = 110;
        private static int _seenByAssessmentStatus = 150;
        static int _assessmentSetTime = 10;
        static int _requestOrigin = 12;
        public SetLightAssessmentResultHandler(
            ITrackingQueryService trackingQueryService,
            IAssessmentQueryService assessmentQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IValidator<LightAssessmentResultInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _assessmentQueryService = assessmentQueryService;
            _assessmentQueryService.NotNull(nameof(assessmentQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(LightAssessmentResultInputDto inputDto, int assessmentCode, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            await Validatoin(latestTrackingInfo.TrackId, latestTrackingInfo.StatusId);

            TrackingInsertDuplicateDto trackingInsertSeenAssessmentDto = new(inputDto.TrackNumber, _seenByAssessmentStatus, inputDto.Description, assessmentCode, _requestOrigin, true, true);
            TrackingInsertDuplicateDto trackingInsertSetAssessmentResultDto = new(inputDto.TrackNumber, _setAssessmentResultStatus, inputDto.Description, assessmentCode, _requestOrigin, true, false);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(latestTrackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            AssessmentUpdateDto assessmentUpdateDto = await GetAssessmentUpdateDto(inputDto, latestTrackingInfo, moshtrakInfo, assessmentCode, trackingInsertSetAssessmentResultDto.TrackId);

            await ExecuteSqlCommand(latestTrackingInfo.ZoneId, trackingInsertSetAssessmentResultDto, trackingInsertSeenAssessmentDto, assessmentUpdateDto);
        }
        private async Task InputValidation(LightAssessmentResultInputDto input, CancellationToken cancellationToken)
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
        private async Task<AssessmentUpdateDto> GetAssessmentUpdateDto(LightAssessmentResultInputDto inputDto, TrackingOutputDto latestTrackingInfo, MoshtrakOutputDto moshtrakInfo, int assessmentCode, Guid trackIdResult)
        {
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
                AllInJson = JsonSerializer.Serialize<LightAssessmentResultInputDto>(inputDto)
            };
        }
        private async Task ExecuteSqlCommand(int zoneId, TrackingInsertDuplicateDto trackingInsertSetAssessmentResultDto, TrackingInsertDuplicateDto trackingInsertSeenAssessmentDto, AssessmentUpdateDto assessmentUpdateDto)
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
                    await _examinationCommandService.Update(assessmentUpdateDto);

                    transaction.Commit();
                }
            }
        }
    }
}
