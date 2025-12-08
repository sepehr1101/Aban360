namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingDetailCreateDuplicateDto
    {
        public int Id { get; set; }
        public short CurrentCounterStateCode { get; set; }
        public string CurrentDateJalali { get; set; }
        public int CurrentNumber { get; set; }
        public Guid InsertByUserId { get; set; }
        public DateTime InsertDateTime { get; set; }

        public double SumItems { get; set; }
        public double SumItemsBeforeDiscount { get; set; }
        public double DiscountSum { get; set; }
        public double Consumption { get; set; }
        public double  MonthlyConsumption { get; set; }
        public MeterReadingDetailCreateDuplicateDto(int id, short currentCounterStateCode, string currentDateJalali, int currentNumber, Guid insertByUserId, DateTime insertDateTime, double sumItems, double sumItemsBeforeDiscount, double discountSum, double consumption, double monthlyConsumption)
        {
            Id = id;
            CurrentCounterStateCode = currentCounterStateCode;
            CurrentDateJalali = currentDateJalali;
            CurrentNumber = currentNumber;
            InsertByUserId = insertByUserId;
            InsertDateTime = insertDateTime;


            SumItems = sumItems;    
            SumItemsBeforeDiscount = sumItemsBeforeDiscount;
            DiscountSum = discountSum;
            Consumption = consumption;
            MonthlyConsumption = monthlyConsumption;
        }
    }
}

