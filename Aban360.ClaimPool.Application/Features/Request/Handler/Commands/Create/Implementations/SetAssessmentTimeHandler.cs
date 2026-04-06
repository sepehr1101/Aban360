using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class SetAssessmentTimeHandler : AbstractBaseConnection, ISetAssessmentTimeHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IAssessmentQueryService _assessmentQueryService;
        private readonly IOfficialHolidayQueryService _officialHolidayQueryService;
        private readonly IValidator<AssessmentSetTimeInputDto> _validator;
        static int _firstStepStatusId = 0;
        static int _setAssessmentTimeStatusId = 10;
        static int _setReAssessmentRequired = 15;
        static int _firstStepExamination = 1;

        public SetAssessmentTimeHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IAssessmentQueryService assessmentQueryService,
            IOfficialHolidayQueryService officialHolidayQueryService,
            IValidator<AssessmentSetTimeInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _assessmentQueryService = assessmentQueryService;
            _assessmentQueryService.NotNull(nameof(assessmentQueryService));

            _officialHolidayQueryService = officialHolidayQueryService;
            _officialHolidayQueryService.NotNull(nameof(officialHolidayQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(AssessmentSetTimeInputDto input, int userName, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await Validation(input, cancellationToken);

            MoshtrakOutputDto moshtrakInfo = await GetMoshtrakInfo(latestTrackingInfo.ZoneId, input.TrackNumber);
            TrackingInsertDuplicateDto trackingInsert = GetTrackingCreateDto(input, userName);
            AssessmentInsertDto assessmentInsert = await GetAssessmentInsertDto(input, latestTrackingInfo, trackingInsert.TrackId, moshtrakInfo);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    ExaminationCommandService examinationCommandService = new(connection, transaction);

                    await trackingCommandService.InsertDuplicate(trackingInsert);
                    await examinationCommandService.Insert(assessmentInsert);

                    transaction.Commit();
                }
            }
        }
        private async Task<TrackingOutputDto> Validation(AssessmentSetTimeInputDto input, CancellationToken cancellationToken)
        {
            await InputDtoValidation(input, cancellationToken);
            FridayValidation(input.AssessmentDateJalali);
            await OfficialHolidayValidation(input.AssessmentDateJalali);
            TrackingOutputDto latestTrackingInfo = await StatusValidation(input.TrackNumber);
            return latestTrackingInfo;
        }
        private async Task InputDtoValidation(AssessmentSetTimeInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task<TrackingOutputDto> StatusValidation(int trackNumber)
        {
            int[] allowedSetTime = { _firstStepStatusId, _setReAssessmentRequired };
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            if (!allowedSetTime.Contains(latestTrackingInfo.StatusId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.CantSetTime);
            }

            return latestTrackingInfo;
        }
        private void FridayValidation(string date)
        {
            string gregorianDate = ConvertDate.JalaliToGregorian(date);
            string[] partsDate = gregorianDate.Split('-');
            int year = int.Parse(partsDate[0]);
            int month = int.Parse(partsDate[1]);
            int day = int.Parse(partsDate[2]);

            DateTime _date = new(year, month, day);
            if (_date.DayOfWeek == DayOfWeek.Friday)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidFridayDate);
            }
        }
        private async Task OfficialHolidayValidation(string date)
        {
            ICollection<OfficialHoliday> officialHolidays = await _officialHolidayQueryService.Get();
            if (officialHolidays.Select(o => o.DateJalali).Contains(date))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidOfficialHolidayDate);
            }
        }

        private async Task<MoshtrakOutputDto> GetMoshtrakInfo(int zoneId, int trackNumber)
        {
            MoshtrakGetDto moshtrakSearch = new(zoneId, null, null, trackNumber);
            IEnumerable<MoshtrakOutputDto> moshtrakListInfo = await _moshtrakQueryService.Get(moshtrakSearch, MoshtrakSearchTypeEnum.ByTrackNumber);
            MoshtrakOutputDto? validMoshtrackRequest = moshtrakListInfo.Where(m => m.IsRegistered == false).FirstOrDefault();
            if (validMoshtrackRequest is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFountOpenRequest);
            }

            return validMoshtrackRequest;
        }
        private TrackingInsertDuplicateDto GetTrackingCreateDto(AssessmentSetTimeInputDto inputDto, int userName)
        {
            return new TrackingInsertDuplicateDto(inputDto.TrackNumber, _setAssessmentTimeStatusId, inputDto.Description, userName);
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
                ResultId = _firstStepExamination,
                Description = inputDto.Description,
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
