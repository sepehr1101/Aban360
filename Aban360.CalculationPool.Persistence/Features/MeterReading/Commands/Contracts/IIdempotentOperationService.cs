using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Contracts
{
    public interface IIdempotentOperationService
    {
        Task<IdempotentOperationResultDto> TryBegin(string operationKey, Guid lockToken);
        Task Complete(string operationKey, Guid lockToken, string responseJson, IDbConnection connection, IDbTransaction transaction);
        Task Fail(string operationKey, Guid lockToken);
    }
}
