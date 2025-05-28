using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class LocationInfoService : AbstractBaseConnection, ILocationInfoService
    {
        public LocationInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<LocationInfoDto> GetInfo(string billId)
        {
            string individualsQuery = GetIndividualsSummayDtoQuery();
            LocationInfoDto result = await _sqlConnection.QueryFirstOrDefaultAsync<LocationInfoDto>(individualsQuery, new { billId });

            return result;
        }
        private string GetIndividualsSummayDtoQuery()
        {
            return @"select 
                    	w.ReadingNumber as 'AccountNumber',--
                     	e.PostalCode,
                     	e.X,
                     	e.Y,
                    	N'---' as 'EvaluatorSpecifications',
                     	e.Address
                     from ClaimPool.WaterMeter w
                     join ClaimPool.Estate e on w.EstateId=e.Id
                         where w.BillId=@billId";

        }
    }
}
