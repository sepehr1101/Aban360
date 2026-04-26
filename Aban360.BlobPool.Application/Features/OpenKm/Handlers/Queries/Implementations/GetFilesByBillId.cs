using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations
{
    internal sealed class GetFilesByBillId : IGetFilesByBillId
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        public GetFilesByBillId(IOpenKmQueryService openKmQueryService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
        }

        public async Task<FileListResponse> Handle(string input, string? trackNumber, CancellationToken cancellationToken)
        {
            bool doesFolderExist = await _openKmQueryService.CheckFolderExists(input);
            if (!doesFolderExist)
            {
                if (string.IsNullOrWhiteSpace(trackNumber))
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
    }
}
