using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public class ConsumerSummaryInfo : IConsumerSummaryInfo
    {
        private readonly IConfiguration _configuration;
        public ConsumerSummaryInfo(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.NotNull(nameof(configuration));
        }
        public async Task<ResultSummaryDto> GetSummery(string id)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);

            string getSummery = SummeryGetQuery();
            var summery = await connection.QuerySingleAsync<ResultSummaryDto>(getSummery, new { id = id });

            string getWaterMeterTag = WaterMeterTagGetByBillId();
            var tags = await connection.QueryAsync<string>(getWaterMeterTag, new { id = id });

            summery.WaterMeterTags = tags.ToList();
            return summery;
        }

        private string SummeryGetQuery()
        {
            return "select Distinct top 1 " +
                         "     W.CustomerNumber,W.BillId,W.ReadingNumber,W.InstallationDate,W.ProductDate,W.GuaranteeDate," +
                         "     E.Address,E.ContractualCapacity,E.HouseholdNumber," +
                         "     E.UnitDomesticWater,E.UnitCommercialWater, E.UnitOtherWater," +
                         "     E.UnitDomesticSewage,E.UnitCommercialSewage,E.UnitOtherSewage,E.EmptyUnit," +
                         "     C.Title as ConstructionType ," +
                         "     U.Title as UsageConsumtionTitle,UU.Title as UsageSellTitle," +
                         "     I.FullName," +
                         "     S.InstallationDate as SiphonInstallationDate, " +
                         "     H.Title as Headquarter , P.Title as Province,R.Title as Region,Z.Title as Zone, M.Title as Municipality " +
                         " from WaterMeter W " +
                         " left join Estate E on W.EstateId=E.Id" +
                         " left join IndividualEstate IE on E.Id=IE.EstateId" +
                         " left join Individual I on IE.IndividualId=I.Id" +
                         " left join ConstructionType C on E.ConstructionTypeId=C.Id" +
                         " left join Flat F on E.Id=F.EstateId" +
                         " left join Usage U on E.UsageConsumtionId=U.Id" +
                         " left join Usage UU on E.UsageSellId=UU.Id" +
                         " left join WaterMeterSiphon WS on W.Id=WS.WaterMeterId" +
                         " left join Siphon S on WS.SiphonId=S.Id" +
                         " left join Municipality M on E.MunipulityId=M.Id" +
                         " left join Zone Z on M.ZoneId=Z.Id" +
                         " left join Region R on Z.RegionId=R.Id" +
                         " left join Headquarters H on R.HeadquartersId=H.Id" +
                         " left join Province P on H.ProvinceId=P.Id" +
                         " where W.BillId=@id ";
        }

        private string WaterMeterTagGetByBillId()
        {
            return " select WTD.Title" +
                   " from WaterMeter W " +
                   " left join WaterMeterTag WT on W.Id=WT.WaterMeterId" +
                   " left join WaterMeterTagDefinition WTD on WT.WaterMeterTagDefinitionId=WTD.Id  " +
                   " where W.BillId=@id";
        }
    }
}