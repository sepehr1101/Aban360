using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace Aban360.Common.Categories.UseragentLog
{
    public static class LogInfoJson
    {
        public static string Get(HttpRequest request, bool skipBotDetection)
        {
            var logInfo= DeviceDetection.GetLogInfo(request, skipBotDetection);
            return logInfo.Marshal();
        }
        public static string Get()
        {
            LogInfo logInfo = new() 
            {
                Bot=new Bot(),
                Client=new Client(),
                Device=new Device(),
                Os = new Os()
            };
            return logInfo.Marshal();
        }
    }
}
