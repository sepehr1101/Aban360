using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations
{
    internal sealed class GetFilesByBillId : IGetFilesByBillId
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly ICommonZoneService _commonZoneService;
        public GetFilesByBillId(
            IOpenKmQueryService openKmQueryService,
            ICommonMemberQueryService commonMemberQueryService,
            ICommonZoneService commonZoneService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));

            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<FileListResponse> Handle(string input, string? trackNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                await ZoneValidate(appUser, input);
            }

            if (string.IsNullOrWhiteSpace(input) || input == "null")
            {
                throw new BaseException("invalid billId in DMS");
            }
            bool doesFolderExist = await _openKmQueryService.CheckFolderExists(input);
            if (!doesFolderExist)
            {
                if (string.IsNullOrWhiteSpace(trackNumber) || input == "null")
                {
                    throw new BaseException("invalid billId and tracknumber in DMS");
                }
                bool trackNumberFolderExists = await _openKmQueryService.CheckFolderExists($"r_{trackNumber}");
                if (!trackNumberFolderExists)
                {
                    throw new BaseException("invalid billId and tracknumber in DMS");
                }

                //billId folder not exists, tracknumber folder exists
                string uuid = await _openKmQueryService.GetFolderUuid($"r_{trackNumber}");
                await _openKmQueryService.RenameFolder(uuid, input);
            }

            FileListResponse result = await _openKmQueryService.GetFilesByBillId(input);
            return result;
        }
        private async Task ZoneValidate(IAppUser appUser,string input)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(input);
            MemberInfoGetDto memberInfo = await _commonMemberQueryService.Get(zoneIdAndCustomerNumber);
            await _commonZoneService.IsUserInZone(appUser, memberInfo.ZoneId);
        }
    }
}
