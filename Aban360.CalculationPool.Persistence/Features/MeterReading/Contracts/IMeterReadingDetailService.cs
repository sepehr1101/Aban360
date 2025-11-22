using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts
{
    public interface IMeterReadingDetailService
    {
        Task Create(IEnumerable<MeterReadingDetailCreateDto> input);
    }
}
