using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Tagging
{
    public interface ITagGroupReportQueryService
    {
        Task<ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto>> Get(TagsInputDto input);
        Task<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>> Get(TagsInputDto input, bool isZoneTitle);
    }
    internal sealed class TagGroupReportQueryService : AbstractBaseConnection, ITagGroupReportQueryService
    {
        public TagGroupReportQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto>> Get(TagsInputDto input)
        {
            string TagGroupQueryString = GetTagGroupQuery(input.TagIds.Any() == true);
            IEnumerable<TagGroupReportDetailDataOutputDto> tagGroupData = await _sqlReportConnection.QueryAsync<TagGroupReportDetailDataOutputDto>(TagGroupQueryString, new { TagGroupIds = input.TagIds });
            TagsHeaderOutputDto tagGroupHeader = new TagsHeaderOutputDto()
            {
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (tagGroupData is not null && tagGroupData.Any()) ? tagGroupData.Count() : 0,
                CustomerCount = (tagGroupData is not null && tagGroupData.Any()) ? tagGroupData.Count() : 0,
				Title= ReportLiterals.TagGroupDetail,
            };

            ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto> result = new ReportOutput<TagsHeaderOutputDto, TagGroupReportDetailDataOutputDto>(ReportLiterals.TagGroupDetail, tagGroupHeader, tagGroupData);
            return result;
        }

        public async Task<ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>> Get(TagsInputDto input, bool isZoneTitle)
        {
            string groupedParam = isZoneTitle ? "ZoneTitle" : "UsageTitle";
            string groupedFieldTitle = isZoneTitle ? ReportLiterals.ByZone : ReportLiterals.ByUsage;
			string reportTitle = ReportLiterals.TagGroupSummary + "-" + groupedFieldTitle;

            string TagGroupQueryString = GetTagGroupSummaryQuery(input.TagIds.Any() == true, groupedParam);
            IEnumerable<TagsReportSummaryDataOutputDto> tagGroupData = await _sqlReportConnection.QueryAsync<TagsReportSummaryDataOutputDto>(TagGroupQueryString, new { TagGroupIds = input.TagIds });
            TagsHeaderOutputDto tagGroupHeader = new TagsHeaderOutputDto()
            {
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (tagGroupData is not null && tagGroupData.Any()) ? tagGroupData.Count() : 0,
                CustomerCount = tagGroupData.Sum(r => r.CustomerCount),
                Count = tagGroupData.Sum(r => r.Count),
				Title=reportTitle,
            };

            ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto> result = new ReportOutput<TagsHeaderOutputDto, TagsReportSummaryDataOutputDto>(reportTitle, tagGroupHeader, tagGroupData);
            return result;
        }

        private string GetTagGroupQuery(bool hasTagGroup)
        {
            string queryTagGroups = hasTagGroup ? " AND tg.Id IN @TagGroupIds" : string.Empty;

            return @$"Select
						tg.Title as TagGroupTitle,
						tg.Id as TagGroupId,
						t.Title as TagTitle,
						t.Id as TagId,
						bt.BillId,
						bt.ExpireDateJalali as  BillIdTagsExpireDateJalali,
						t46.C2 as RegionTitle,
						c.zoneId,
						c.ZoneTitle,
						c.BillId,
						c.ReadingNumber,
						c.CustomerNumber,
						TRIM(c.FirstName) as FirstName,
						TRIM(c.SureName) as Surname,
						TRIM(c.FirstName)+TRIM(c.SureName) as FullName,
						TRIM(c.FatherName) as FatherName,
						TRIM(c.NationalId) as NationalCode,
						TRIM(c.PostalCode) as PostalCode,
						TRIM(c.PhoneNo) as PhoneNumber,
						TRIM(c.MobileNo) as MobileNumber,
						TRIM(c.Address) as Address,
						c.ContractCapacity as ContractualCapacity,
						c.CommercialCount as CommercialUnit,
						c.DomesticCount as DomesticUnit,
						c.OtherCount as OtherUnit,
						(c.CommercialCount + c.DomesticCount +c.OtherCount) as TotalUnit,
						c.EmptyCount as EmptyUnit,
						c.UsageTitle ,
						c.BranchType,
						c.WaterDiameterTitle as MeterDiameterTitle,
						c.MainSiphonTitle as SiphonDiameterTitle 
					From [CustomerWarehouse].dbo.TagGroups tg
					Join [CustomerWarehouse].dbo.Tags t
						On tg.Id=t.TagGroupId
					Join [CustomerWarehouse].dbo.BillIdTags bt
						On bt.TagId=t.Id
					Join [CustomerWarehouse].dbo.Clients c
						On c.BillId=bt.BillId collate SQL_Latin1_General_CP1_CI_AS	
					Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
					Where
						c.ToDayJalali IS NULL 
						{queryTagGroups}";
        }

        private string GetTagGroupSummaryQuery(bool hasTagGroup, string groupedParam)
        {
            string queryTagGroups = hasTagGroup ? " AND tg.Id IN @TagGroupIds" : string.Empty;

            return @$"
					Select
						tg.Title TagsTitle,
						c.{groupedParam} as ItemTitle,
						COUNT(c.ZoneTitle) as Count,
						COUNT(Distinct c.BillId) as CustomerCount
					From [CustomerWarehouse].dbo.TagGroups tg
					Join [CustomerWarehouse].dbo.Tags t
						On tg.Id=t.TagGroupId
					Join [CustomerWarehouse].dbo.BillIdTags bt
						On bt.TagId=t.Id
					Join [CustomerWarehouse].dbo.Clients c
						On c.BillId=bt.BillId collate SQL_Latin1_General_CP1_CI_AS	
					Where
						c.ToDayJalali IS NULL
						{queryTagGroups} 
					Group By c.{groupedParam},tg.Title";
        }
    }
}
