using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts
{
    public interface ISmsStateTemplateQueryService
    {
        Task<SmsStateTemplateGetDto> Get(short id);
        Task<SmsStateTemplateGetDto> GetFirst(int groupId, int typeId);
        Task<SmsStateTemplateGetDto?> GetNextStep(int id, bool hasException);
        Task<IEnumerable<SmsStateTemplateGetDto>> Get(bool isValid);
        Task<SmsStateTemplateGetDto?> GetNextTemplate(int firstFlowId, int groupId);
    }
}
