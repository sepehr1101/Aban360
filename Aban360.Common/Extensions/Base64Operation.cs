namespace Aban360.Common.Extensions
{
    public static class Base64Operation
    {
        public static async Task<string> GetLogoBase64(CancellationToken cancellationToken)
        {
            const string logoPath = @"AppData\Images\logoBase64.txt";
            if (Path.Exists(logoPath))
                return await File.ReadAllTextAsync(logoPath, cancellationToken);

            return string.Empty;
        }
        public static async Task<string> GetNotFoundBase64(CancellationToken cancellationToken)
        {
            const string logoPath = @"AppData\Images\LogoNotFoundBase64.txt";
            if (Path.Exists(logoPath))
                return await File.ReadAllTextAsync(logoPath, cancellationToken);

            return string.Empty;
        }
        public static async Task<string> GetDudicalDocumentBase64(CancellationToken cancellationToken)
        {
            const string logoPath = @"AppData\Images\JudicalDocumentBase64.txt";
            if (Path.Exists(logoPath))
                return await File.ReadAllTextAsync(logoPath, cancellationToken);

            return string.Empty;
        }
        public static async Task<string> GetDudicalBase64(CancellationToken cancellationToken)
        {
            const string logoPath = @"AppData\Images\JudicalBase64.txt";
            if (Path.Exists(logoPath))
                return await File.ReadAllTextAsync(logoPath, cancellationToken);

            return string.Empty;
        }
    }
}
