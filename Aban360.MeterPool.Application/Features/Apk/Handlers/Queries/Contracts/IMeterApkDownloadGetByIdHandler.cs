namespace Aban360.MeterPool.Application.Features.Apk.Handlers.Queries.Contracts
{
    public interface IMeterApkDownloadGetByIdHandler
    {
        Task<byte[]> Handle(short id, CancellationToken cancellationToken);
    }
}
