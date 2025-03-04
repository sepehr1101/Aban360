using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Commands.Update.Contracts
{
    public interface ICompanyServiceTypeUpdateHandler
    {
        Task Handle(CompanyServiceTypeUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
