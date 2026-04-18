using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
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
    internal sealed class RequestAfterSaleHandler : AbstractBaseConnection, IRequestAfterSaleHandler
    {
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IValidator<RequestAfterSaleInputDto> _validator;
        static string _insertWayTitle = "رایاب";
        static int _requestOrigin = 12;
        static int _afterSaleRequestServiceId = 2;
        static int _firstStepStatusId = 0;

        public RequestAfterSaleHandler(
           ICommonMemberQueryService commonMemberQueryService,
           IMoshtrakQueryService moshtrakQueryService,
           IValidator<RequestAfterSaleInputDto> validator,
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

        public async Task Handle(RequestAfterSaleInputDto input, int userName, CancellationToken cancellationToken)
        {
            await InputValidation(input, cancellationToken);
            MemberInfoGetDto memberInfo = await OpenRequestValidation(input);
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

                    int trackNumber = (int)(await t0CommandService.GetTrackNumber());
                    MoshtrakCreateDto moshtrak = GetMoshtrackCreateDto(input, memberInfo, trackNumber);
                    TrackingInsertDto tracking = GetTrackingCreateDto(input, memberInfo, userName, trackNumber);

                    await moshtrakCommandService.Insert(moshtrak, dbName);
                    await trackingCommandService.Insert(tracking);

                    transaction.Commit();
                }
            }

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
                BrokerId=0,//todo
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
                RequestOrigin = _requestOrigin,
            };
        }
    }
}
