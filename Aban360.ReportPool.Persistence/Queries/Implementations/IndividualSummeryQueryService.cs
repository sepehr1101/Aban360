using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class IndividualSummeryQueryService : IIndividualSummeryQueryService
    {
        private readonly IConfiguration _configuration;
        public IndividualSummeryQueryService(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.NotNull(nameof(configuration));
        }
        public async Task<IndividualSummaryDto> GetOwnerShipSummery(string billId,short relationTypeId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);
            string? estateQuery = IndividualOwnerShipGetQuery();
            IndividualSummaryDto? result = await connection.QuerySingleAsync<IndividualSummaryDto>(estateQuery , new { billId = billId, relationTypeId =relationTypeId});
            
            return result;
        }
        public async Task<IndividualSummaryDto> GetStakeHolderSummery(string billId,short relationTypeId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);
            string? estateQuery = IndividualStakeHolderGetQuery();
            IndividualSummaryDto? result = await connection.QuerySingleAsync<IndividualSummaryDto>(estateQuery , new { billId = billId, relationTypeId =relationTypeId});
            
            return result;
        }

        private string IndividualOwnerShipGetQuery()
        {
            return @"select
                        I.FullName,I.FatherName,I.NationalId,I.PhoneNumbers,I.MobileNumbers
                     from WaterMeter W
                     left join Estate E on W.EstateId=E.Id
                     left join IndividualEstate IE on E.Id=IE.EstateId
                     left join Individual I on IE.IndividualId=I.Id
                     where W.BillId=@billId and IE.IndividualEstateRelationTypeId=@relationTypeId";
        }
        private string IndividualStakeHolderGetQuery()
        {
            return @"select
                        I.FullName,I.FatherName,I.NationalId,I.PhoneNumbers,I.MobileNumbers
                     from WaterMeter W
                     left join Estate E on W.EstateId=E.Id
                     left join IndividualEstate IE on E.Id=IE.EstateId
                     left join Individual I on IE.IndividualId=I.Id
                     where W.BillId=@billId and IE.IndividualEstateRelationTypeId!=@relationTypeId";
        }
    }
}
