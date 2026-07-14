using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts
{
    public interface IMeterSmsStateTemplateQueryService
    {
        Task<MeterSmsStateTemplateGetDto> Get(short id);
        Task<IEnumerable<MeterSmsStateTemplateGetDto>> Get();
    }
}
