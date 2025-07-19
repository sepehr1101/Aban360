using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;

namespace Aban360.ReportPool.Application.Features.FlatReports.Handler.Commands.Contracts
{
    public interface IServerReportsCreateHandler
    {
        void Handle(ServerReportsCreateDto createDto,CancellationToken cancellationToken);
    }
}
