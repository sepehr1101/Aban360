using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class HandoverDetailQueryService : AbstractBaseConnection, IHandoverDetailQueryService
    {
        public HandoverDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<HandoverHeaderOutputDto, HandoverDetailDataOutputDto>> Get(HandoverInputDto input)
        {
            string handoverDetailQuery = GetHandoverDetailQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                zoneIds = input.ZoneIds,
                branchTypeIds=input.BranchTypeIds,
            };
            IEnumerable<HandoverDetailDataOutputDto> data = await _sqlReportConnection.QueryAsync<HandoverDetailDataOutputDto>(handoverDetailQuery, @params);
            if (!data.Any())
            {
                throw new BaseException(ExceptionLiterals.NotFoundAnyData);
            }
            HandoverHeaderOutputDto header = new HandoverHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = (data is not null && data.Any()) ? data.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<HandoverHeaderOutputDto, HandoverDetailDataOutputDto>(ReportLiterals.HandoverDetail, header, data);
            return result;
        }

        private string GetHandoverDetailQuery()
        {
            return @"Select
                    	c.CustomerNumber,
                    	c.ReadingNumber,
                    	c.FirstName,
                    	c.SureName AS Surname,
                    	c.FirstName +' '+ c.SureName AS FullName,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.Address ,
                    	c.ZoneTitle,
                    	c.DomesticCount AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	c.BillId,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.WaterRequestDate AS MeterRequestDate,
                    	c.WaterInstallDate AS MeterInstallationDate,
                    	c.BranchType AS UseStateTitle
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	(@fromReadingNumber IS NULL OR
                    	 @toReadingNumber IS NULL OR
                    	 c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    	 c.ZoneId IN @zoneIds AND
						 c.UsageStateId IN @branchTypeIds";
        }
    }
}
