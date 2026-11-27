using Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Contracts;
using Aban360.Common.Exceptions;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.Runtime.Versioning;

namespace Aban360.BlobPool.Application.Features.OpenKm.Handlers.Commands.Implementations
{
    internal sealed class ReadingImageWatermarker : IReadingImageWatermarker
    {
        public async Task<byte[]> Apply(IFormFile file, string billId, string uploadedBy, DateTime capturedAt, CancellationToken cancellationToken)
        {
            if (!OperatingSystem.IsWindows()) throw new PlatformNotSupportedException("پردازش تصویر با کتابخانه موجود پروژه به Windows نیاز دارد.");
            using var input = new MemoryStream();
            await file.CopyToAsync(input, cancellationToken);
            input.Position = 0;
            cancellationToken.ThrowIfCancellationRequested();
            return Draw(input, billId, uploadedBy, capturedAt, cancellationToken);
        }

        [SupportedOSPlatform("windows")]
        private static byte[] Draw(Stream input, string billId, string uploadedBy, DateTime capturedAt, CancellationToken cancellationToken)
        {
            Image source;
            try { source = Image.FromStream(input, useEmbeddedColorManagement: false, validateImageData: true); }
            catch (Exception exception) when (exception is ArgumentException or OutOfMemoryException)
            {
                throw new CustomValidationException("محتوای فایل یک تصویر معتبر نیست.");
            }
            using (source)
            {
                if (source.RawFormat.Guid != ImageFormat.Jpeg.Guid && source.RawFormat.Guid != ImageFormat.Png.Guid)
                    throw new CustomValidationException("تصویر باید با فرمت JPEG یا PNG باشد.");
                if ((long)source.Width * source.Height > 25_000_000 || source.Width < 160 || source.Height < 120)
                    throw new CustomValidationException("ابعاد تصویر باید حداقل ۱۶۰ در ۱۲۰ و حداکثر ۲۵ مگاپیکسل باشد.");
                if (source.PropertyIdList.Contains(0x0112))
                {
                    var value = source.GetPropertyItem(0x0112)?.Value;
                    if (value is { Length: >= 2 }) source.RotateFlip(BitConverter.ToUInt16(value, 0) switch
                    {
                        2 => RotateFlipType.RotateNoneFlipX, 3 => RotateFlipType.Rotate180FlipNone,
                        4 => RotateFlipType.Rotate180FlipX, 5 => RotateFlipType.Rotate90FlipX,
                        6 => RotateFlipType.Rotate90FlipNone, 7 => RotateFlipType.Rotate270FlipX,
                        8 => RotateFlipType.Rotate270FlipNone, _ => RotateFlipType.RotateNoneFlipNone
                    });
                }
                using var bitmap = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb);
                using var graphics = Graphics.FromImage(bitmap);
                graphics.Clear(Color.White);
                graphics.DrawImage(source, 0, 0, source.Width, source.Height);
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                string text = $"تاریخ: \u200e{capturedAt.ToShortPersianDateString()} {capturedAt.ToString("HH:mm:ss", CultureInfo.InvariantCulture)}\u200e\nشناسه قبض: \u200e{billId}\u200e\nکاربر: {uploadedBy.Replace('\r', ' ').Replace('\n', ' ')}";
                float margin = Math.Max(4, source.Width / 150f);
                float maxWidth = source.Width - margin * 4;
                using var font = new Font("Tahoma", Math.Clamp(source.Width / 70f, 8f, 22f), FontStyle.Regular, GraphicsUnit.Pixel);
                using var format = new StringFormat(StringFormatFlags.DirectionRightToLeft)
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Near
                };
                SizeF size = graphics.MeasureString(text, font, new SizeF(maxWidth, source.Height), format);
                if (size.Height + margin * 4 > source.Height) throw new CustomValidationException("ابعاد تصویر برای درج مشخصات بارگذاری کافی نیست.");
                var panel = new RectangleF(source.Width - size.Width - margin * 3, source.Height - size.Height - margin * 3,
                    size.Width + margin * 2, size.Height + margin * 2);
                using var background = new SolidBrush(Color.FromArgb(155, Color.Black));
                graphics.FillRectangle(background, panel);
                using var foreground = new SolidBrush(Color.White);
                graphics.DrawString(text, font, foreground, new RectangleF(panel.X + margin, panel.Y + margin, size.Width, size.Height), format);
                cancellationToken.ThrowIfCancellationRequested();
                using var output = new MemoryStream();
                using var quality = new EncoderParameters(1);
                quality.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                bitmap.Save(output, ImageCodecInfo.GetImageEncoders().Single(codec => codec.FormatID == ImageFormat.Jpeg.Guid), quality);
                return output.ToArray();
            }
        }
    }
}
