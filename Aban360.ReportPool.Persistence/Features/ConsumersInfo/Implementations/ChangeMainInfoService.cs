using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Implementations
{
    internal sealed class ChangeMainInfoService : AbstractBaseConnection, IChangeMainInfoService
    {
        public ChangeMainInfoService(IConfiguration configuration)
            : base(configuration) { }

        public async Task<IEnumerable<ChangeMainInfoDto>> GetInfo(string billId)
        {
            string ChangeMainQuery = GetChangeMainSummayDtoQuery();
            //IEnumerable<ChangeMainInfoDto> result = await _sqlConnection.QueryAsync<ChangeMainInfoDto>(ChangeMainQuery, new { billId });
            //IEnumerable<ChangeMainInfoDto> result = GetFakeChangeMainInfo();
            IEnumerable<ChangeMainInfoDto> result =new List<ChangeMainInfoDto>();

            return result;
        }
        private string GetChangeMainSummayDtoQuery()
        {
            return @"";
        }
        private IEnumerable<ChangeMainInfoDto> GetFakeChangeMainInfo()
        {
            IEnumerable<ChangeMainInfoDto> changeMainInfo = new List<ChangeMainInfoDto>()
            {
                new ChangeMainInfoDto(){ChangeTypeTitle="تغیر نام",LastState="احمدد",CurrentState="احمد",ChangeDate="1401/01/01",SystemUserCode="15224"},
                new ChangeMainInfoDto(){ChangeTypeTitle="تغیر کاربری",LastState="مسکونی",CurrentState="تجاری",ChangeDate="1402/03/24",SystemUserCode="15224"},
                new ChangeMainInfoDto(){ChangeTypeTitle="تغیر آدرس",LastState="خیابان جی - خیابان شهید رجایی",CurrentState="خیابان احمد اباد - خیابان نشاط",ChangeDate="1404/01/29",SystemUserCode="15224"},
                new ChangeMainInfoDto(){ChangeTypeTitle="تغیر شماره تماس",LastState="09131002320",CurrentState="091322353536",ChangeDate="1403/08/10",SystemUserCode="15223"},
            };

            return changeMainInfo;
        }
    }
}
