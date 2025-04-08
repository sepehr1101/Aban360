using Aban360.BlobPool.Domain.Features.Taxonomy;
namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts
{
    public interface IExecuteableMimetypeCommandService
    {
        Task Add(ExecutableMimetype executableMimetype);
        Task Add(ICollection<ExecutableMimetype> executableMimetypes);
        void AddSync(ExecutableMimetype executableMimetype);
        void AddSync(ICollection<ExecutableMimetype> executableMimetypes);
        void Remove(ExecutableMimetype executableMimetype);
        void Remove(ICollection<ExecutableMimetype> executableMimetypes);
    }
}