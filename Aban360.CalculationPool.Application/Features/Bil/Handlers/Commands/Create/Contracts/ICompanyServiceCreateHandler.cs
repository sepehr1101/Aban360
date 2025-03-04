using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Create.Contracts
{
    public interface ICompanyServiceCreateHandler
    {
        Task Handle(CompanyServiceCreateDto createDto, CancellationToken cancellationToken);
    }
}
