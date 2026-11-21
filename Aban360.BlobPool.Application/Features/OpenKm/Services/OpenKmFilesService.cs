using Aban260.BlobPool.Infrastructure.Providers.OpenKm.Contracts;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands;
using Aban360.BlobPool.Domain.Providers.Dto;
using Aban360.BlobPool.Persistence.Features.DmsServices.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Constants.Literals;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations;
using Aban360.BlobPool.Domain.Features.OpenKm;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Queries.Implementations;
using Aban360.BlobPool.Application.Features.OpenKm.Services;

namespace Aban360.BlobPool.Application.Features.OpenKm.Services
{
    public interface IOpenKmFilesService
    {
        Task<AddFileDto> AddFileAsync(AddFormFileInput input, IAppUser appUser, CancellationToken cancellationToken);
        Task<AddFileDto> AddFileAsync(AddBase64FileInput input, IAppUser appUser, CancellationToken cancellationToken);
        Task<AddFileDto> AddFileAsync(AddDiscountFileInput input, IAppUser appUser, CancellationToken cancellationToken);
        Task RemoveFileAsync(RemoveFileDto removeFileDto, IAppUser appUser, CancellationToken cancellationToken);
        Task<byte[]> GetDocumentBinaryAsync(string documentId, bool isThumbnail, CancellationToken cancellationToken);
        Task<string> GetDownloadLinkAsync(string uuid, bool oneTimeUse, CancellationToken cancellationToken);
        Task<FileListResponse> DisplayRemovedFilesAsync(RemovedFilesInput input, CancellationToken cancellationToken);
    }

    internal sealed class OpenKmFilesService : IOpenKmFilesService
    {
        private readonly OpenKmOptions _options;
        private readonly IOpenKmQueryService _openKmQueryService;
        private readonly IOpenKmFoldersMetadataService _addMetaHandler;
        private readonly IOpenKmMetaDataQueryServices _matadataService;
        private readonly IOpLogCommandService _opLogCommandService;
        private const string _RemoveFiledeleteFolderName = "deleted";
        public OpenKmFilesService(IOptions<OpenKmOptions> options, IOpenKmQueryService openKmQueryService, IOpenKmFoldersMetadataService addMetaHandler, IOpenKmMetaDataQueryServices metadataService, IOpLogCommandService opLogCommandService)
        {
            _options = options.Value;
            _options.NotNull(nameof(_options));
            _openKmQueryService = openKmQueryService;
            _openKmQueryService.NotNull(nameof(openKmQueryService));
            _addMetaHandler = addMetaHandler;
            _addMetaHandler.NotNull(nameof(addMetaHandler));
            _matadataService = metadataService;
            _matadataService.NotNull(nameof(_addMetaHandler));
            _addMetaHandler.NotNull(nameof(_addMetaHandler));
            _opLogCommandService = opLogCommandService;
            _opLogCommandService.NotNull(nameof(_opLogCommandService));
            _opLogCommandService.NotNull(nameof(opLogCommandService));
            _openKmQueryService.NotNull(nameof(_openKmQueryService));
        }

        public async Task<AddFileDto> AddFileAsync(AddFormFileInput input, IAppUser appUser, CancellationToken cancellationToken)
        {
            StreamContent content = await AddFileGetStreamContent(input.File);
            return await AddFileAsync(input.BillId, input.TrackNumber, input.DocumentTypeId, content, input.File.FileName, appUser, cancellationToken);
        }

        public async Task<AddFileDto> AddFileAsync(AddBase64FileInput input, IAppUser appUser, CancellationToken cancellationToken)
        {
            StreamContent content = AddFileGetStreamContent(input.File);
            return await AddFileAsync(input.BillId, input.TrackNumber, input.DocumentTypeId, content, input.FileName, appUser, cancellationToken);
        }

        public async Task<AddFileDto> AddFileAsync(AddDiscountFileInput input, IAppUser appUser, CancellationToken cancellationToken)
        {
            StreamContent content = await AddFileGetStreamContent(input.File);
            int documentTypeValue = await _matadataService.GetFileValue(input.DocumentTypeId);
            var(folderName, filePath) = AddFileGetFoldernameAndPath(input);
            string folderUuid = await _addMetaHandler.CreateFolderAsync(folderName, cancellationToken);
            AddFileDto addFileDto = await _openKmQueryService.AddFileDiscount(filePath, content, input.File.FileName);
            await _openKmQueryService.MarkNodeAsMetadatable(addFileDto.Uuid, true);
            AddOrUpdateMetaDataDto addMetaDto = new()
            {
                title = documentTypeValue
            };
            await _addMetaHandler.AddOrUpdateMetaDataAsync(addMetaDto, addFileDto.Uuid, cancellationToken);
            string opLogText = string.Format(OpLogLiterals.OpenKmAddFileOpLog, addFileDto.Uuid, addMetaDto.title);
            await _opLogCommandService.Insert(opLogText, appUser);
            return addFileDto;
        }

