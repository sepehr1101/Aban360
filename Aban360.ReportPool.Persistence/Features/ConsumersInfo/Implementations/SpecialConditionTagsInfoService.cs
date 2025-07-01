using Aban360.Common.Db.Exceptions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class SpecialConditionTagsInfoService : AbstractBaseConnection, ISpecialConditionTagsInfoService
    {
        public SpecialConditionTagsInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<SpecialConditionTagsInfoDto> GetInfo(string billId)
        {
            //string branchHistoryQuery = GetSpecialConditionTagsSummayDtoQuery();
            string branchHistoryQuery = GetSpecialConditionTagsSummaryDtoWithClientDbQuery();
            string waterMeterTagsQuery = GetWaterMeterTagsDtoQuery();
            string individualTagsQuery = GetIndividualTagsDtoQuery();

            SpecialConditionTagsInfoDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<SpecialConditionTagsInfoDto>(branchHistoryQuery, new { billId });
            if (result == null)
                throw new InvalidIdException();

            // result.tagsInfoDtos = await _sqlReportConnection.QueryAsync<TagsInfoDto>(waterMeterTagsQuery, new { billId });
            // result.tagsInfoDtos.Concat(await _sqlReportConnection.QueryAsync<TagsInfoDto>(individualTagsQuery,new { billId }));
            result.tagsInfoDtos=new List<TagsInfoDto>();

            return result;
        }
        private string GetSpecialConditionTagsSummaryDtoWithClientDbQuery()
        {
            return @"select 
                     	c.BranchType AS HandoverTitle,
                     	c.DiscountTypeTitle AS DiscountTypeTitle, --todo:
                     	c.DeletionStateTitle AS UsageStateTitle ,
                     	c.IsSpecial AS SpecialBranch,
                     	c.EmptyCount AS EmptyUnit,
                     	c.FamilyCount AS HouseholderNumber,
                     	0 AS NonSequentialFlag
                     
                     from [CustomerWarehouse].dbo.Clients c
                     where
						c.BillId=@billId AND
						c.ToDayJalali is null";

        }
         private string GetSpecialConditionTagsSummayDtoQuery()
        {
            return @"select top(1)
                    	h.Title as 'HandoverTitle',--without relation
                    	dt.Title as 'DiscountTypeTitle',
                    	us.Title as 'UsageStateTitle',
                    	0 as 'SpecialBranch',
                    	e.EmptyUnit as 'EmptyUnit',
                    	e.HouseholdNumber as 'HouseholderNumber',
                    	1 as 'NonSequentialFlag'
                    from ClaimPool.WaterMeter w 
					join ClaimPool.Estate e on w.EstateId=e.Id
					join ClaimPool.Handover h on e.HandoverId=h.Id
					join ClaimPool.IndividualEstate ie on ie.EstateId=e.Id
					join ClaimPool.Individual i on ie.IndividualId=i.Id
					join ClaimPool.IndividualDiscountType idt on idt.IndividualId=i.Id
					join ClaimPool.DiscountType dt on idt.DiscountTypeId=dt.Id
                    join ClaimPool.UseState us on w.UseStateId=us.Id
                         where w.BillId=@billId";

        }
        private string GetWaterMeterTagsDtoQuery()
        {
            return @"select
                    	wtd.Id,
                    	wtd.Title
                    from ClaimPool.WaterMeter w 
                    join ClaimPool.WaterMeterTag wt on w.Id=wt.WaterMeterId
                    join ClaimPool.WaterMeterTagDefinition wtd on wtd.Id=wt.WaterMeterTagDefinitionId
                         where w.BillId=@billId";
        
        }
        private string GetIndividualTagsDtoQuery()
        {
            return @"select
                    	itd.Id,
						itd.Title
                    from ClaimPool.WaterMeter w 
					join ClaimPool.Estate e on w.EstateId=e.Id
					join ClaimPool.IndividualEstate ie on e.Id=ie.EstateId
					join ClaimPool.Individual i on ie.IndividualId=i.Id
					join ClaimPool.IndividualTag it on it.IndividualId=i.Id
					join ClaimPool.IndividualTagDefinition itd on it.IndividualTagDefinitionId=itd.Id
                         where w.BillId=@billId";
        }
    }
}
