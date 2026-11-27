using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
namespace Aban360.BlobPool.Persistence.Features.Taxonomy.Commands.Contracts
{
    public interface IExecutableMimetypeCommandService
    {
        Task Add(ExecutableMimetype executableMimetype);
        Task Add(ICollection<ExecutableMimetype> executableMimetypes);
        void AddSync(ExecutableMimetype executableMimetype);
        void AddSync(ICollection<ExecutableMimetype> executableMimetypes);
        void Remove(ExecutableMimetype executableMimetype);
        void Remove(ICollection<ExecutableMimetype> executableMimetypes);
    }
}