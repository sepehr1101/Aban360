using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ServiceLinkDebtorCustomersQueryService : AbstractBaseConnection, IServiceLinkDebtorCustomersQueryService
    {
        public ServiceLinkDebtorCustomersQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ServiceLinkDebtorCustomersHeaderOutputDto, ServiceLinkDebtorCustomersDataOutputDto>> GetInfo(ServiceLinkDebtorCustomersInputDto input)
        {
            string debtorCustomersQueryString = GetServiceLinkdebtorCustomersQuery();
            var @params = new
            {
                fromAmount = input.FromAmount < 10000 ? 10000 : input.FromAmount,
                toAmount = input.ToAmount,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<ServiceLinkDebtorCustomersDataOutputDto> debtorCustomersData = await _sqlReportConnection.QueryAsync<ServiceLinkDebtorCustomersDataOutputDto>(debtorCustomersQueryString, @params);
            ServiceLinkDebtorCustomersHeaderOutputDto debtorCustomersHeader = new ServiceLinkDebtorCustomersHeaderOutputDto()
            {
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (debtorCustomersData is not null && debtorCustomersData.Any()) ? debtorCustomersData.Count() : 0,
            };
            if (debtorCustomersData is not null && debtorCustomersData.Any())
            {
                debtorCustomersHeader.SumCreditAmount = debtorCustomersData.Sum(x => x.CreditorAmount);                   
                debtorCustomersHeader.SumInstallmentDebtAmout = debtorCustomersData.Sum(x => x.InstallmentDebtAmout);
                debtorCustomersHeader.SumPrincipalDebt = debtorCustomersData.Sum(x => x.PrincipalDebt);
                debtorCustomersHeader.SumTotalDebt = debtorCustomersData.Sum(x => x.TotalDebt);
            }

            var result = new ReportOutput<ServiceLinkDebtorCustomersHeaderOutputDto, ServiceLinkDebtorCustomersDataOutputDto>(ReportLiterals.ServiceLinkDebtorCustomers, debtorCustomersHeader, debtorCustomersData);

            return result;
        }

        private string GetServiceLinkdebtorCustomersQuery()
        {
            return @"Select
                    	v.ZoneTitle,
                    	v.Radif AS CustomerNumber,
                    	v.BillId,
                    	v.FullName,
                    	v.Mobile,
                    	v.BedehiGhesti AS InstallmentDebtAmount,
                    	v.Bestankari AS CreditorAmount,
                    	v.BedehiAsli AS PrincipalDebt,
                    	v.BedehiAll AS TotalDebt
                    From [CustomerWarehouse].dbo.VosoolEnsheabAlert v
                    Where	
                    	v.BedehiAll BETWEEN @fromAmount AND @toAmount AND
                    	v.ZoneId IN @zoneIds";
        }


    }
}
