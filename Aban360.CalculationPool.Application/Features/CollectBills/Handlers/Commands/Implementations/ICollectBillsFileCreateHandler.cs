using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Commands.Implementations
{
    public interface ICollectBillsFileCreateHandler
    {
        Task<CollectBillsGetZipFileInfo> Handle(string? reportDateJalali, IAppUser appUser, CancellationToken cancellationToken);
    }
}
