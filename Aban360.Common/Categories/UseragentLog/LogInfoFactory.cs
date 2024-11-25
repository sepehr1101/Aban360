using DeviceDetectorNET;

namespace Aban360.Common.Categories.UseragentLog
{
    internal static class LogInfoFactory
    {
        internal static LogInfo Create(DeviceDetector dd, ClientHints clientHints, bool skipBotDetection)
        {
            dd.Parse();
            if (skipBotDetection)
            {
                dd.SkipBotDetection();
            }
            LogInfo logInfo = new()
            {
                Client=dd.WithClient(clientHints),
                Device=dd.WithDevice(),
                Os=dd.WithOs(),
                Bot=skipBotDetection? new Bot(): dd.WithBot(),
            };         
            return logInfo;
        }
        private static Client WithClient(this DeviceDetector dd,ClientHints clientHints)
        {
            var clientInfo = dd.GetClient(); // holds information about browser, feed reader, media player, ...
            var clientMatch = clientInfo.Match;
            Client client = new Client();
            if (!(clientMatch is null))
            {
                client.App = clientHints.App;
                client.Platform = clientHints.Platform;
                client.Architecture = clientHints.Architecture;
                client.Type = clientMatch.Type;
                client.Version = clientMatch.Version;
                client.Name = clientMatch.Name;
            }
            return client;
        }
        private static Os WithOs(this DeviceDetector dd)
        {
            var osInfo = dd.GetOs();
            var osMatch = osInfo.Match;
            Os os = new Os();
            if ((osMatch is not null) && osInfo.Success)
            {
                os.Platform = osMatch.Platform;
                os.Family = osMatch.Family;
                os.Name = osMatch.Name;
                os.ShortName = osMatch.ShortName;
            }
            return os;  
        }
        private static Device WithDevice(this DeviceDetector dd)
        {
            var deviceInfo = dd.GetDeviceName();
            Device device = new Device();
            if (dd.IsParsed() && (deviceInfo is not null))
            {
                device.IsBot = dd.IsBot();
                device.IsDesktop = dd.IsDesktop();
                device.IsTablet = dd.IsTablet();
                device.IsMobile = dd.IsMobile();
                device.IsTablet = dd.IsTablet();
                device.IsBrwoser = dd.IsBrowser();
                device.IsTouchEnabled = dd.IsTouchEnabled();
                device.Brand = dd.GetBrandName();
                device.Title = dd.GetModel();
            }
            return device;
        }
        private static Bot WithBot(this DeviceDetector dd)
        {
            var botInfo = dd.GetBot();
            var botMatch = botInfo.Match;
            Bot bot = new();
            if(dd.IsParsed() && (botInfo is not null))
            {
                bot.Category = botMatch.Category;
                bot.Name = botMatch.Name;
                bot.Url = botMatch.Url;
                bot.Producer= botMatch.Producer?.Name;
            };
            return bot;
        }
    }
}