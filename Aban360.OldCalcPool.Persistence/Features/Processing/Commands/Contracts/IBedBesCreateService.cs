using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Microsoft.Data.SqlClient;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts
{
    public interface IBedBesCreateService
    {
        Task Create(BedBesCreateDto input);
        Task Create(ICollection<BedBesCreateDto> input);
        Task Create(ICollection<BedBesCreateDto> input, int zoneId);
    }
}
