using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface ITagReportQueryService
    {
        Task<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>> Get(TagsInputDto input, bool isZoneTitle);
    }
}
