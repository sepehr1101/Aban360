using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts
{
    public interface ICheckedListGetHandler
    {
        Task<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCheckedDto>> Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken);
    }
}
