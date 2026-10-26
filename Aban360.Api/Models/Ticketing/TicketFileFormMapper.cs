using Aban360.Common.Exceptions;
using Aban360.UserPool.Application.Features.Ticketing.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;

namespace Aban360.Api.Models.Ticketing
{
    internal static class TicketFileFormMapper
    {
        public const long MaxRequestBodySize = TicketFileCreateValidator.MaxFileCount
            * TicketFileCreateValidator.MaxFileSizeBytes + 1024 * 1024;
        public const long MaxSingleFileRequestBodySize = TicketFileCreateValidator.MaxFileSizeBytes + 1024 * 1024;

        internal static async Task<ICollection<TicketFileCreateDto>> Read(
            ICollection<IFormFile> files, CancellationToken cancellationToken)
        {
            if (files == null || files.Count > TicketFileCreateValidator.MaxFileCount)
                throw new CustomValidationException("حداکثر پنج پیوست برای هر پیام مجاز است.");
            var result = new List<TicketFileCreateDto>();
            foreach (var file in files)
                result.Add(await Read(file, cancellationToken));
            return result;
        }

        internal static async Task<TicketFileCreateDto> Read(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length <= 0 || file.Length > TicketFileCreateValidator.MaxFileSizeBytes)
                throw new CustomValidationException("فایل باید دارای محتوا و حداکثر ۵۰ مگابایت باشد.");
            await using var input = file.OpenReadStream();
            using var output = new MemoryStream();
            var buffer = new byte[81920];
            int count;
            while ((count = await input.ReadAsync(buffer.AsMemory(), cancellationToken)) > 0)
            {
                if (output.Length + count > TicketFileCreateValidator.MaxFileSizeBytes)
                    throw new CustomValidationException("حجم فایل بیش از حد مجاز است.");
                await output.WriteAsync(buffer.AsMemory(0, count), cancellationToken);
            }
            if (output.Length == 0 || output.Length != file.Length)
                throw new CustomValidationException("محتوای فایل به‌صورت کامل دریافت نشده است.");
            return new TicketFileCreateDto
            {
                OriginalFileName = file.FileName,
                ContentType = file.ContentType,
                FileContent = output.ToArray()
            };
        }
    }
}
