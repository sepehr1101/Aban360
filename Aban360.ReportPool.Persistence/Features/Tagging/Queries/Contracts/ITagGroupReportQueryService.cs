using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts
{
    public interface ITagGroupReportQueryService
    {
        Task<ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto>> Get(TagsInputDto input);
        Task<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>> Get(TagsInputDto input, bool isZoneTitle);
    }
}
