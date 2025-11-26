using Microsoft.AspNetCore.Http;

namespace Aban360.Common.Extensions
{
    public static class IoExtensions
    {
        public static async Task<string> SaveToDisk(IFormFile file, string path)
        {
            string filePath = Path.Combine(path, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}
