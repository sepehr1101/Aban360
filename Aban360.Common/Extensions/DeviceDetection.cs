using Aban360.Common.Entities.UseragentLog;
using DeviceDetectorNET;
using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Parser;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Numerics;

namespace Aban360.Common.Extensions
{
    public static class DeviceDetection
    {
        //source  of this projcet at :https://github.com/totpero/DeviceDetector.NET
        public static LogInfo GetLogInfo(HttpRequest request)
        {
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            var userAgent = request.Headers["User-Agent"]; // change this to the useragent you want to parse
            var headers = request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
            var clientHints = ClientHints.Factory(headers);  // client hints are optional

            var dd = new DeviceDetector(userAgent, clientHints);
            dd.SkipBotDetection();

            dd.Parse();
            var clientInfo = dd.GetClient(); // holds information about browser, feed reader, media player, ...
            var osInfo = dd.GetOs();
            var device = dd.GetDeviceName();
            var brand = dd.GetBrandName();
            var model = dd.GetModel();
            return GenerateLogInfo(dd, clientHints);
        }
        private static LogInfo GenerateLogInfo(DeviceDetector dd, ClientHints clientHints )
        {
            dd.Parse();
            var clientInfo = dd.GetClient(); // holds information about browser, feed reader, media player, ...
            var osInfo = dd.GetOs();
            var deviceInfo = dd.GetDeviceName();
            var brand = dd.GetBrandName();
            var model = dd.GetModel();
            var clientMatch=clientInfo.Match;
            Client client = new Client();
            if (!(clientMatch is null))
            {
                client.App= clientHints.App;
                client.Platform= clientHints.Platform;
                client.Architecture= clientHints.Architecture;
                client.Type= clientMatch.Type;
                client.Version= clientMatch.Version;
                client.Name= clientMatch.Name;
            }
            var osMatch=osInfo.Match;
            Os os= new Os();
            if (!(osMatch is null) && osInfo.Success)
            {
                os.Platform= osMatch.Platform;
                os.Family= osMatch.Family;
                os.Name= osMatch.Name;
                os.ShortName= osMatch.ShortName;
            }

            return new LogInfo();
        }
        public static void GetLogInfoWithBot(HttpRequest request)
        {
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            var userAgent = request.Headers["User-Agent"]; // change this to the useragent you want to parse
            var headers = request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
            var clientHints = ClientHints.Factory(headers);  // client hints are optional

            var dd = new DeviceDetector(userAgent, clientHints);

            // OPTIONAL: Set caching method
            // By default static cache is used, which works best within one php process (memory array caching)
            // To cache across requests use caching in files or memcache
            // add using DeviceDetectorNET.Cache;
            //dd.SetCache(new DictionaryCache());

            // OPTIONAL: If called, GetBot() will only return true if a bot was detected  (speeds up detection a bit)
            //dd.DiscardBotInformation();

            // OPTIONAL: If called, bot detection will completely be skipped (bots will be detected as regular devices then)
            dd.SkipBotDetection();

            dd.Parse();

            if (dd.IsBot())
            {
                // handle bots,spiders,crawlers,...
                var botInfo = dd.GetBot();
            }
            else
            {
                var clientInfo = dd.GetClient(); // holds information about browser, feed reader, media player, ...
                var osInfo = dd.GetOs();
                var device = dd.GetDeviceName();
                var brand = dd.GetBrandName();
                var model = dd.GetModel();
            }
        }
        public static Tuple<bool, string> IsBot(HttpRequest request)
        {
            var userAgent = request.Headers["User-Agent"];
            var botParser = new BotParser();
            botParser.SetUserAgent(userAgent);

            // OPTIONAL: discard bot information. Parse() will then return true instead of information
            botParser.DiscardDetails = true;

            var result = botParser.Parse();

            if (result != null)
            {
                return new Tuple<bool,string>(false,string.Empty);
            }
            if(result is not null)
            {
                return new Tuple<bool, string>(true, result.ParserName);
            }
            return new Tuple<bool, string>(true,string.Empty);
        }
    }
}
