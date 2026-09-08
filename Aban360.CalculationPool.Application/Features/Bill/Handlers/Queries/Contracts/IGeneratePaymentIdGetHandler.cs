using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts
{
    public interface IGeneratePaymentIdGetHandler
    {
        Task<string> Handle(GeneratePaymentIdInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
