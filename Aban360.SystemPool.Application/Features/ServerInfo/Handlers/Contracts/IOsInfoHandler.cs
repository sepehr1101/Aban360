using Aban360.SystemPool.Domain.Features.ServerInfo;

namespace Aban360.SystemPool.Application.Features.ServerInfo.Handlers.Contracts
{
    public interface IOsInfoHandler
    {
        OsInfo Handle();
    }
}