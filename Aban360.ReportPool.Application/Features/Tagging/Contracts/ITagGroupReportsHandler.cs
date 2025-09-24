using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.Tagging;

namespace Aban360.ReportPool.Application.Features.Tagging.Contracts
{
    public interface ITagGroupReportsHandler
    {
        Task<ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto>> Handle(TagsInputDto inputDto, CancellationToken cancellationToken);
        Task<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>> Handle(TagsInputDto inputDto, bool isZoneTitle, CancellationToken cancellationToken);
    }
}