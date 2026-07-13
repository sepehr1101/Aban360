using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using DNTPersianUtils.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class RequestCofirmeHandler : AbstractBaseConnection, IRequestCofirmeHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IExaminationQueryService _examinationQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IValidator<RequestConfirmInputDto> _validator;
        private string _currentDateJalali = DateTime.Now.ToShortPersianDateString();
        private int _requestConfirmStatusId = 90;
        private int _validCounterStateCode = 0;
        private int _saleGroupServiceId = 1;
        static int _requestOrigin = 12;
        private int _operator = 666;
        public RequestCofirmeHandler(
            IHttpContextAccessor contextAccessor,
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IExaminationQueryService examinationQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService,
            IValidator<RequestConfirmInputDto> validator,
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

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task Handle(RequestConfirmInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await InputValidate(inputDto, cancellationToken);
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(inputDto.TrackNumber);
            AssessmentDataOutputDto assessmentInfo = await _examinationQueryService.GetLatestByTrackNumber(inputDto.TrackNumber);
            MoshtrakOutputDto? moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, inputDto.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();

            Validate(moshtrakInfo, trackingInfo);
            bool isSale = trackingInfo.ServiceGroupId == _saleGroupServiceId;

            MoshtrakSabtUpdateDto moshtrakUpdateDto = new(moshtrakInfo.Id, true, inputDto.Description);
            TrackingInsertDuplicateDto trackingInsertDto = new(inputDto.TrackNumber, _requestConfirmStatusId, inputDto.Description, int.Parse(appUser.Username), _requestOrigin, true, false);
            CustomerInsertDto? customerInsertDto = new();
            CustomerUpdateDto? customerUpdateDto = new();
            if (isSale)
            {
                customerInsertDto = GetCustomerInsertDto(moshtrakInfo, trackingInfo, assessmentInfo);
            }
            else
            {
                MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(trackingInfo.ZoneId, moshtrakInfo.CustomerNumber));
                customerUpdateDto = GetCustomerUpdateAfterSetNewValues(moshtrakInfo, memberInfo);
            }

            await ExecSql(moshtrakUpdateDto, trackingInsertDto, customerInsertDto, customerUpdateDto, appUser, inputDto.TrackNumber, moshtrakInfo.ZoneId, isSale);
        }
        private async Task ExecSql(MoshtrakSabtUpdateDto moshtrakUpdateDto, TrackingInsertDuplicateDto trackingInsertDto, CustomerInsertDto? customerInsertDto, CustomerUpdateDto? customerUpdateDto, IAppUser appUser, int trackNumber, int zoneId, bool isSale)
        {
            string dbName = GetDbName(zoneId);
            int archMemRecordId = 0;
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    MoshtrakCommandService moshtrakCommandService = new(connection, transaction);
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    MembersCommandService membersCommandService = new(connection, transaction);
                    ArchMemCommandService archMemCommandService = new(connection, transaction);
                    ClientsCommandService clientsCommandService = new(connection, transaction);
                    OpLogWithTransactionCommandService opLogCommandService = new(_contextAccessor, connection, transaction);

                    await moshtrakCommandService.UpdateSabt(moshtrakUpdateDto, dbName);
                    await trackingCommandService.UpdateIsConsiderdLatest(trackNumber, true);
                    await trackingCommandService.InsertDuplicate(trackingInsertDto);
                    if (isSale)
                    {
                        await membersCommandService.Insert(customerInsertDto, dbName);
                        archMemRecordId = await archMemCommandService.InsertNew(customerInsertDto, dbName);
                    }
                    else
                    {
                        await membersCommandService.Update(customerUpdateDto, dbName);
                        archMemRecordId = await archMemCommandService.InsertByPreviousRecord(customerUpdateDto, dbName, dbName);
                    }
                    string opLogText = string.Format(OpLogLiterals.RequestConfirmTracking, trackNumber, zoneId, archMemRecordId);
                    await clientsCommandService.InsertByArchMemId(archMemRecordId, dbName);
                    await opLogCommandService.Insert(opLogText, appUser);

                    transaction.Commit();
                }
            }
        }
        private CustomerInsertDto GetCustomerInsertDto(MoshtrakOutputDto moshtrakInfo, TrackingOutputDto trackingInfo, AssessmentDataOutputDto assessmentInfo)
        {
            return new CustomerInsertDto()
            {
                ZoneId = moshtrakInfo.ZoneId,
                CustomerNumber = moshtrakInfo.CustomerNumber,
                BillId = trackingInfo.BillId,
                X = assessmentInfo.X1 ?? string.Empty,
                Y = assessmentInfo.Y1 ?? string.Empty,
                ReadingNumber = moshtrakInfo.ReadingNumber,//todo
                FirstName = moshtrakInfo.FirstName ?? string.Empty,
                Surname = moshtrakInfo.Surname ?? string.Empty,
                Address = moshtrakInfo.Address ?? string.Empty,
                PostalCode = moshtrakInfo.PostalCode ?? string.Empty,
                Plaque = string.Empty,//
                NationalCode = moshtrakInfo.NationalCode,
                PhoneNumber = moshtrakInfo.PhoneNumber ?? string.Empty,
                MobileNumber = moshtrakInfo.MobileNumber,
                FatherName = moshtrakInfo.FatherName,
                BranchTypeId = moshtrakInfo.BranchTypeId,
                UsageSellId = moshtrakInfo.UsageId,
                UsageConsumptionId = moshtrakInfo.UsageId,
                EmptyUnit = 0,//
                CommertialUnit = moshtrakInfo.CommercialUnit,
                DomesticUnit = moshtrakInfo.DomesticUnit,
                OtherUnit = moshtrakInfo.OtherUnit,
                ImprovementCommertial = moshtrakInfo.ImprovementCommercial,
                ImprovementDomestic = moshtrakInfo.ImprovementDomestic,
                ImprovementOverall = moshtrakInfo.ImprovementOverall,
                Premises = moshtrakInfo.Premises,
                HouseholdNumber = 0,//
                HouseholdDateJalali = string.Empty,//
                MeterDiamterId = moshtrakInfo.MeterDiameterId,
                IsSpecial = moshtrakInfo.IsSpecial,
                ContractualCapacity = moshtrakInfo.ContractualCapacity,
                WaterDebt = 0,
                C20 = 0,
                NAb = 0,
                NFaz = 0,

                MeterInstallationDateJalali = string.Empty,
                MeterRequestDateJalali = _currentDateJalali,
                SewageInstallationDateJalali = string.Empty,
                SewageRequestDateJalali = moshtrakInfo.HasEnsheabFazelab ? _currentDateJalali : string.Empty,
                Siphon100 = moshtrakInfo.Siphon100,
                Siphon125 = moshtrakInfo.Siphon125,
                Siphon150 = moshtrakInfo.Siphon150,
                Siphon200 = moshtrakInfo.Siphon200,
                Siphon5 = 0,//
                Siphon6 = 0,//
                Siphon7 = 0,//
                Siphon8 = 0,//
                MainSiphon = moshtrakInfo.MainSiphon,
                CommonSiphon = moshtrakInfo.CommonSiphon,
                Operator = _operator,
                DeletionStateId = _validCounterStateCode,
                BodySerial = string.Empty,
                MeterRegisterDateJalali = string.Empty,
                SewageRegisterDateJalali = string.Empty,
                GuildId = 0,
            };
        }
        private CustomerUpdateDto GetCustomerUpdateDto(MemberInfoGetDto memberInfo)
        {
            return new CustomerUpdateDto()
            {
                Id = memberInfo.Id,
                ZoneId = memberInfo.ZoneId,
                CustomerNumber = memberInfo.CustomerNumber,
                BillId = memberInfo.BillId,
                X = memberInfo.X ?? string.Empty,
                Y = memberInfo.Y ?? string.Empty,
                ReadingNumber = memberInfo.ReadingNumber,
                FirstName = memberInfo.FirstName ?? string.Empty,
                Surname = memberInfo.Surname ?? string.Empty,
                Address = memberInfo.Address ?? string.Empty,
                PostalCode = memberInfo.PostalCode ?? string.Empty,
                Plaque = memberInfo.Plaque,
                NationalCode = memberInfo.NationalCode,
                PhoneNumber = memberInfo.PhoneNumber ?? string.Empty,
                MobileNumber = memberInfo.MobileNumber,
                FatherName = memberInfo.FatherName,
                BranchTypeId = memberInfo.UseStateId,
                UsageSellId = memberInfo.UsageId,
                UsageConsumptionId = memberInfo.UsageId,
                EmptyUnit = memberInfo.EmptyUnit,
                CommertialUnit = memberInfo.CommercialUnit,
                DomesticUnit = memberInfo.DomesticUnit,
                OtherUnit = memberInfo.OtherUnit,
                ImprovementCommertial = memberInfo.CommercialImprovement,
                ImprovementDomestic = memberInfo.DomesticImprovement,
                ImprovementOverall = memberInfo.OverallImprovement,
                Premises = memberInfo.Premises,
                HouseholdNumber = memberInfo.HouseholdNumber,
                HouseholdDateJalali = memberInfo.HouseholdDateJalali,
                MeterDiamterId = memberInfo.MeterDiameterId,
                IsSpecial = memberInfo.IsSpecial,
                ContractualCapacity = memberInfo.ContractualCapacity,

                MeterInstallationDateJalali = memberInfo.MeterInstallationDateJalali,
                MeterRequestDateJalali = memberInfo.MeterRequestDateJalali,
                SewageInstallationDateJalali = memberInfo.SiphonInstallationDateJalali,
                SewageRequestDateJalali = memberInfo.SiphonRequestDateJalali,
                Siphon100 = memberInfo.Siphon100,
                Siphon125 = memberInfo.Siphon125,
                Siphon150 = memberInfo.Siphon150,
                Siphon200 = memberInfo.Siphon200,
                Siphon5 = memberInfo.Siphon5,
                Siphon6 = memberInfo.Siphon6,
                Siphon7 = memberInfo.Siphon7,
                Siphon8 = memberInfo.Siphon8,
                MainSiphon = int.Parse(memberInfo.MainSiphon),
                CommonSiphon = memberInfo.CommonSiphon1,
                Operator = _operator,
                DeletionStateId = memberInfo.DeletionStateId,
                BodySerial = memberInfo.BodySerial,
                MeterRegisterDateJalali = memberInfo.MeterInstalltionRegisterDateJalali,
                SewageRegisterDateJalali = memberInfo.SiphonInstalltionRegisterDateJalali,
                GuildId = 0,
            };
        }
        private CustomerUpdateDto GetCustomerUpdateAfterSetNewValues(MoshtrakOutputDto moshtrakInfo, MemberInfoGetDto memberInfo)
        {
            CustomerUpdateDto updateDto = GetCustomerUpdateDto(memberInfo);
            MoshtrakServiceDto moshtrackService = GetMoshtrakService(moshtrakInfo);
            if (moshtrackService.HasEnsheabFazelab)
            {
                updateDto.SewageRequestDateJalali = _currentDateJalali;
                updateDto.Siphon100 = moshtrakInfo.Siphon100;
                updateDto.Siphon125 = moshtrakInfo.Siphon125;
                updateDto.Siphon150 = moshtrakInfo.Siphon150;
                updateDto.Siphon200 = moshtrakInfo.Siphon200;
                updateDto.Siphon5 = 0;
                updateDto.Siphon6 = 0;
                updateDto.Siphon7 = 0;
                updateDto.Siphon8 = 0;
                updateDto.MainSiphon = moshtrakInfo.MainSiphon;
                updateDto.CommonSiphon = moshtrakInfo.CommonSiphon;
            }
            if (moshtrackService.HasTaqirName)
            {
                updateDto.FirstName = moshtrakInfo.FirstName ?? string.Empty;
                updateDto.Surname = moshtrakInfo.Surname ?? string.Empty;
                updateDto.FatherName = moshtrakInfo.FatherName ?? string.Empty;
                updateDto.MobileNumber = moshtrakInfo.MobileNumber;
                updateDto.PhoneNumber = moshtrakInfo.PhoneNumber ?? string.Empty;
                updateDto.NationalCode = moshtrakInfo.NationalCode ?? string.Empty;
                updateDto.Address = moshtrakInfo.Address;
                updateDto.PostalCode = moshtrakInfo.PostalCode ?? string.Empty;
            }
            if (moshtrackService.HasTaqirVahed)
            {
                updateDto.CommertialUnit = moshtrakInfo.CommercialUnit;
                updateDto.DomesticUnit = moshtrakInfo.DomesticUnit;
                updateDto.OtherUnit = moshtrakInfo.OtherUnit;
                updateDto.ImprovementCommertial = moshtrakInfo.ImprovementCommercial;
                updateDto.ImprovementDomestic = moshtrakInfo.ImprovementDomestic;
                updateDto.ImprovementOverall = moshtrakInfo.ImprovementOverall;
            }
            if (moshtrackService.HasTaqirKarbari)
            {
                updateDto.UsageSellId = moshtrakInfo.UsageId;
            }
            if (moshtrackService.HasTaqirQotrEnsheab)
            {
                updateDto.MeterDiamterId = moshtrakInfo.MeterDiameterId;
            }
            if (moshtrackService.HasSifoonEzafe)
            {
                updateDto.Siphon100 = moshtrakInfo.Siphon100;
                updateDto.Siphon125 = moshtrakInfo.Siphon125;
                updateDto.Siphon150 = moshtrakInfo.Siphon150;
                updateDto.Siphon200 = moshtrakInfo.Siphon200;
                updateDto.Siphon5 = 0;
                updateDto.Siphon6 = 0;
                updateDto.Siphon7 = 0;
                updateDto.Siphon8 = 0;
            }
            if (moshtrackService.HasTaqirQotrSifoon)
            {
                updateDto.MainSiphon = moshtrakInfo.MainSiphon;
            }
            if (moshtrackService.HasZarfiatQarardadi)
            {
                updateDto.ContractualCapacity = moshtrakInfo.ContractualCapacity;
            }
            return updateDto;
        }
        private async Task InputValidate(RequestConfirmInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private void Validate(MoshtrakOutputDto? moshtrakInfo, TrackingOutputDto trackingInfo)
        {
            if (moshtrakInfo is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidTrackNumber);
            }
            if (moshtrakInfo.IsRegistered)
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotFoundAnyOpenTrack);
            }
            if (trackingInfo.StatusId != _requestConfirmStatusId)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }
            if (moshtrakInfo.CustomerNumber <= 0 || string.IsNullOrWhiteSpace(trackingInfo.BillId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.SetBillId);
            }
        }
        private MoshtrakServiceDto GetMoshtrakService(MoshtrakOutputDto serviceSelected)
        {
            return new MoshtrakServiceDto()
            {
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
    }
}