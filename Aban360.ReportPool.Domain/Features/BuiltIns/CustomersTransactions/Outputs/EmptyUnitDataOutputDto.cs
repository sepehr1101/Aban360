﻿namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record EmptyUnitDataOutputDto
    {
        public int CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string UsageTitle { get; set; }
        public string MeterDiameterTitle { get; set; }
        public string EventDateJalali { get; set; }
        public string Address { get; set; }
        public int DomesticUnit { get; set; }
        public int CommercialUnit { get; set; }
        public int OtherUnit { get; set; }
        public string BillId { get; set; }
        public string UseStateTitle { get; set; }
        public int EmptyUnit { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; }
        public string NationalCode { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FatherName { get; set; }
    }
}
