using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Tagging;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Tagging
{
    public interface ITagReportQueryService
    {
        Task<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>> Get(TagsInputDto input, bool isZoneTitle);
    }
    internal sealed class TagReportQueryService : AbstractBaseConnection, ITagReportQueryService
    {
        public TagReportQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>> Get(TagsInputDto input, bool isZoneTitle)
        {
            string groupedParam = isZoneTitle ? "ZoneTitle" : "UsageTitle";
            string summaryTitle = isZoneTitle ? ReportLiterals.ByZone : ReportLiterals.ByUsage;
            string reportTitle = ReportLiterals.TagSummary + "-" + summaryTitle;

            string TagQueryString = GetTagSummaryQuery(input.TagIds.Any() == true, groupedParam);
            IEnumerable<TagsReportSummaryDataOutputDto> tagData = await _sqlReportConnection.QueryAsync<TagsReportSummaryDataOutputDto>(TagQueryString, new { TagIds = input.TagIds });
            TagsHeaderOutputDto tagHeader = new TagsHeaderOutputDto()
            {
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (tagData is not null && tagData.Any()) ? tagData.Count() : 0,
                CustomerCount = tagData.Sum(r => r.CustomerCount),
                Count = tagData.Sum(r => r.Count),
                Title = reportTitle
            };

            ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto> result = new ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>(reportTitle, tagHeader, tagData);
            return result;
        }


        private string GetTagSummaryQuery(bool hasTagGroup, string groupedParam)
        {
            string queryTag = hasTagGroup ? " AND t.Id IN @TagIds" : string.Empty;

            return @$"
					Select
                    	t.Title TagsTitle,
                    	c.UsageTitle as ItemTitle,
                    	COUNT(c.ZoneTitle) as Count,
                    	COUNT(Distinct c.BillId) as CustomerCount
                    From [CustomerWarehouse].dbo.Tags t
                    Join [CustomerWarehouse].dbo.BillIdTags bt
                    	On bt.TagId=t.Id
                    Join [CustomerWarehouse].dbo.Clients c
                    	On c.BillId=bt.BillId collate SQL_Latin1_General_CP1_CI_AS	
                    Where
                    	c.ToDayJalali IS NULL 
                        {queryTag}
                    Group By c.UsageTitle,t.Title";
        }
    }
}
