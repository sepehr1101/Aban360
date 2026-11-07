using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.BlobPool.Domain.Features.DmsServices.Entities;
using Aban360.BlobPool.Persistence.Features.DmsServices.Queries.Contracts;
using Microsoft.Extensions.Logging;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations
{
    internal sealed class ArchiveFileTitleEnricher : IArchiveFileTitleEnricher
    {
        private readonly IOpenKmQueryService _openKm;
        private readonly IOpenKmMetaDataQueryServices _metadata;
        private readonly ILogger<ArchiveFileTitleEnricher> _logger;

        public ArchiveFileTitleEnricher(IOpenKmQueryService openKm, IOpenKmMetaDataQueryServices metadata,
            ILogger<ArchiveFileTitleEnricher> logger)
        {
            _openKm = openKm;
            _metadata = metadata;
            _logger = logger;
        }

        public async Task<FileListWithTitlesResponse> Enrich(FileListResponse files, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = new FileListWithTitlesResponse
            {
                Documents = files.Documents?.Select(document => new DocumentWithFileTitle(document)).ToList() ?? new()
            };
            if (result.Documents.Count == 0) return result;

            Dictionary<(string Section, int Value), OpenKmMetaData> reference;
            try
            {
                reference = (await _metadata.Get().WaitAsync(cancellationToken))
                    .GroupBy(item => (item.Section, item.Value))
                    .ToDictionary(group => group.Key, group => group.First());
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Could not read archive title reference data; returning the original file listing without titles.");
                return result;
            }
            var options = new ParallelOptions { MaxDegreeOfParallelism = 4, CancellationToken = cancellationToken };
            await Parallel.ForEachAsync(result.Documents.Where(document => !string.IsNullOrWhiteSpace(document.Uuid))
                .GroupBy(document => document.Uuid, StringComparer.Ordinal), options, async (documents, token) =>
            {
                try
                {
                    var properties = await _openKm.GetMetaDataProperties(documents.Key).WaitAsync(token);
                    string title = string.Empty;
                    foreach (var property in properties?.RawMetaDatas ?? new())
                    {
                        if (property?.Value is int value && reference.TryGetValue((property.Name, value), out var item)
                            && item.KeyLabel == "نوع فایل")
                        {
                            title = string.IsNullOrWhiteSpace(item.Label) ? string.Empty : item.Label;
                            break;
                        }
                    }
                    foreach (var document in documents) document.FileTitle = title;
                }
                catch (OperationCanceledException) when (token.IsCancellationRequested) { throw; }
                catch (Exception exception)
                {
                    _logger.LogWarning(exception, "Could not read archive title for document {DocumentId}.", documents.Key);
                }
            });
            return result;
        }
    }
}
