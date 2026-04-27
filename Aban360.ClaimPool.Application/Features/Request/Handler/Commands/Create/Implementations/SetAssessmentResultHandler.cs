using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
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
using System.Text.Json;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class SetAssessmentResultHandler : AbstractBaseConnection, ISetAssessmentResultHandler
    {
        private readonly IAssessmentQueryService _assessmentQueryService;
        private readonly IValidator<AssessmentResultInputDto> _validator;
        private static int _setAssessmentResultStatus = 110;
        private static int _setAssessmentTimeStatus = 10;
        static int _requestOrigin = 12;
        public SetAssessmentResultHandler(
            IAssessmentQueryService assessmentQueryService,
            IValidator<AssessmentResultInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _assessmentQueryService = assessmentQueryService;
            _assessmentQueryService.NotNull(nameof(assessmentQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(AssessmentResultInputDto inputDto, int assessmentCode, CancellationToken cancellationToken)
        {
            await InputValidation(inputDto, cancellationToken);
            TrackingInsertDuplicateDto trackingInsertDto = new(inputDto.TrackNumber, _setAssessmentResultStatus, inputDto.Description, assessmentCode, _requestOrigin);
            AssessmentUpdateDto assessmentUpdateDto = await GetAssessmentUpdateDto(inputDto, assessmentCode, trackingInsertDto.TrackId);
            await Validation(inputDto.TrackingId);

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MoshtrakCommandService _moshtrackCommandService = new(connection, transaction);
                    TrackingCommandService _trackingCommandService = new(connection, transaction);
                    ExaminationCommandService _assessmentCommandService = new(connection, transaction);
                    string dbName = GetDbName(inputDto.ZoneId);

                    await _trackingCommandService.UpdateIsConsiderdLatest(inputDto.TrackNumber, true);
                    await _trackingCommandService.InsertDuplicate(trackingInsertDto);
                    //await _moshtrackCommandService.Update(GetMoshtrackUpdateDto(inputDto), dbName);//todo: uncommited
                    await _assessmentCommandService.Update(assessmentUpdateDto);

                    transaction.Commit();
                }
            }
        }
        private MoshtrkUpdateDto GetMoshtrackUpdateDto(AssessmentResultInputDto inputDto)
        {
            MoshtrakServiceDto serviceSelected = MoshtrakService.GetServicesSelected(inputDto.SelectedServices);

            return new MoshtrkUpdateDto()
            {
                TrackingId = inputDto.TrackingId,
                TrackNumber = inputDto.TrackNumber,
                ServiceGroupId = inputDto.ServiceGroupId,
                StringTrackNumber = inputDto.TrackNumber.ToString().PadLeft(11, '0'),//inputDto.StringTrackNumber,
                BillId = inputDto.BillId,
                CustomerNumber = inputDto.CustomerNumber,
                NeighbourBillId = inputDto.NeighbourBillId,
                ZoneId = inputDto.ZoneId,
                NotificationMobile = inputDto.NotificationMobile,
                UsageId = inputDto.UsageId,
                MeterDiameterId = inputDto.MeterDiameterId,
                BranchTypeId = inputDto.BranchTypeId,
                DiscountCount = inputDto.DiscountCount,
                TrackingResultId = inputDto.TrackingResultId,
                PhoneNumber = inputDto.PhoneNumber,
                MobileNumber = inputDto.MobileNumber,
                FirstName = inputDto.FirstName,
                Surname = inputDto.Surname,
                Premises = inputDto.Premises,
                ImprovementCommertial = inputDto.ImprovementCommertial,
                ImprovementDomestic = inputDto.ImprovementDomestic,
                ImprovementOverall = inputDto.ImprovementOverall,
                Siphon100 = inputDto.Siphon100,
                Siphon125 = inputDto.Siphon125,
                Siphon150 = inputDto.Siphon150,
                Siphon200 = inputDto.Siphon200,
                ContractualCapacity = inputDto.ContractualCapacity,
                HouseValue = inputDto.HouseValue,
                CommertialUnit = inputDto.CommertialUnit,
                DomesticUnit = inputDto.DomesticUnit,
                OtherUnit = inputDto.OtherUnit,
                DiscountTypeId = inputDto.DiscountTypeId,
                NationalCode = inputDto.NationalCode,
                FatherName = inputDto.FatherName ?? string.Empty,
                PostalCode = inputDto.PostalCode,
                IsNonPermanent = inputDto.IsNonPermanent,
                Address = inputDto.Address,
                PreViewId = inputDto.PreViewId,
                CounterType = inputDto.CounterType,
                InstallAgentState = inputDto.InstallAgentState,
                BlockId = inputDto.BlockId,
                MainSiphon = inputDto.MainSiphon,
                CommonSiphon = inputDto.CommonSiphon,

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
        private async Task<AssessmentUpdateDto> GetAssessmentUpdateDto(AssessmentResultInputDto inputDto, int assessmentCode, Guid trackIdResult)
        {
            AssessmentGetDto assessmentData = await _assessmentQueryService.Get(assessmentCode);
            return new AssessmentUpdateDto()
            {
                ResultId = inputDto.ResultId,
                Description = inputDto.Description,
                TrackId = inputDto.TrackingId,
                TrackIdResult = trackIdResult,
                SetResultDateTime = DateTime.Now,
                X1 = inputDto.X1,
                Y1 = inputDto.Y1,
                X2 = inputDto.X2,
                Y2 = inputDto.Y2,
                TrenchLenS = inputDto.TrenchLenS,
                TrenchLenW = inputDto.TrenchLenW,
                AsphaltLenW = inputDto.AsphaltLenW,
                AsphaltLenS = inputDto.AsphaltLenS,
                RockyLenS = inputDto.RockyLenS,
                RockyLenW = inputDto.RockyLenW,
                OtherLenS = inputDto.OtherLenS,
                OtherLenW = inputDto.OtherLenW,
                BasementDepth = inputDto.BasementDepth,
                HasMap = inputDto.HasMap,
                ReadingNumber = inputDto.ReadingNumber,
                Premises = inputDto.Premises,
                HouseValue = inputDto.HouseValue,
                UsageId = inputDto.UsageId,
                Accuracy = inputDto.Accuracy,
                AllInJson = JsonSerializer.Serialize<AssessmentResultInputDto>(inputDto)
            };
        }
        private async Task Validation(Guid trackId)
        {
            //if (previousStatusId != _setAssessmentTimeStatus)
            //{
            //    throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            //}
            bool hasResult = await _assessmentQueryService.HasResultByTrackId(trackId);
            if (hasResult)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidSetResultDuplicate);
            }
        }
        private async Task InputValidation(AssessmentResultInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
    }
}
