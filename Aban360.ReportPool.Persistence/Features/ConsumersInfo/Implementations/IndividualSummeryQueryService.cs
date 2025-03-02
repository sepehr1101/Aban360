using Aban360.ReportPool.Persistence.Base;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    internal sealed class IndividualSummeryQueryService : AbstractBaseConnection, IIndividualSummeryQueryService
    {
        public IndividualSummeryQueryService(IConfiguration configuration)
            :base(configuration) 
        {
        }
        public async Task<IEnumerable<IndividualSummaryDto>> GetOwnerShipSummery(string billId,short relationTypeId)
        {
            string estateQuery = GetIndividualOwnerShipQuery();
            IEnumerable<IndividualSummaryDto> result = await _sqlConnection.QueryAsync<IndividualSummaryDto>(estateQuery , new { billId = billId, relationTypeId =relationTypeId});
            
            return result;
        }
        public async Task<IEnumerable<IndividualSummaryDto>> GetStakeHolderSummery(string billId,short relationTypeId)
        {
            string estateQuery = GetIndividualStakeHolderQuery();
            IEnumerable<IndividualSummaryDto> result = await _sqlConnection.QueryAsync<IndividualSummaryDto>(estateQuery , new { billId = billId, relationTypeId =relationTypeId});
            
            return result;
        }

        private string GetIndividualOwnerShipQuery()
        {
            return @"SELECT
                        I.Id, I.FullName,I.FatherName,I.NationalId,I.PhoneNumbers,I.MobileNumbers
                     from WaterMeter W
                     join Estate E on W.EstateId=E.Id
                     join IndividualEstate IE on E.Id=IE.EstateId
                     join Individual I on IE.IndividualId=I.Id
                     where W.BillId=@billId and IE.IndividualEstateRelationTypeId=@relationTypeId";
        }
        private string GetIndividualStakeHolderQuery()
        {
            return @"SELECT
                        I.Id, I.FullName,I.FatherName,I.NationalId,I.PhoneNumbers,I.MobileNumbers
                     from WaterMeter W
                     left join Estate E on W.EstateId=E.Id
                     left join IndividualEstate IE on E.Id=IE.EstateId
                     left join Individual I on IE.IndividualId=I.Id
                     where W.BillId=@billId and IE.IndividualEstateRelationTypeId <> @relationTypeId";
        }
    }
}
