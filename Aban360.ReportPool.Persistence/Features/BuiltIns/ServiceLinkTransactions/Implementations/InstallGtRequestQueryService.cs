using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class InstallGtRequestQueryService : AbstractBaseConnection, IInstallGtRequestQueryService
    {
        public InstallGtRequestQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<InstallGtRequestHeaderOutputDto, InstallGtRequestDataOutputDto>> GetInfo(InstallGtRequestInputDto input)
        {
            string query = GetInstallGtRequestQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                distance = input.Distance
            };
            IEnumerable<InstallGtRequestDataOutputDto> installGtRequestData = await _sqlReportConnection.QueryAsync<InstallGtRequestDataOutputDto>(query, @params);
            InstallGtRequestHeaderOutputDto installGtRequestHeader = new InstallGtRequestHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = installGtRequestData is not null && installGtRequestData.Any() ? installGtRequestData.Count() : 0,
                RecordCount = installGtRequestData is not null && installGtRequestData.Any() ? installGtRequestData.Count() : 0,
                Title = ReportLiterals.WaterInstallGtRequest,
                SumDistance = installGtRequestData?.Sum(i => i.Distance) ?? 0,
            };

            var result = new ReportOutput<InstallGtRequestHeaderOutputDto, InstallGtRequestDataOutputDto>(
                ReportLiterals.WaterInstallGtRequest,
                installGtRequestHeader,
                installGtRequestData);

            return result;
        }

        private string GetInstallGtRequestQuery()
        {
            return @"USE CustomerWarehouse
                    SELECT
                    	WaterRequestDate as WaterRequestDateJalali,
                    	WaterInstallDate as WaterInstallDateJalali,
                    	CustomerNumber,
                    	ZoneId,
                    	ZoneTitle,
                    	BillId,
                    	TRIM(FirstName)+' '+TRIM(SureName) as FullName,
                    	ReadingNumber,
                    	UsageTitle,
                    	DATEDIFF(DAY,dbo.PersianToMiladi(WaterInstallDate),[CustomerWarehouse].dbo.PersianToMiladi(WaterRequestDate)) as Distance
                    FROM [CustomerWarehouse].dbo.Clients
                    WHERE 
                    	WaterRequestDate>WaterInstallDate AND 
                    	WaterRequestDate BETWEEN @fromDate AND @toDate AND
                    	WaterInstallDate>'1300/01/01' AND 
                    	DATEDIFF(DAY,dbo.PersianToMiladi(WaterInstallDate),dbo.PersianToMiladi(WaterRequestDate))>@distance AND
                    	ToDayJalali IS NULL
                    ORDER BY 
                    	WaterRequestDate DESC";
        }
    }
}
