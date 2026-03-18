using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class RequestDuplicateValidation : IRequestDuplicateValidation
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        public RequestDuplicateValidation(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            ICommonMemberQueryService commonMemberQueryService)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
        }

        public async Task<TrackingDuplicateValidationOutputDto> Handle(TrackingDuplicateValidationInputDto inputDto, CancellationToken cancellationToken)
        {
            MoshtrakGetDto moshtrakSearch = new();
            MoshtrakSearchTypeEnum moshtrakSearchType = 0;
            if (inputDto.ValidationType == TrackingDuplicateValidationTypeEnum.ByNationalCode)
            {
                ZoneIdAndCustomerNumber neighbourInfo = await _commonMemberQueryService.Get(inputDto.NeighbourBillId);
                moshtrakSearch = new(neighbourInfo.ZoneId, null, inputDto.NationalCode, null);
                moshtrakSearchType = MoshtrakSearchTypeEnum.ByNationalCode;
            }
            if (inputDto.ValidationType == TrackingDuplicateValidationTypeEnum.ByBillId)
            {
                ZoneIdAndCustomerNumber customerInfo = await _commonMemberQueryService.Get(inputDto.BillId);
                moshtrakSearch = new(customerInfo.ZoneId, customerInfo.CustomerNumber, null, null);
                moshtrakSearchType = MoshtrakSearchTypeEnum.ByCustomerNumber;
            }
            IEnumerable<MoshtrakOutputDto> moshtrakInfo = await _moshtrakQueryService.Get(moshtrakSearch, moshtrakSearchType);
            MoshtrakOutputDto latestRequest = moshtrakInfo.FirstOrDefault();

            return new TrackingDuplicateValidationOutputDto()
            {
                ZoneId = latestRequest.ZoneId,
                CustomerNumber = latestRequest.CustomerNumber,
                NationalCode = latestRequest.NationalCode,
                TrackNumber = latestRequest.TrackNumber,
                FirstName = latestRequest.FirstName,
                Surname = latestRequest.Surname,
                FatherName = latestRequest.FatherName,
                MobileNumber = latestRequest.MobileNumber,
                RequestDateJalali = latestRequest.RequestDateJalali,
                IsDuplicate = moshtrakInfo.Where(m => m.IsRegistered == false).Any(),
            };
        }
    }
}
