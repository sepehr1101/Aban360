using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface ITagQueryService
    {
        Task<IEnumerable<TagDto>> GetAll();
        Task<TagDto?> GetById(int id);
        Task<IEnumerable<TagsStringCodeValidateDto>> ValidateStringCodes(IEnumerable<string> stringCodes, IDbConnection connection, IDbTransaction transaction);
    }
}
