using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts
{
    public interface IInvalidPaymentIdGetHandler
    {
        Task<ReportOutput<InvalidPaymentIdHeaderOutputDto, InvalidPaymentIdDataOutputDto>> Handle(InvalidPaymentIdInputDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
