using Aban360.ReportPool.Domain.Features.FlatReports.Dto.Commands;

namespace Aban360.ReportPool.Persistence.Features.FlatReports.Commands.Contracts
{
    public interface IServerReportsCreateService
    {
        void Create(ServerReportsCreateDto input);
    }
}