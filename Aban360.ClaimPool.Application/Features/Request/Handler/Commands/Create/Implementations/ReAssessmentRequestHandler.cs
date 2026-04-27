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
using Aban360.Common.Timing;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class ReAssessmentRequestHandler : AbstractBaseConnection, IReAssessmentRequestHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IAssessmentQueryService _assessmentQueryService;
        static int _reAssessmentStatusId = 15;
        static int _deletedSatatus = 90000;
        static int _requestOrigin = 12;
        public ReAssessmentRequestHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IAssessmentQueryService assessmentQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _assessmentQueryService = assessmentQueryService;
            _assessmentQueryService.NotNull(nameof(assessmentQueryService));
        }

        public async Task Handle(AssessmentSetTimeInputDto inputDto, int userCode, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(latestTrackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            Validation(inputDto.TrackNumber, latestTrackingInfo.StatusId, moshtrakInfo.IsRegistered);

            TrackingInsertDuplicateDto trackingInsertDto = GetTrackingInsertDto(inputDto, userCode);
            AssessmentInsertDto assessmentInsert = await GetAssessmentInsertDto(inputDto, latestTrackingInfo, trackingInsertDto.TrackId, moshtrakInfo);
            string dbName = GetDbName(latestTrackingInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    ExaminationCommandService examinationCommandService = new(connection, transaction);

                    await trackingCommandService.UpdateIsConsiderdLatest(inputDto.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertDto);
                    await examinationCommandService.Insert(assessmentInsert);

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
        private TrackingInsertDuplicateDto GetTrackingInsertDto(AssessmentSetTimeInputDto inputDto, int userCode)
        {
            return new TrackingInsertDuplicateDto(inputDto.TrackNumber, _reAssessmentStatusId, inputDto.Description, userCode, _requestOrigin);
        }
        private async Task<AssessmentInsertDto> GetAssessmentInsertDto(AssessmentSetTimeInputDto inputDto, TrackingOutputDto latestTrackingInfo, Guid newTrackId, MoshtrakOutputDto moshtrakInfo)
        {
            AssessmentGetDto assessmentData = await _assessmentQueryService.Get(inputDto.AssessmentCode);

            return new AssessmentInsertDto()
            {
                TrackNumber = inputDto.TrackNumber,
                BillId = latestTrackingInfo.BillId ?? string.Empty,
                AssessmentCode = assessmentData.Code,
                AssessmentMobile = assessmentData.PhoneNumber,
                AssessmentName = assessmentData.FullName,
                ZoneId = latestTrackingInfo.ZoneId,
                AssessmentDateJalali = inputDto.AssessmentDateJalali,
                AssessmentGregorianDateTime = ConvertDate.JalaliToDateTime(inputDto.AssessmentDateJalali),
                ResultId = null,
                Description = null,
                TrackId = newTrackId,
                TrackIdResult = Guid.Empty,//todo
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
                HasMap = false,//
                ReadingNumber = moshtrakInfo.ReadingNumber ?? string.Empty,
                Premises = 0,
                HouseValue = 0,
                UsageId = moshtrakInfo.UsageId,
                AllInJson = JsonSerializer.Serialize<AssessmentSetTimeInputDto>(inputDto)
            };
        }
    }
}
