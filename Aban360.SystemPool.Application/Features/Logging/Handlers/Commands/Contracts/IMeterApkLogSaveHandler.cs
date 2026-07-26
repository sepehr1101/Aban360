using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;

namespace Aban360.SystemPool.Application.Features.Logging.Handlers.Commands.Contracts
{
    public interface IMeterApkLogSaveHandler
    {
        Task Handle(MeterApkLogInsertDto input, CancellationToken cancellationToken);
    }
}
