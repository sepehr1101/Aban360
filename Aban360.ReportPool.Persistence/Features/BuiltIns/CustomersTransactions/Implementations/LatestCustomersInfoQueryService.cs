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
    internal sealed class LatestCustomersInfoQueryService : AbstractBaseConnection, ILatestCustomersInfoQueryService
    {
        public LatestCustomersInfoQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<LatestCustomersInfoHeaderOutputDto, LatestCustomersInfoDataOutputDto>> Get(LatestCustomersInfoInputDto input)
        {
            string title = ReportLiterals.LatestCustomersInfo;
            string query = GetQuery();
            IEnumerable<LatestCustomersInfoDataOutputDto> data=await _sqlReportConnection.QueryAsync<LatestCustomersInfoDataOutputDto>(query, input);
            LatestCustomersInfoHeaderOutputDto header = new()
            {
                RecordCount=data?.Count() ?? 0, 
                Title=title
            };

            ReportOutput<LatestCustomersInfoHeaderOutputDto, LatestCustomersInfoDataOutputDto> result=new(title,header,data);
            return result;
        }
        private string GetQuery()
        {
            return @";With LatestBills As(
						Select 
							b.ZoneId,
							b.CustomerNumber,
							b.BillId,
							b.NextNumber LatestMeterNumber,
							b.RegisterDay RegisterDayJalali,
							Rn=ROW_NUMBER() OVER(Partition By b.ZoneId,b.CustomerNumber Order By b.RegisterDay Desc)
						From CustomerWarehouse.dbo.Bills b
						Where 
							b.ZoneId IN @ZoneIds AND 
							b.CounterStateCode NOT IN(4,7,8)
					)
					Select top 10 
						t46.C0 RegionId,
						t46.C2 RegionTitle,
						t51.C0 ZoneId,
						t51.C2 ZoneTitle,
						c.CustomerNumber ,
						c.ReadingNumber,
						c.BillId,
						c.FirstName,
						c.SureName Surname,
						c.FirstName + ' ' + c.SureName FullName,
						c.NationalId NationalCode,
						c.PhoneNo PhoneNumber,
						c.MobileNo MobileNumber,
						c.PostalCode ,
						c.Address,
						c.UsageId,
						t41.C1 UsageTitle,
						c.UsageId BranchTypeId,
						t7.C1 BranchTypeTitle,
						c.DomesticCount DomesticUnit,
						c.CommercialCount CommercialUnit,
						c.OtherCount OtherUnit,
						c.FieldArea Premises,
						c.CommercialArea ImprovementsCommercial,
						c.DomesticArea ImprovementsDomestic,
						c.ConstructedArea ImprovementsOverall,
						c.ContractCapacity ContractualCapacity,
						c.WaterRequestDate MeterRequestDateJalali,
						c.PhysicalWaterInstallDateJalali MeterInstallationDateJalali,
						c.SewageRequestDate SewageRequestDateJalali,
						c.PhysicalSewageInstallDateJalali SewageInstallationDateJalali,
						b.LatestMeterNumber ,
						b.RegisterDayJalali LatestReadingDateJalali,
						wd.Debt WaterRemained,
						ISNULL(va.BedehiAll ,0) SubscriptionRemained
					From CustomerWarehouse.dbo.Clients c
					Left Join LatestBills b
						ON c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
					Left Join CustomerWarehouse.dbo.WaterDebt wd
						ON c.BillId Collate Arabic_CI_AS=wd.BillId 
					Left Join CustomerWarehouse.dbo.VosoolEnsheabAlert va
						ON c.ZoneId=va.ZoneId AND c.CustomerNumber=va.Radif
					Left Join [Db70].dbo.T51 t51
						ON c.ZoneId=t51.C0
					Left Join [Db70].dbo.T46 t46
						ON t51.C1=t46.C0
					Left Join [Db70].dbo.T41 t41
						ON c.UsageId=t41.C0
					Left Join [Db70].dbo.T7 t7
						ON c.UsageId=t7.C0
					Where 
						c.ZoneId IN @ZoneIds AND
						c.ToDayJalali IS NULL AND
						b.Rn=1";
        }
    }
}
