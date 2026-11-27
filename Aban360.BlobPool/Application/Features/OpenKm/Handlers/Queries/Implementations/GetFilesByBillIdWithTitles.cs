using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.ApplicationUser;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations
{
    internal sealed class GetFilesByBillIdWithTitles : IGetFilesByBillIdWithTitles
    {
        private readonly IGetFilesByBillId _files;
        private readonly IArchiveFileTitleEnricher _titles;

        public GetFilesByBillIdWithTitles(IGetFilesByBillId files, IArchiveFileTitleEnricher titles)
        {
            _files = files;
            _titles = titles;
        }

        public async Task<FileListWithTitlesResponse> Handle(string input, string? trackNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var files = await _files.Handle(input, trackNumber, appUser, cancellationToken);
            return await _titles.Enrich(files, cancellationToken);
        }
    }
}
