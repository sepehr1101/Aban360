using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class InstallationPrintQueryService : AbstractBaseConnection, IInstallationPrintQueryService
    {
        public InstallationPrintQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<FlatReportOutput<InstallationPrintHeaderOutputDto, InstallationPrintDataOutputDto>> Get(ZoneIdAndCustomerNumber input)
        {
            string title = ReportLiterals.InstallmentPrint;
            string query = GetQuery(GetDbName(input.ZoneId));
            InstallationPrintDataOutputDto data = await _sqlReportConnection.QueryFirstOrDefaultAsync<InstallationPrintDataOutputDto>(query, input);
            InstallationPrintHeaderOutputDto header = new()
            {
                Title = title
            };
            return new FlatReportOutput<InstallationPrintHeaderOutputDto, InstallationPrintDataOutputDto>(title,header,data);
        }
        private string GetQuery(string dbName)
        {
            return $@"Select 
						t51.C0 ZoneId,
						t51.C2 ZoneTitle,
						t46.C0 RegionId,
						t46.C2 RegionTitle,
						t7.C0 BranchTypeId,
						t7.C1 BranchTypeTitle,
						t5.C0 MeterDiameterId,
						t5.C2 MeterDiamterTitle,
						t41.C0 UsageId,
						t41.C1 UsageTitle,
						m.radif CustomerNumber,
						m.town ZoneId,
						m.bill_id BillId,
						m.radif CustomerNumber,
						TRIM(m.name) FirstName,
						TRIM(m.family) Surname,
						(TRIM(m.name)+' '+TRIM(m.family)) as FullName,
						TRIM(m.address) as Address,
						TRIM(m.MOBILE) as MobileNumber,
						TRIM(m.POST_COD) PostalCode,
						TRIM(m.eshtrak) as ReadingNumber,
						m.fix_mas as ContractualCapacity,
						m.ted_khane as HouseholdNumber,
						m.Khali_s  EmptyUnit,
						m.tedad_mas DomesticUnit,
						m.tedad_tej CommercialUnit,
						m.tedad_vahd OtherUnit,
						m.arse Premises,
						m.aian_mas ImprovementsDomestic ,
						m.aian ImprovementsOverall,
						m.aian_tej ImprovementsCommercial 
					From [{dbName}].dbo.members m
					Left Join [Db70].dbo.T51 t51
						ON m.Town=t51.c0
					Left Join [Db70].dbo.T46 t46
						ON t51.C1=t46.C0 
					Left Join [Db70].dbo.T7 t7
						ON m.noe_va=t7.C0
					Left Join [Db70].dbo.T5 t5
						ON m.enshab=t5.C0
					Left Join [Db70].dbo.T41 t41
						ON m.cod_enshab=t41.C0
					Where
						m.town=@ZoneId AND
						m.radif=@CustomerNumber";
        }
    }
}
