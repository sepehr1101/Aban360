using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class ClientGuildQueryService : AbstractBaseConnection, IClientGuildQueryService
    {
        public ClientGuildQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ClientGuildDetailHeaderOutputDto, ClientGuildDetailDataOutputDto>> GetDetail(ClientGuildInputDto input)
        {
            string title = ReportLiterals.ClientGuildDetail;
            string query = GetDetailQuery();
            IEnumerable<ClientGuildDetailDataOutputDto> data = await _sqlReportConnection.QueryAsync<ClientGuildDetailDataOutputDto>(query, input);
            ClientGuildDetailHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                CustomerCount = data?.Count() ?? 0,
                RecordCount = data?.Count() ?? 0,
                Title =title
            };

            return new ReportOutput<ClientGuildDetailHeaderOutputDto, ClientGuildDetailDataOutputDto>(title, header, data);
        }
        public async Task<ReportOutput<ClientGuildSummaryHeaderOutputDto, ClientGuildSummaryDataOutputDto>> GetSummary(ClientGuildInputDto input)
        {
            string title = ReportLiterals.ClientGuildSummary;
            string query = GetSummaryQuery();
            IEnumerable<ClientGuildSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<ClientGuildSummaryDataOutputDto>(query, input);
            ClientGuildSummaryHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                CustomerCount = data?.Sum(g=>g.CustomerCount) ?? 0,
                RecordCount = data?.Count() ?? 0,
                Title = title
            };

            return new ReportOutput<ClientGuildSummaryHeaderOutputDto, ClientGuildSummaryDataOutputDto>(title, header, data);
        }

        private string GetDetailQuery()
        {
            return $@";WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
		                    c.ZoneId IN @zoneIds AND
		                    c.CustomerNumber<>0 AND
		                    c.RegisterDayJalali <= @ToDateJalali
                    )
                    Select	
	                    c.CustomerNumber, 
                    	c.ReadingNumber,
                        c.BlockCode,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) AS Surname,
                    	TRIM(c.Address) AS Address,
                    	c.UsageTitle AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                        IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount)) AS TotalUnit,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.WaterRequestDate AS RequestDateJalali,
                    	c.PhysicalWaterInstallDateJalali AS InstallationDateJalali,
                        c.WaterRegisterDateJalali AS RegisterDateJalali
                    FROM CTE c
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    WHERE	  
                        c.RN=1 AND
		                c.ZoneId IN @zoneIds AND
		                c.RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
	                    c.DeletionStateId NOT IN(1,2)";
        }
        private string GetSummaryQuery()
        {
            return $@";WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
		                    c.ZoneId IN @zoneIds AND
		                    c.CustomerNumber<>0 AND
		                    c.RegisterDayJalali <= @ToDateJalali
                    )
                    Select	
	                   MAX(c.ZoneId) ZoneId,
					   c.ZoneTitle,
					   MAX(c.UsageId) UsageId,
					   c.UsageTitle,
					   COUNT(1) CustomerCount
                    FROM CTE c
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    WHERE	  
                        c.RN=1 AND
		                c.ZoneId IN @zoneIds AND
		                c.RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
	                    c.DeletionStateId NOT IN(1,2)
					GROUP BY c.ZoneTitle,c.UsageTitle
					ORDER BY c.ZoneTitle,c.UsageTitle";
        }
    }
}
