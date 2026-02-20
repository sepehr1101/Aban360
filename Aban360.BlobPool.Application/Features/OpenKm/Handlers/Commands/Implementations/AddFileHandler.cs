using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations
{
    internal sealed class AddFileHandler : IAddFileHandler
    {
        private readonly IOpenKmQueryService _openKmQueryService;
        public AddFileHandler(IOpenKmQueryService openKmQueryService)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
        }

        public async Task<AddFileDto> Handle(string billId, string localFilePath, CancellationToken cancellationToken)
        {
            return await _openKmQueryService.AddFileByBillId(billId, localFilePath);
        }
        public async Task<AddFileDto> Handle(AddFormFileInput input, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream();
            await input.File.CopyToAsync(stream);
            stream.Position = 0;

            var content = new StreamContent(stream);
            //content.Headers.ContentType = new MediaTypeHeaderValue(input.File.ContentType);
            return await _openKmQueryService.AddFile(GetPath(input.BillId, input.TrackNumber), content);
        }
        public async Task<AddFileDto> Handle(AddBase64FileInput input, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(input.File))
                throw new BaseException("Base64 image content cannot be null or empty." + nameof(input.File));

            byte[] fileBytes;

            try
            {
                fileBytes = Convert.FromBase64String(input.File);
            }
            catch (FormatException ex)
            {
                throw new BaseException("Invalid Base64 string provided." + nameof(input.File) + ex.Message);
            }

            MemoryStream stream = new(fileBytes);
            StreamContent content = new(stream);
            return await _openKmQueryService.AddFile(GetPath(input.BillId, input.TrackNumber), content);
        }
        private string GetPath(string billId, long? trackNumber)
        {
            string path = trackNumber.HasValue ? "requestPath" : "billIdPath";
            return path;
        }
    }
}
