using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts
{
    public interface IArticle11CreateHadler
    {
        Task Handle(Article11InputDto inputDto, CancellationToken cancellationToken);
    }
}
