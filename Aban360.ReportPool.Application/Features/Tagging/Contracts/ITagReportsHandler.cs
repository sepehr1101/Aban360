using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Contracts
{
    public interface ITagReportsHandler
    {
        Task<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>> Handle(TagsInputDto inputDto, bool isZoneTitle, CancellationToken cancellationToken);
    }
}