using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Timing;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class RequestAfterSaleHandler : AbstractBaseConnection, IRequestAfterSaleHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IExaminationScheduleQueryService _examinationScheduleQueryService;
        private readonly IExaminationQueryService _examinationQueryService;
        private readonly IValidator<RequestAfterSaleInputDto> _validator;
        static int _afterSaleRequestServiceId = 2;
        static int _setAssessmentTimeStatusId = 10;
        static string _insertWayTitle = "رایاب";
        static int _requestOrigin = 12;
        static int _firstStepStatusId = 0;
        public RequestAfterSaleHandler(
            IHttpContextAccessor contextAccessor,
           ICommonMemberQueryService commonMemberQueryService,
           IMoshtrakQueryService moshtrakQueryService,
           IExaminationScheduleQueryService examinationScheduleQueryService,
           IExaminationQueryService examinationQueryService,
           IValidator<RequestAfterSaleInputDto> validator,
           IConfiguration configuration)
           : base(configuration)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor.NotNull(nameof(contextAccessor));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _examinationScheduleQueryService = examinationScheduleQueryService;
            _examinationScheduleQueryService.NotNull(nameof(examinationScheduleQueryService));

            _examinationQueryService = examinationQueryService;
            _examinationQueryService.NotNull(nameof(examinationQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<(MoshtrakCreateDto, Guid)> Handle(RequestAfterSaleInputDto input, int userName, CancellationToken cancellationToken)
        {
            await InputValidation(input, cancellationToken);
            MemberInfoGetDto memberInfo = await OpenRequestValidation(input);
            var (assessmentCode, assessmentDateJalali) = await GetAssessmentDateTime(memberInfo);

            return await SqlCommands(input, memberInfo, userName, assessmentCode, assessmentDateJalali);
        }
        private async Task InputValidation(RequestAfterSaleInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private async Task<MemberInfoGetDto> OpenRequestValidation(RequestAfterSaleInputDto input)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(input.BillId);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _moshtrakQueryService.CheckOpenRequest(memberInfo.CustomerNumber, memberInfo.ZoneId);

            return memberInfo;
        }
        private async Task<(MoshtrakCreateDto, Guid)> SqlCommands(RequestAfterSaleInputDto input, MemberInfoGetDto memberInfo, int userName, int assessmentCode, string assessmentDateJalali)
        {
            MoshtrakCreateDto moshtrakInsertDto;
            Guid trackId = new Guid();
            string dbName = GetDbName(memberInfo.ZoneId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    MoshtrakCommandService moshtrakCommandService = new(connection, transaction);
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    T0CommandService t0CommandService = new(connection, transaction);
                    ExaminationCommandService examinationCommandService = new(connection, transaction);

                    int trackNumber = (int)(await t0CommandService.GetTrackNumber());
                    moshtrakInsertDto = GetMoshtrackCreateDto(input, memberInfo, trackNumber);
                    TrackingInsertDto trackingSetRequestInsertDto = GetTrackingCreateDto(input, memberInfo, userName, trackNumber);
                    trackId = trackingSetRequestInsertDto.TrackId;

                    await moshtrakCommandService.Insert(moshtrakInsertDto, dbName);
                    await trackingCommandService.Insert(trackingSetRequestInsertDto);

                    if (!string.IsNullOrWhiteSpace(assessmentDateJalali))
                    {
                        TrackingInsertDuplicateDto trackingInsertSetTimeDto = new(trackNumber, _setAssessmentTimeStatusId, input.Description, userName, _requestOrigin, true, false);
                        AssessmentInsertDto assessmentInsert = await GetAssessmentInsertDto(trackingInsertSetTimeDto, trackingSetRequestInsertDto, memberInfo, assessmentCode, assessmentDateJalali);
                        await trackingCommandService.UpdateIsConsiderdLatest(trackingSetRequestInsertDto.TrackNumber, true);
                        await trackingCommandService.InsertDuplicate(trackingInsertSetTimeDto);
                        await examinationCommandService.Insert(assessmentInsert);
                    }

                    transaction.Commit();
                }
            }

            return (moshtrakInsertDto, trackId);
        }
        private async Task<(int, string)> GetAssessmentDateTime(MemberInfoGetDto memberInfo)
        {
            IEnumerable<AssessmentScaduleGetDto> assessmentsScadule = await _examinationScheduleQueryService.Get(memberInfo.ZoneId, memberInfo.ReadingNumber);

            for (int offset = 1; offset <= 14; offset++)
            {
                DateTime date = DateTime.Now.Date.AddDays(offset);
                string dateJalali = date.ToShortPersianDateString();

                int dayIndex =
                    date.DayOfWeek == DayOfWeek.Saturday ? 0 :
                    date.DayOfWeek == DayOfWeek.Sunday ? 1 :
                    date.DayOfWeek == DayOfWeek.Monday ? 2 :
                    date.DayOfWeek == DayOfWeek.Tuesday ? 3 :
                    date.DayOfWeek == DayOfWeek.Wednesday ? 4 :
                    date.DayOfWeek == DayOfWeek.Thursday ? 5 :
                                                          6;

                foreach (var eachAssessment in assessmentsScadule)
                {
                    int dayValue = dayIndex switch
                    {
                        0 => eachAssessment.Day0,
                        1 => eachAssessment.Day1,
                        2 => eachAssessment.Day2,
                        3 => eachAssessment.Day3,
                        4 => eachAssessment.Day4,
                        5 => eachAssessment.Day5,
                        6 => eachAssessment.Day6,
                        _ => 0
                    };

                    if (dayValue <= 0) continue;

                    int assessmentTaskCount =
                        await _examinationQueryService.GetWithoutResultInDate(dateJalali, eachAssessment.AssessmentCode);

                    if (assessmentTaskCount < dayValue)
                    {
                        return (eachAssessment.AssessmentCode, dateJalali);
                    }
                }
            }
            return (0, string.Empty);
        }
        private MoshtrakCreateDto GetMoshtrackCreateDto(RequestAfterSaleInputDto inputDto, MemberInfoGetDto memberInfo, int trackNumber)//todo
        {
            MoshtrakServiceDto serviceSelected = MoshtrakService.GetServicesSelected(inputDto.SelectedServices);
            return new MoshtrakCreateDto()
            {
                TrackNumber = trackNumber,
                ServiceGroupId = _afterSaleRequestServiceId,
                StringTrackNumber = trackNumber.ToString().PadLeft(11, '0'),
                BillId = memberInfo.BillId,
                CustomerNumber = memberInfo.CustomerNumber,
                NeighbourBillId = null,
                ZoneId = memberInfo.ZoneId,
                NotificationMobile = inputDto.MobileNumber,
                UsageId = memberInfo.UsageId,
                MeterDiameterId = memberInfo.MeterDiameterId,
                BranchTypeId = memberInfo.UseStateId,
                DiscountTypeId = memberInfo.DiscountId,
                DiscountCount = memberInfo.DiscountCount,
                PhoneNumber = memberInfo.PhoneNumber,
                MobileNumber = memberInfo.MobileNumber,
                NationalCode = memberInfo.NationalCode,
                FirstName = memberInfo.FirstName,
                Surname = memberInfo.Surname,
                FatherName = memberInfo.FatherName,
                Premises = memberInfo.Premises,
                ImprovementCommertial = memberInfo.CommercialImprovement,
                ImprovementDomestic = memberInfo.DomesticImprovement,
                ImprovementOverall = memberInfo.OverallImprovement,
                Siphon100 = memberInfo.Siphon1,
                Siphon125 = memberInfo.Siphon2,
                Siphon150 = memberInfo.Siphon3,
                Siphon200 = memberInfo.Siphon4,
                MainSiphon = int.Parse(memberInfo.MainSiphon),
                CommonSiphon = memberInfo.CommonSiphon1,
                ContractualCapacity = memberInfo.ContractualCapacity,
                HouseValue = 0,//todo
                CommertialUnit = memberInfo.CommercialUnit,
                DomesticUnit = memberInfo.DomesticUnit,
                OtherUnit = memberInfo.OtherUnit,
                IsNonPermanent = false,
                Address = inputDto.Address,
                PreViewId = string.Empty,//todo
                CounterType = memberInfo.DeletionStateId,
                InstallAgentState = 0,//todo
                BlockId = string.IsNullOrWhiteSpace(memberInfo.BlockCode) ? string.Empty : memberInfo.BlockCode,
                InsertWayTitle = _insertWayTitle,
                PostalCode = memberInfo.PostalCode,
                IsSpecial = memberInfo.IsSpecial,
                ReadingNumber = memberInfo.ReadingNumber,
                BrokerId = 0,//todo
                s0 = serviceSelected.s0,
                s1 = serviceSelected.s1,
                s2 = serviceSelected.s2,
                s3 = serviceSelected.s3,
                s4 = serviceSelected.s4,
                s5 = serviceSelected.s5,
                s8 = serviceSelected.s8,
                s9 = serviceSelected.s9,
                s10 = serviceSelected.s10,
                s11 = serviceSelected.s11,
                s12 = serviceSelected.s12,
                s13 = serviceSelected.s13,
                s14 = serviceSelected.s14,
                s15 = serviceSelected.s15,
                s16 = serviceSelected.s16,
                s17 = serviceSelected.s17,
                s18 = serviceSelected.s18,
                s19 = serviceSelected.s19,
                s20 = serviceSelected.s20,
                s21 = serviceSelected.s21,
                s22 = serviceSelected.s22,
                s23 = serviceSelected.s23,
                s24 = serviceSelected.s24,
                s25 = serviceSelected.s25,
                s26 = serviceSelected.s26,
                s27 = serviceSelected.s27,
                s28 = serviceSelected.s28,
                s29 = serviceSelected.s29,
                s30 = serviceSelected.s30,
                s31 = serviceSelected.s31,
                s32 = serviceSelected.s32,
                s33 = serviceSelected.s33,
                s34 = serviceSelected.s34,
                s35 = serviceSelected.s35,
                s36 = serviceSelected.s36,
                s37 = serviceSelected.s37,
                s38 = serviceSelected.s38,
                s39 = serviceSelected.s39,
                s40 = serviceSelected.s40,
                s41 = serviceSelected.s41,
                s42 = serviceSelected.s42,
                s43 = serviceSelected.s43,
                s44 = serviceSelected.s44,
                s45 = serviceSelected.s45,
                s46 = serviceSelected.s46,
                s47 = serviceSelected.s47,
                s48 = serviceSelected.s48,
            };
        }
        private TrackingInsertDto GetTrackingCreateDto(RequestAfterSaleInputDto inputDto, MemberInfoGetDto memberInfo, int userName, int trackNumber)
        {
            return new TrackingInsertDto()
            {
                TrackNumber = trackNumber,
                ZoneId = memberInfo.ZoneId,
                BillId = memberInfo.BillId,
                ServiceGroupId = _afterSaleRequestServiceId,
                StatusId = _firstStepStatusId,
                InsertByUserId = userName,
                Description = inputDto.Description,
                NotificationMobile = inputDto.MobileNumber,
                NeighbourBillId = null,
                RequestOrigin = _requestOrigin
            };
        }
        private async Task<AssessmentInsertDto> GetAssessmentInsertDto(TrackingInsertDuplicateDto trackingInsertSetTimeDto, TrackingInsertDto previousTrackingInfo, MemberInfoGetDto memberInfo, int assessmentCode, string assessmentDateJalali)
        {
            AssessmentGetDto assessmentData = await _examinationQueryService.Get(assessmentCode);
            string body = await new StreamReader(_contextAccessor.HttpContext.Request.Body).ReadToEndAsync();

            return new AssessmentInsertDto()
            {
                TrackNumber = trackingInsertSetTimeDto.TrackNumber,
                BillId = previousTrackingInfo.BillId ?? string.Empty,
                AssessmentCode = assessmentData.Code,
                AssessmentMobile = assessmentData.PhoneNumber,
                AssessmentName = assessmentData.FullName,
                ZoneId = previousTrackingInfo.ZoneId,
                AssessmentDateJalali = assessmentDateJalali,
                AssessmentGregorianDateTime = ConvertDate.JalaliToDateTime(assessmentDateJalali),
                ResultId = null,
                Description = null,
                TrackId = trackingInsertSetTimeDto.TrackId,
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
                ReadingNumber = memberInfo.ReadingNumber ?? string.Empty,
                Premises = 0,
                HouseValue = 0,
                UsageId = memberInfo.UsageId,
                AllInJson = body
            };
        }
    }
}
