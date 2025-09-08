using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;

namespace Aban360.OldCalcPool.Persistence.Features.Rules.Commands.Contracts
{
    public interface INerkhCreateService
    {
        Task Create(NerkhCreateDto input,int switchNerkh);
        Task Create(NerkhCreateDto input);
    }
}
