namespace Aban360.MeterPool.Application.Features.Apk.Queries.Contracts
{
    public interface IMeterApkDownloadGetByIdHandler
    {
        Task<byte[]> Handle(short id, CancellationToken cancellationToken);
    }
}
