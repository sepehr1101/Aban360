using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Delete.Contracts
{
    public interface ICompanyServiceTypeDeleteHandler
    {
        Task Handle(CompanyServiceTypeDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
