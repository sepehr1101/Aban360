using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface ITagQueryService
    {
        Task<IEnumerable<TagDto>> GetAll();
        Task<IEnumerable<TagDto>> GetByTagGroupIds(IEnumerable<int> tagGroupIds);
        Task<TagDto?> GetById(int id);
        Task<TagDto?> GetByStringCode(string stringCode);
        Task<IEnumerable<TagsStringCodeValidateDto>> ValidateStringCodes(IEnumerable<string> stringCodes, IDbConnection connection, IDbTransaction transaction);
    }
}
