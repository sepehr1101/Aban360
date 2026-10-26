using Aban360.UserPool.Application.Features.Auth.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using FluentValidation;

namespace Aban360.UserPool.Application.Features.Ticketing.Validations
{
    public class TicketFileCreateValidator : BaseValidator<TicketFileCreateDto>
    {
        public const int MaxFileCount = 5;
        public const long MaxFileSizeBytes = 50L * 1024 * 1024;

        public TicketFileCreateValidator()
        {
            RuleFor(x => x.OriginalFileName).NotEmpty().MaximumLength(255)
                .Must(x => x != "." && x != ".." && !x.Any(c => char.IsControl(c) || "<>:\"/\\|?*".Contains(c)))
                .WithMessage("نام فایل معتبر نیست.");
            RuleFor(x => x.ContentType).NotEmpty().MaximumLength(150);
            RuleFor(x => x.FileContent).NotEmpty()
                .Must(x => x.LongLength <= MaxFileSizeBytes).WithMessage("حداکثر حجم هر فایل ۵۰ مگابایت است.");
            RuleFor(x => x).Must(IsSupportedFile).WithMessage("نوع، پسوند یا محتوای فایل پشتیبانی نمی‌شود.");
        }

        private static bool IsSupportedFile(TicketFileCreateDto file)
        {
            var bytes = file.FileContent;
            if (bytes == null || string.IsNullOrWhiteSpace(file.OriginalFileName)) return false;
            var extension = Path.GetExtension(file.OriginalFileName).ToLowerInvariant();
            return file.ContentType switch
            {
                "image/jpeg" => (extension is ".jpg" or ".jpeg") && bytes.AsSpan().StartsWith(new byte[] { 0xff, 0xd8, 0xff }),
                "image/png" => extension == ".png" && bytes.AsSpan().StartsWith(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }),
                "image/gif" => extension == ".gif" && (bytes.AsSpan().StartsWith("GIF87a"u8) || bytes.AsSpan().StartsWith("GIF89a"u8)),
                "image/webp" => extension == ".webp" && bytes.Length >= 12 && bytes.AsSpan().StartsWith("RIFF"u8) && bytes.AsSpan(8, 4).SequenceEqual("WEBP"u8),
                "video/mp4" => extension == ".mp4" && bytes.Length >= 12 && bytes.AsSpan(4, 4).SequenceEqual("ftyp"u8),
                "video/webm" => extension == ".webm" && bytes.AsSpan().StartsWith(new byte[] { 0x1a, 0x45, 0xdf, 0xa3 }),
                _ => false
            };
        }
    }
}
