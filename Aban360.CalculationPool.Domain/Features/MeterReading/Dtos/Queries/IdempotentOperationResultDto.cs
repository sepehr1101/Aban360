using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public sealed record IdempotentOperationResultDto
    {
        public bool Acquired { get; init; }
        public IdempotentOperationStatusEnum Status { get; init; }
        public string? ResponseJson { get; init; }
        public Guid? LockToken { get; init; }
    }
}
