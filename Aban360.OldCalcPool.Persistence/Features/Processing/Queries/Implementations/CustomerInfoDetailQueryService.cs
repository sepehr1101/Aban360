using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Implementations
{
    internal sealed class CustomerInfoDetailQueryService : AbstractBaseConnection, ICustomerInfoDetailQueryService
    {
        public CustomerInfoDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<CustomerInfoOutputDto> GetInfo(string billId)
        {
            string zoneIdQueryString = GetZoneIdQuery();
            int zoneId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(zoneIdQueryString, new { billId });
            if (zoneId == null)
            {
                throw new BaseException(ExceptionLiterals.BillIdNotFound);
            }
            string customerInfoQueryString = GetCustomerInfoDataQuery(zoneId);
            CustomerInfoOutputDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerInfoOutputDto>(customerInfoQueryString, new { billId });

            return result;
        }

        private string GetCustomerInfoDataQuery(int zoneId)
        {
            return @$"Select
                    	m.town as ZoneId,
                    	m.radif as Radif,
                    	m.noe_va as BranchType,
                    	m.cod_enshab as UsageId,
                    	m.tedad_mas as DomesticUnit,
                    	m.tedad_tej as CommertialUnit,
                    	m.tedad_vahd as OtherUnit,
                    	m.inst_ab as WaterInstallationDateJalali,
                    	m.inst_fas as SewageInstallationDateJalali,
                    	m.n_ab as WaterCount,
                    	m.n_faz as SewageCount,
						m.fix_mas as ContractualCapacity
                    From [{zoneId}].dbo.members m
                    Where
                    	m.bill_id=@billId";
        }

        private string GetZoneIdQuery()
        {
            return @"Select c.ZoneId
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.BillId=@billId AND
                    	c.ToDayJalali IS NULL";
        }
    }
}
