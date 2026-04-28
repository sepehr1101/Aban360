using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.Common.Timing;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class SetAssessmentTimeHandler : AbstractBaseConnection, ISetAssessmentTimeHandler
    {
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IExaminationQueryService _assessmentQueryService;
        private readonly IOfficialHolidayQueryService _officialHolidayQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IExaminerOffQueryService _examinerOffQueryService;
        private readonly IValidator<AssessmentSetTimeInputDto> _validator;
        static int _firstStepStatusId = 0;
        static int _setAssessmentTimeStatusId = 10;
        static int _setReAssessmentRequired = 15;
        static int _requestOrigin = 12;

        public SetAssessmentTimeHandler(
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IExaminationQueryService assessmentQueryService,
            IOfficialHolidayQueryService officialHolidayQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            IExaminerOffQueryService examinerOffQueryService,
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

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _examinerOffQueryService = examinerOffQueryService;
            _examinerOffQueryService.NotNull(nameof(examinerOffQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<SetAssessmentTimeDataOutputDto> Handle(AssessmentSetTimeInputDto input, int userName, CancellationToken cancellationToken)
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

                    await trackingCommandService.UpdateIsConsiderdLatest(trackingInsert.TrackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsert);
                    await examinationCommandService.Insert(assessmentInsert);

                    transaction.Commit();
                }
            }

            return await GetOutputDto(input, moshtrakInfo, assessmentInsert, trackingInsert.TrackId);
        }
        private async Task<TrackingOutputDto> Validation(AssessmentSetTimeInputDto input, CancellationToken cancellationToken)
        {
            await InputDtoValidation(input, cancellationToken);
            FridayValidation(input.AssessmentDateJalali);
            await OfficialHolidayValidation(input.AssessmentDateJalali, input.AssessmentCode);
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
        private DateTime FridayValidation(string date)
        {
            DateTime _date = ConvertDate.JalaliToDateTime(date);
            if (_date.DayOfWeek == DayOfWeek.Friday)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidFridayDate);
            }
            return _date;
        }
        private async Task OfficialHolidayValidation(string date, int assessmentCode)
        {
            ICollection<OfficialHoliday> officialHolidays = await _officialHolidayQueryService.Get();
            if (officialHolidays.Select(o => o.DateJalali).Contains(date))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidOfficialHolidayDate);
            }

            IEnumerable<AssessmentOffGetDto> assessmentOffs = await _examinerOffQueryService.Get(assessmentCode);
            if (assessmentOffs.Where(a => a.OffDateJalali == date).Any())
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidOffDate);
            }

            if (date.CompareTo(DateTime.Now.ToShortPersianDateString()) < 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidPreviousDate);
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
            return new TrackingInsertDuplicateDto(inputDto.TrackNumber, _setAssessmentTimeStatusId, inputDto.Description, userName, _requestOrigin, true, false);
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
                TrackIdResult = null,
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
        private async Task<SetAssessmentTimeDataOutputDto> GetOutputDto(AssessmentSetTimeInputDto input, MoshtrakOutputDto moshtrakInfo, AssessmentInsertDto assessmentInsert, Guid trackId)
        {
            IEnumerable<NumericDictionary> moshtrakServiceSelected = MoshtrakService.GetServicesSelectedDto(GetMoshtrakServiceDto(moshtrakInfo));
            string serviceSelected = string.Join(",", moshtrakServiceSelected.Select(m => m.Title));

            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(input.TrackNumber);

            string? neighbourAddress = null;
            if (!string.IsNullOrWhiteSpace(moshtrakInfo.NeighbourBillId))
            {
                ZoneIdAndCustomerNumber neighbourZoneId = await _commonMemberQueryService.Get(moshtrakInfo.NeighbourBillId);
                MemberInfoGetDto neighbourInfo = await _commonMemberQueryService.Get(neighbourZoneId);
                neighbourAddress = neighbourInfo.Address;
            }
            return new SetAssessmentTimeDataOutputDto()
            {
                TrackId = trackId,
                BillId = trackingInfo.BillId,
                ServiceGroupId = trackingInfo.ServiceGroupId,
                TrackNumber = moshtrakInfo.TrackNumber,
                Address = moshtrakInfo.Address,
                FullName = $@"{moshtrakInfo.Surname} {moshtrakInfo.FirstName}",
                MobileNumber = moshtrakInfo.NotificationMobile ?? moshtrakInfo.MobileNumber,
                ServiceSelectedList = serviceSelected,
                NeighbourBillId = moshtrakInfo.NeighbourBillId,
                NeighbourAddress = neighbourAddress,

                AssessmentName = assessmentInsert.AssessmentName,
                AssessmentCode = assessmentInsert.AssessmentCode,
                AssessmentMobileNumber = assessmentInsert.AssessmentMobile,
                AssessmentDateJalai = assessmentInsert.AssessmentDateJalali
            };
        }
        private MoshtrakServiceDto GetMoshtrakServiceDto(MoshtrakOutputDto moshtrakInfo)
        {
            return new MoshtrakServiceDto()
            {
                s0 = moshtrakInfo.s0,
                s1 = moshtrakInfo.s1,
                s2 = moshtrakInfo.s2,
                s3 = moshtrakInfo.s3,
                s4 = moshtrakInfo.s4,
                s5 = moshtrakInfo.s5,
                s8 = moshtrakInfo.s8,
                s9 = moshtrakInfo.s9,
                s10 = moshtrakInfo.s10,
                s11 = moshtrakInfo.s11,
                s12 = moshtrakInfo.s12,
                s13 = moshtrakInfo.s13,
                s14 = moshtrakInfo.s14,
                s15 = moshtrakInfo.s15,
                s16 = moshtrakInfo.s16,
                s17 = moshtrakInfo.s17,
                s18 = moshtrakInfo.s18,
                s19 = moshtrakInfo.s19,
                s20 = moshtrakInfo.s20,
                s21 = moshtrakInfo.s21,
                s22 = moshtrakInfo.s22,
                s23 = moshtrakInfo.s23,
                s24 = moshtrakInfo.s24,
                s25 = moshtrakInfo.s25,
                s26 = moshtrakInfo.s26,
                s27 = moshtrakInfo.s27,
                s28 = moshtrakInfo.s28,
                s29 = moshtrakInfo.s29,
                s30 = moshtrakInfo.s30,
                s31 = moshtrakInfo.s31,
                s32 = moshtrakInfo.s32,
                s33 = moshtrakInfo.s33,
                s34 = moshtrakInfo.s34,
                s35 = moshtrakInfo.s35,
                s36 = moshtrakInfo.s36,
                s37 = moshtrakInfo.s37,
                s38 = moshtrakInfo.s38,
                s39 = moshtrakInfo.s39,
                s40 = moshtrakInfo.s40,
                s41 = moshtrakInfo.s41,
                s42 = moshtrakInfo.s42,
                s43 = moshtrakInfo.s43,
                s44 = moshtrakInfo.s44,
                s45 = moshtrakInfo.s45,
                s46 = moshtrakInfo.s46,
                s47 = moshtrakInfo.s47,
                s48 = moshtrakInfo.s48,
            };
        }
    }
}
