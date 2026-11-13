// Test-only contract for unchanged pre-refactor snapshots.
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts
{
    public interface ICompanyServiceTypeDeleteHandler
    {
        Task Handle(CompanyServiceTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
