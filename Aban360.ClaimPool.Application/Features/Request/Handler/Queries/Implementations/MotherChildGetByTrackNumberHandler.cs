using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class MotherChildGetByTrackNumberHandler : IMotherChildGetByTrackNumberHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IMotherQueryService _motherQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        public MotherChildGetByTrackNumberHandler(
            IMoshtrakQueryService moshtrakQueryService,
            IMotherQueryService motherQueryService,
            ITrackingQueryService trackingQueryService,
            ICommonMemberQueryService commonMemberQueryService)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _motherQueryService = motherQueryService;
            _motherQueryService.NotNull(nameof(motherQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        }

        public async Task<MotherChildOutputDto> Handle(int trackNumber, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            MoshtrakOutputDto inheritedMoshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            MotherInfoOutputDto? inheritedInfo = await _motherQueryService.Get(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);

            MemberInfoGetDto motherInfo = await _commonMemberQueryService.Get(new ZoneIdAndCustomerNumber(trackingInfo.ZoneId, inheritedInfo.MotherCustomerNumber));

            MotherOutputDto inheritedOutput = new()
            {
                TrackNumber = trackNumber,
                StringTrackNumber = trackingInfo.StringTrackNumber,
                BillId = trackingInfo.BillId,
                CustomerNumbe = inheritedMoshtrakInfo.CustomerNumber,
                ZoneId = inheritedInfo.ZoneId,
                MeterDiameterId = inheritedInfo.MeterDiameterId,
                UsageId = inheritedInfo.UsageId,
                IsSpecial = inheritedInfo.IsSpecial,

                Siphon100 = inheritedInfo.Siphon100,
                Siphon125 = inheritedInfo.Siphon125,
                Siphon150 = inheritedInfo.Siphon150,
                Siphon200 = inheritedInfo.Siphon200,
                CommonSiphon = 0,//inheritedInfo.CommonSiphon, //todo: string*

                CommercialUnit = inheritedInfo.CommercialUnit,
                DomesticUnit = inheritedInfo.DomesticUnit,
                OtherUnit = inheritedInfo.OtherUnit,
                Premises = inheritedInfo.Premises,
                ImprovementCommercial = inheritedInfo.ImprovementCommercial,
                ImprovementDomestic = inheritedInfo.ImprovementDomestic,
                ImprovementOverall = inheritedInfo.ImprovementOverall,
                ContractualCapacity = inheritedInfo.ContractualCapacity,
            };
            MotherOutputDto motherOutput = new()
            {
                TrackNumber = 0,
                BillId = trackingInfo.BillId,
                StringTrackNumber = string.Empty,
                CustomerNumbe = inheritedMoshtrakInfo.CustomerNumber,
                ZoneId = motherInfo.ZoneId,
                MeterDiameterId = motherInfo.MeterDiameterId,
                UsageId = motherInfo.UsageId,
                IsSpecial = motherInfo.IsSpecial,

                Siphon100 = motherInfo.Siphon1,
                Siphon125 = motherInfo.Siphon2,
                Siphon150 = motherInfo.Siphon3,
                Siphon200 = motherInfo.Siphon4,
                CommonSiphon = motherInfo.CommonSiphon1,

                CommercialUnit = motherInfo.CommercialUnit,
                DomesticUnit = motherInfo.DomesticUnit,
                OtherUnit = motherInfo.OtherUnit,
                Premises = motherInfo.Premises,
                ImprovementCommercial = motherInfo.CommercialImprovement,
                ImprovementDomestic = motherInfo.DomesticImprovement,
                ImprovementOverall = motherInfo.DomesticImprovement,
                ContractualCapacity = motherInfo.ContractualCapacity,
            };

            return new MotherChildOutputDto(motherOutput, inheritedOutput);
        }
    }
}
