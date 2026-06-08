using System.Drawing.Imaging;
using System.Drawing;
using MapOptions = Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.Common.Extensions;
using Microsoft.Extensions.Options;

namespace Aban360.ReportPool.Infrastructure.Features.Geo
{
    public interface IMapService
    {
        Task<string> GenerateMapBase64(string x, string y);
    }

    public class MapService : IMapService
    {
        private readonly HttpClient _httpClient;
        private readonly MapOptions.MapOptions _mapOptions;
        private const int zoom = 18;
        private const int tileSize = 256;
        public MapService(
            HttpClient httpClient,
           IOptions<MapOptions.MapOptions> mapOptions)
        {
            _httpClient = httpClient;
            _httpClient.NotNull(nameof(httpClient));

            _mapOptions = mapOptions.Value;
            _mapOptions.NotNull(nameof(mapOptions));
        }

        public async Task<string> GenerateMapBase64(string x, string y)
        {
            double latitude = (double.Parse)(y);
            double longitude = (double.Parse)(x);
            var (centerTileX, centerTileY) = LatLonToTile(latitude, longitude, zoom);

            using Bitmap finalBitmap = new Bitmap(tileSize * 3, tileSize * 3);
            using Graphics graphics = Graphics.FromImage(finalBitmap);
            await GetGraphicLocation(finalBitmap, graphics, centerTileX, centerTileY);
            DrawLocationIcon(graphics, latitude, longitude);

            using MemoryStream output = new MemoryStream();
            finalBitmap.Save(output, ImageFormat.Png);
            return Convert.ToBase64String(output.ToArray());
        }
        private async Task GetGraphicLocation(Bitmap finalBitmap, Graphics graphics, int centerTileX, int centerTileY)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    int tileX = centerTileX + dx;
                    int tileY = centerTileY + dy;

                    byte[]? bytes = await GetSingleImage(tileX, tileY);
                    if (bytes is null)
                    {
                        continue;
                    }

                    using MemoryStream ms = new MemoryStream(bytes);
                    using Image tileImage = Image.FromStream(ms);
                    graphics.DrawImage(tileImage, (dx + 1) * tileSize, (dy + 1) * tileSize, tileSize, tileSize);
                }
            }
        }
        private async Task<byte[]?> GetSingleImage(int tileX, int tileY)
        {
            string requestUrl = _mapOptions.BaseUrl + _mapOptions.Road;

            string urlPrameters = $"{zoom}/{tileX}/{tileY}.png";
            string finalyUrl = requestUrl + urlPrameters;
            var response = await _httpClient.GetAsync(finalyUrl);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            byte[] bytes = await response.Content.ReadAsByteArrayAsync();
            return bytes;
        }
        private void DrawLocationIcon(Graphics graphics, double latitude, double longitude)
        {
            var (worldPixelX, worldPixelY) = LatLonToWorldPixel(latitude, longitude, zoom);

            double offsetX = worldPixelX % tileSize;
            double offsetY = worldPixelY % tileSize;

            float pointX = (float)(tileSize + offsetX);
            float pointY = (float)(tileSize + offsetY);

            string pinPath = Path.Combine(AppContext.BaseDirectory, "AppData", "images", "location_icon.png");
            using Image pinImage = Image.FromFile(pinPath);
            int pinWidth = 100;
            int pinHeight = 100;

            float drawX = pointX - pinWidth / 2f;
            float drawY = pointY - pinHeight;

            graphics.DrawImage(pinImage, drawX, drawY, pinWidth, pinHeight);

        }
        private static (int X, int Y) LatLonToTile(double latitude, double longitude, int zoom)
        {
            double latRad = latitude * Math.PI / 180.0;
            int n = 1 << zoom;
            int tileX = (int)Math.Floor((longitude + 180.0) / 360.0 * n);
            int tileY = (int)Math.Floor((1 - Math.Log(Math.Tan(latRad) + 1 / Math.Cos(latRad)) / Math.PI) / 2 * n);

            return (tileX, tileY);
        }
        private static (double X, double Y) LatLonToWorldPixel(double latitude, double longitude, int zoom)
        {
            double sinLat = Math.Sin(latitude * Math.PI / 180.0);
            double mapSize = 256 * Math.Pow(2, zoom);
            double pixelX = (longitude + 180.0) / 360.0 * mapSize;
            double pixelY = (0.5 - Math.Log((1 + sinLat) / (1 - sinLat)) / (4 * Math.PI)) * mapSize;

            return (pixelX, pixelY);
        }
    }
}