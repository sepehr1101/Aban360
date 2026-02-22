using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations
{
    internal sealed class AddFileHandler : IAddFileHandler
    {  
        private readonly IOpenKmQueryService _openKmQueryService;
        private readonly IAddOrUpdateMetaDataHandler _addMetaHandler;
        public AddFileHandler(
            IOpenKmQueryService openKmQueryService,
            IAddOrUpdateMetaDataHandler addMetaHandler)
        {
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));

            _addMetaHandler = addMetaHandler;
            _addMetaHandler.NotNull(nameof(addMetaHandler));
        }
   
        public async Task<AddFileDto> Handle(AddFormFileInput input, CancellationToken cancellationToken)
        {           
            StreamContent content = await GetStreamContent(input.File);
            return await Handle(input.BillId, input.TrackNumber, input.DocumentTypeId, content, input.File.FileName, cancellationToken);
        }
        public async Task<AddFileDto> Handle(AddBase64FileInput input, CancellationToken cancellationToken)
        {   
            StreamContent content = GetStreamContent(input.File);
            return await Handle(input.BillId, input.TrackNumber, input.DocumentTypeId, content, input.FileName, cancellationToken);
        }

        private async Task<AddFileDto> Handle(string billId, long? trackNumber, int documentTypeId, StreamContent content, string name, CancellationToken cancellationToken)
        {
            string path = GetPath(billId, trackNumber, name);
            await CreateFolder(path);
            AddFileDto addFileDto= await _openKmQueryService.AddFile(path, content, name);
            await _openKmQueryService.MarkNodeAsMetadatable(addFileDto.Uuid, true);
            AddOrUpdateMetaDataDto addMetaDto = new()
            {               
                title=documentTypeId
            };
            await _addMetaHandler.Handle(addMetaDto, addFileDto.Uuid, cancellationToken);
            return addFileDto;
        }
        private string GetPath(string billId, long? trackNumber, string fileName)
        {
            string path = trackNumber.HasValue ? $"r_{trackNumber.Value}" : billId;
            return $@"{path}/{fileName}";
        }
        private StreamContent GetStreamContent(string base64File)
        {
            if (string.IsNullOrWhiteSpace(base64File))
                throw new BaseException("Base64 image content cannot be null or empty." + nameof(base64File));

            byte[] fileBytes;

            try
            {
                fileBytes = Convert.FromBase64String(base64File);
            }
            catch (FormatException ex)
            {
                throw new BaseException("Invalid Base64 string provided." + nameof(base64File) + ex.Message);
            }

            MemoryStream stream = new(fileBytes);
            StreamContent content = new(stream);
            return content;
        }
        private async Task<StreamContent> GetStreamContent(IFormFile file)
        {
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            StreamContent content = new(stream);
            return content;
        }
        private async Task CreateFolder(string path)
        {
            //TODO: if exists skip
            //ELSE: create Folder
        }
    }
}
