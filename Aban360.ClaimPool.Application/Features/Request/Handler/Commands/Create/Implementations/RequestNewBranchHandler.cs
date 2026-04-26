using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Tracking.Commands.Implementations;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations
{
    internal sealed class RequestNewBranchHandler : AbstractBaseConnection, IRequestNewBranchHandler
    {
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IValidator<RequestNewBranchInputDto> _validator;
        static string _insertWayTitle = "رایاب";
        static int _requestOrigin = 12;
        static int _newRequestServiceId = 1;
        static int _firstStepStatusId = 0;

        public RequestNewBranchHandler(
            ICommonMemberQueryService commonMemberQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IValidator<RequestNewBranchInputDto> validator,
            IConfiguration configuration)
            : base(configuration)
        {
            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<(MoshtrakCreateDto, Guid)> Handle(RequestNewBranchInputDto inputDto, int userName, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);
            ZoneIdAndCustomerNumber neighbourCustomerInfo = await _commonMemberQueryService.Get(inputDto.NeighbourBillId);
            await _moshtrakQueryService.CheckOpenRequest(inputDto.NationalCode, neighbourCustomerInfo.ZoneId);
            string dbName = GetDbName(neighbourCustomerInfo.ZoneId);
            MoshtrakCreateDto moshtrakInsertDto;
            Guid trackId = new Guid();

            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    MoshtrakCommandService moshtrakCommandService = new(connection, transaction);
                    TrackingCommandService trackingCommandService = new(connection, transaction);
                    T0CommandService t0CommandService = new(connection, transaction);

                    int trackNumber = (int)(await t0CommandService.GetTrackNumber());
                    moshtrakInsertDto = GetMoshtrackCreateDto(inputDto, trackNumber, neighbourCustomerInfo.ZoneId);
                    TrackingInsertDto trackingInsertDto = GetTrackingCreateDto(inputDto, userName, trackNumber, neighbourCustomerInfo.ZoneId);
                    trackId = trackingInsertDto.TrackId;

                    await moshtrakCommandService.Insert(moshtrakInsertDto, dbName);
                    await trackingCommandService.Insert(trackingInsertDto);

                    transaction.Commit();
                }
            }
            return (moshtrakInsertDto, trackId);
        }
        private async Task Validation(RequestNewBranchInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }
        private MoshtrakCreateDto GetMoshtrackCreateDto(RequestNewBranchInputDto inputDto, int trackNumber, int zoneId)
        {
            MoshtrakServiceDto serviceSelected = MoshtrakService.GetServicesSelected(inputDto.SelectedServices);
            return new MoshtrakCreateDto()
            {
                TrackNumber = trackNumber,
                ServiceGroupId = _newRequestServiceId,
                StringTrackNumber = trackNumber.ToString().PadLeft(11, '0'),
                BillId = string.Empty,
                CustomerNumber = 0,
                NeighbourBillId = inputDto.NeighbourBillId,
                ZoneId = zoneId,
                NotificationMobile = inputDto.MobileNumber,
                UsageId = 0,
                MeterDiameterId = 0,
                BranchTypeId = 0,
                DiscountTypeId = 0,
                DiscountCount = 0,
                PhoneNumber = inputDto.PhoneNumber,
                MobileNumber = inputDto.MobileNumber,
                NationalCode = inputDto.NationalCode,
                FirstName = inputDto.FirstName,
                Surname = inputDto.Surname,
                FatherName = inputDto.FatherName,
                Premises = 0,
                ImprovementCommertial = 0,
                ImprovementDomestic = 0,
                ImprovementOverall = 0,
                Siphon100 = 0,
                Siphon125 = 0,
                Siphon150 = 0,
                Siphon200 = 0,
                MainSiphon = 0,
                CommonSiphon = 0,
                ContractualCapacity = 0,
                HouseValue = 0,
                CommertialUnit = 0,
                DomesticUnit = 0,
                OtherUnit = 0,
                IsNonPermanent = false,
                Address = inputDto.Address,
                PreViewId = string.Empty,
                CounterType = 0,
                InstallAgentState = 0,
                BlockId = string.Empty,
                InsertWayTitle = _insertWayTitle,
                PostalCode = string.Empty,
                ReadingNumber = string.Empty,
                IsSpecial = false,
                BrokerId = 0,//todo8
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
        private TrackingInsertDto GetTrackingCreateDto(RequestNewBranchInputDto inputDto, int userName, int trackNumber, int zoneId)
        {
            return new TrackingInsertDto()
            {
                TrackNumber = trackNumber,
                ZoneId = zoneId,
                BillId = null,
                ServiceGroupId = _newRequestServiceId,
                StatusId = _firstStepStatusId,
                InsertByUserId = userName,
                Description = inputDto.Description,
                NotificationMobile = inputDto.MobileNumber,
                NeighbourBillId = inputDto.NeighbourBillId,
                RequestOrigin = _requestOrigin,
            };
        }
    }
}
