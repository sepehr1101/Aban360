using Aban360.Common.Db.Exceptions;
using Aban360.ReportPool.Domain.Features.Geo;
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
            //string individualsQuery = GetIndividualsSummayDtoQuery();
            string individualsQuery = GetIndividualsSummaryDtoWithClientDbQuery();
            LocationInfoDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<LocationInfoDto>(individualsQuery, new { billId });
            if (result==null)
                throw new InvalidIdException(); 

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
        private string GetIndividualsSummaryDtoWithClientDbQuery()
        {
            return @"select 
                    	c.ReadingNumber,
                    	c.PostalCode,
                    	c.X, 
                    	c.Y,
                    	'' AS EvaluatorSpecifications,
                    	c.Address 
                    from [CustomerWarehouse].dbo.Clients c
                    where c.BillId=@billId
                    and c.ToDayJalali is null";
        }
    }
}
