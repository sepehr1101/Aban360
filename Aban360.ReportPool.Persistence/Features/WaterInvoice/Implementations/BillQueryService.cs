using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.WaterInvoice.Implementations
{
    internal sealed class BillQueryService : AbstractBaseConnection, IBillQueryService
    {
        public BillQueryService(IConfiguration configuration)
            :base(configuration)
        {
        }

        public async Task<BillItemsGetDto> Get(int id)
        {
			string query = GetItemsByBillIdQuery();
            BillItemsGetDto? data = await _sqlReportConnection.QueryFirstOrDefaultAsync<BillItemsGetDto>(query, new { id });
			if (data == null)
			{
				throw new InvalidBillIdException(ExceptionLiterals.InvoiceNotFound);
			}
			return data;
        }
        private string GetItemsByBillIdQuery()
        {
            return $@"Select 
						Id,
						BillId,
						CustomerNumber,
						t46.C0 RegionId,
						t46.C2 RegionTitle,
						ZoneId,
						ZoneTitle,
						UsageId,
						UsageTitle,
						BranchTypeId,
						BranchType BranchTypeTitle,
						Item1 AbBaha,
						Item2 FazelabBaha,
						Item3 AbonmanAb,
						Item4 AbonmanFazelab,
						Item5 Maliat,
						Item6 Tabsare2,
						Item7 Tabsare2_3,
						Item8 Jarime,
						Item9 Abresani,
						Item10  JavaniJamiat,
						Item11 FaslGarm,
						Item12 ZaribTadil,
						Item13 Tabsare3Ab,
						Item14 Tabsare3Fazelab,
						Item15 TabsareAbonmanFazelab,
						Item16 GhanonBoodje,
						Item17 JavazemKahande,
						Item18 Boodje
					From CustomerWarehouse.dbo.Bills
					Left Join [Db70].dbo.T51 t51
						On ZoneId=t51.C0
					Left Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
					Where Id=@id";
        }
    }
}
