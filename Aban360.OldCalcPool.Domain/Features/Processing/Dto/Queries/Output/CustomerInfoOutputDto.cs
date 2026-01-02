using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;

namespace Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output
{
    public record CustomerInfoOutputDto
    {
        public int ZoneId { get; set; }
        public int Radif { get; set; }
        public string BillId { get; set; } = default!;
        public int BranchType { get; set; }
        public int UsageId { get; set; }      
        public int DomesticUnit { get; set; }
        public int CommertialUnit { get; set; }        
        public int OtherUnit { get; set; }
        public int UnitAll
        {
            get
            {
                return (DomesticUnit + CommertialUnit + OtherUnit) > 1 ?
                       (DomesticUnit + CommertialUnit + OtherUnit) : 1;
            }
        }
        public int PureDomesticUnit
        {
            get { return (DomesticUnit - EmptyUnit) < 1 ? 1 : (DomesticUnit - EmptyUnit); }
        }
        public int EmptyUnit { get; set; }
        public string WaterInstallationDateJalali { get; set; } = default!;
        public string? SewageInstallationDateJalali { get; set; }
        public string WaterRegisterDate { get; set; } = default!;
        public string? SewageRegisterDate { get; set; }
        public int WaterCount { get; set; }
        public int SewageCalcState { get; set; }
        public int ContractualCapacity { get; set; }
        public int HouseholdNumber { get; set; }
        public string? HouseholdDate { get; set; }
        public string ReadingNumber { get; set; }=default!;
        public string? VillageId { get; set; }
        public bool IsSpecial { get; set; }
        public int MeterDiameterId { get; set; }
        public int HouseholdUnit { get; set; }
        public int VirtualCategoryId { get; set; }

        public CustomerInfoOutputDto()
        {
            
        }
        public CustomerInfoOutputDto(MeterImaginaryInputDto input)
        {
            ZoneId = input.CustomerInfo.ZoneId;
            Radif = input.CustomerInfo.Radif ?? 0;
            BranchType = input.CustomerInfo.BranchType;
            UsageId = input.CustomerInfo.UsageId;
            DomesticUnit = input.CustomerInfo.DomesticUnit;
            CommertialUnit = input.CustomerInfo.CommertialUnit;
            OtherUnit = input.CustomerInfo.OtherUnit;
            EmptyUnit = input.CustomerInfo.EmptyUnit ?? 0;
            WaterInstallationDateJalali = input.CustomerInfo.WaterInstallationDateJalali;
            SewageInstallationDateJalali = input.CustomerInfo.SewageInstallationDateJalali;
            //WaterCount = input.CustomerInfo.WaterCount;
            SewageCalcState = input.CustomerInfo.SewageCalcState ?? 0;
            ContractualCapacity = input.CustomerInfo.ContractualCapacity ?? 0;
            HouseholdNumber = input.CustomerInfo.HouseholdNumber ?? 0;
            HouseholdDate = input.CustomerInfo.HouseholdDate;
            ReadingNumber = input.CustomerInfo.ReadingNumber ?? string.Empty;
            VillageId = input.CustomerInfo.VillageId;
            IsSpecial = input.CustomerInfo.IsSpecial;
            BillId = input.MeterPreviousData.BillId ?? string.Empty;
            VirtualCategoryId = input.CustomerInfo.VirtualCategoryId ?? 0;
            WaterRegisterDate = input.CustomerInfo.WaterRegisterDate;
            SewageRegisterDate = input.CustomerInfo.SewageRegisterDate;
        }
    }
}
