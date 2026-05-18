using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.Common.ApplicationUser;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts
{
    public interface ITankerInsertHandler
    {
        Task<TankerCalculationResultOutputDto> Handle(TankerInsertInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
