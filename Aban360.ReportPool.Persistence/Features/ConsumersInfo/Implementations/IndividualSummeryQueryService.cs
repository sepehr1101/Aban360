using Aban360.Common.Extensions;
using Aban360.ReportPool.Persistence.Base;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal class IndividualSummeryQueryService : AbstractBaseConnection, IIndividualSummeryQueryService
    {
        public IndividualSummeryQueryService(IConfiguration configuration)
            :base(configuration) 
        {
        }
        public async Task<IndividualSummaryDto> GetOwnerShipSummery(string billId,short relationTypeId)
        {
            string? estateQuery = GetIndividualOwnerShipQuery();
            IndividualSummaryDto? result = await _sqlConnection.QuerySingleAsync<IndividualSummaryDto>(estateQuery , new { billId = billId, relationTypeId =relationTypeId});
            
            return result;
        }
        public async Task<IndividualSummaryDto> GetStakeHolderSummery(string billId,short relationTypeId)
        {
            string? estateQuery = GetIndividualStakeHolderQuery();
            IndividualSummaryDto? result = await _sqlConnection.QuerySingleAsync<IndividualSummaryDto>(estateQuery , new { billId = billId, relationTypeId =relationTypeId});
            
            return result;
        }

        private string GetIndividualOwnerShipQuery()
        {
            return @"select
                        I.FullName,I.FatherName,I.NationalId,I.PhoneNumbers,I.MobileNumbers
                     from WaterMeter W
                     left join Estate E on W.EstateId=E.Id
                     left join IndividualEstate IE on E.Id=IE.EstateId
                     left join Individual I on IE.IndividualId=I.Id
                     where W.BillId=@billId and IE.IndividualEstateRelationTypeId=@relationTypeId";
        }
        private string GetIndividualStakeHolderQuery()
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
