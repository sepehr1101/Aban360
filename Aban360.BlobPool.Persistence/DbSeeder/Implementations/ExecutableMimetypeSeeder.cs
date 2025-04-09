using Aban360.BlobPool.Domain.Features.Taxonomy.Entities;
using Aban360.BlobPool.Persistence.Contexts.Contracts;
using Aban360.Common.Db.DbSeeder.Contracts;
using Aban360.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aban360.BlobPool.Persistence.DbSeeder.Implementations
{
    public class ExecutableMimetypeSeeder : IDataSeeder
    {
        public int Order { get; set; } = 10;
        private readonly IUnitOfwork _uow;
        private readonly DbSet<ExecutableMimetype> _executableMimetypes;
        public ExecutableMimetypeSeeder(IUnitOfwork uow)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _executableMimetypes = _uow.Set<ExecutableMimetype>();
            _executableMimetypes.NotNull(nameof(_executableMimetypes));
        }


        public void SeedData()
        {
            if (_executableMimetypes.Any())
            {
                return;
            }

            ICollection<ExecutableMimetype> executableMimetypes = new List<ExecutableMimetype>()
            {
                new ExecutableMimetype(){Id=1,Title="Image", StreamingOption=false ,FrontendExecutor=false},
                new ExecutableMimetype(){Id=2,Title="Video", StreamingOption=true ,FrontendExecutor=false},
                new ExecutableMimetype(){Id=3,Title="Text", StreamingOption=false ,FrontendExecutor=false},
                new ExecutableMimetype(){Id=4,Title="pdf", StreamingOption=false ,FrontendExecutor=false},
            };
            _executableMimetypes.AddRange(executableMimetypes);
            _uow.SaveChanges();

        }
    }
}