        private async Task<AddFileDto> AddFileAsync(string billId, long? trackNumber, int documentTypeId, StreamContent content, string name, IAppUser appUser, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(billId) && !trackNumber.HasValue)
            {
                throw new BaseException("خطای پارامتر، شماره پیگیری و شناسه قبض هر دو بدون مقدار هستند");
            }

            int dotIndex = name.LastIndexOf('.');
            name = dotIndex > 0 ? name + name.Insert(dotIndex, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")) : name;
            int documentTypeValue = await _matadataService.GetFileValue(documentTypeId);
            var(folderName, filePath) = AddFileGetFoldernameAndPath(billId, trackNumber, name);
            string folderUuid = await _addMetaHandler.CreateFolderAsync(folderName, cancellationToken);
            AddFileDto addFileDto = await _openKmQueryService.AddFile(filePath, content, name);
            await _openKmQueryService.MarkNodeAsMetadatable(addFileDto.Uuid, true);
            AddOrUpdateMetaDataDto addMetaDto = new()
            {
                title = documentTypeValue
            };
            await _addMetaHandler.AddOrUpdateMetaDataAsync(addMetaDto, addFileDto.Uuid, cancellationToken);
            string opLogText = string.Format(OpLogLiterals.OpenKmAddFileOpLog, addFileDto.Uuid, addMetaDto.title);
            await _opLogCommandService.Insert(opLogText, appUser);
            return addFileDto;
        }

        private (string, string) AddFileGetFoldernameAndPath(string billId, long? trackNumber, string fileName)
        {
            string path = trackNumber.HasValue ? $"r_{trackNumber.Value}" : billId;
            return (path, $@"{path}/{fileName}");
        }

        private (string, string) AddFileGetFoldernameAndPath(AddDiscountFileInput input)
        {
            string path = $"{_options.BaseDiscountPath}";
            return (path, $@"{path}/{input.Id}");
        }

        private StreamContent AddFileGetStreamContent(string base64File)
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

        private async Task<StreamContent> AddFileGetStreamContent(IFormFile file)
        {
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            StreamContent content = new(stream);
            return content;
        }

        public async Task RemoveFileAsync(RemoveFileDto removeFileDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            string directory = removeFileDto.IsBillId ? removeFileDto.FolderName : $"r_{removeFileDto.FolderName}";
            try
            {
                await _addMetaHandler.CreateFolderAsync(_RemoveFiledeleteFolderName, cancellationToken, $"{directory}");
            }
            catch
            {
                //برای مواردی که شناسه قبض موجود است اما هنوز مشترک قطعی نشده، بنابراین باید الزاماً با شماره پیگیری پیدا شود
                directory = $"r_{removeFileDto.TrackNumber}";
                await _addMetaHandler.CreateFolderAsync(_RemoveFiledeleteFolderName, cancellationToken, directory);
            }

            await _openKmQueryService.Move(removeFileDto.Uuid, $"{directory}/{_RemoveFiledeleteFolderName}");
            string opLogText = string.Format(OpLogLiterals.OpenKmRemoveFileOpLog, removeFileDto.Uuid, directory);
            await _opLogCommandService.Insert(opLogText, appUser);
        }

        public async Task<byte[]> GetDocumentBinaryAsync(string documentId, bool isThumbnail, CancellationToken cancellationToken)
        {
            byte[] image = isThumbnail ? await _openKmQueryService.GetImageThumbnail(documentId) : await _openKmQueryService.GetFileBinary(documentId);
            if (GetDocumentBinaryIsTiff(image))
            {
                using var inputStream = new MemoryStream(image);
                using var bitmap = new Bitmap(inputStream);
                using var outputStream = new MemoryStream();
                bitmap.Save(outputStream, ImageFormat.Jpeg);
                return outputStream.ToArray();
            }

            return image;
        }

        private bool GetDocumentBinaryIsTiff(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 4)
                return false;
            return // Little-endian: "II" 2A 00
            (bytes[0] == 0x49 && bytes[1] == 0x49 && bytes[2] == 0x2A && bytes[3] == 0x00) || // Big-endian:    "MM" 00 2A
            (bytes[0] == 0x4D && bytes[1] == 0x4D && bytes[2] == 0x00 && bytes[3] == 0x2A);
        }

        public async Task<string> GetDownloadLinkAsync(string uuid, bool oneTimeUse, CancellationToken cancellationToken)
        {
            string result = await _openKmQueryService.GetDownloadLink(uuid, oneTimeUse);
            return result;
        }

        public async Task<FileListResponse> DisplayRemovedFilesAsync(RemovedFilesInput input, CancellationToken cancellationToken)
        {
            string directory = input.IsBillId ? input.FolderName : $"r_{input.FolderName}";
            FileListResponse result = await _openKmQueryService.GetRemovedFiles(directory);
            return result;
        }
    }
}
