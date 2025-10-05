using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class ChangeHistoryBase : AbstractBaseConnection
    {
        public ChangeHistoryBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(bool hasZone,string idField,string titleField)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @zoneIds" : string.Empty;

            return $@"use CustomerWarehouse
                    ;With FirstBillGroup as (
                    Select 
                    	c.{idField},
                    	c.BillId,
                    	c.{titleField},
                    	MAX(RegisterDayJalali)as RegisterDayJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Group By c.BillId,c.{titleField},{idField}
                    ),
                    SecondBillGroup as (
                    Select 
                    	c.{idField},
                    	c.BillId,
                    	c.{titleField},
                    	MAX(RegisterDayJalali)as RegisterDayJalali
                    From [CustomerWarehouse].dbo.Clients c
                    Group By c.BillId,c.{titleField},{idField}
                    )
                    Select 
                        c.CustomerNumber,
                        c.ReadingNumber,
                        c.BillId,
                        ss.RegisterDayJalali AS ChangeDateJalali,
                        ff.{titleField} AS From{titleField},
                        ss.{titleField} AS To{titleField},
                        ff.{titleField} AS FromState,
                        ss.{titleField} AS ToState,
                        c.ZoneTitle,
                        c.ZoneId,
                        c.UsageTitle,
                        TRIM(c.FirstName) AS FirstName,
                        TRIM(c.SureName) AS Surname,
                        TRIM(c.FirstName)+' '+TRIM(c.SureName) AS FullName,
                        TRIM(c.FatherName) AS FatherName,
                        TRIM(c.NationalId) AS NationalCode,
                        TRIM(c.PhoneNo) AS PhoneNumber,
                        TRIM(c.Address) AS Address,
                        TRIM(c.PostalCode) AS PostalCode,
                        c.ContractCapacity AS ContractualCapacity,
                        c.CommercialCount as CommercialUnit,
                        c.DomesticCount as DomesticUnit,
                        c.OtherCount as OtherUnit,
                        (c.CommercialCount+c.DomesticCount+c.OtherCount) AS TotalUnit,
                        c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                        c.BranchType AS UseStateTitle,
                        c.EmptyCount AS EmptyUnit,
					    DATEDIFF(DAY,[CustomerWarehouse].dbo.PersianToMiladi(ff.RegisterDayJalali),[CustomerWarehouse].dbo.PersianToMiladi(ss.RegisterDayJalali)) as Distance
                    From CustomerWarehouse.dbo.Clients c 
                    Join FirstBillGroup ff 
                    	On c.BillId=ff.BillId
                    Join SecondBillGroup ss
                    	On c.BillId=ss.BillId AND ff.{idField}<> ss.{idField}
                    Where
                    	c.ToDayJalali IS NULL AND
                    	ff.{idField} IN @fromFieldIds AND
                        ss.{idField} IN @toFieldIds AND
                        ss.RegisterDayJalali>ff.RegisterDayJalali AND
                        (@fromDate IS NULL OR
                        @toDate IS NULL OR
                        ss.RegisterDayJalali BETWEEN @fromDate AND @toDate) AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) 
                    	{zoneQuery} ";
        }

        internal string GetGroupedQuery()
        {
            return $@"";
        }
    }
}
