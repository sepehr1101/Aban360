using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts
{
    public interface ILatestBillSendSmsHandler
    {
        Task Handle(string billId, string? mobileNumber, IAppUser appUser, CancellationToken cancellationToken);
    }
}
