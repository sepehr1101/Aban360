using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.ApplicationUser;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts
{
    public interface IGetFilesByBillId
    {
        Task<FileListResponse> Handle(string input, string? trackNumber, IAppUser appUser, CancellationToken cancellationToken);
    }
}
