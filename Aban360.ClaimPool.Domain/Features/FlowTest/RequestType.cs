namespace Aban360.ClaimPool.Domain.Features.FlowTest
{
    public class RequestType
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<UserRequest> UserRequests { get; set; }
    }

    public class UserRequest
    {
        public int Id { get; set; }
        public string BillId { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //other prop

        public int RequestTypeId { get; set; }

        public virtual RequestType RequestType { get; set; }
        public virtual ICollection<RequestHistory> RequestHistories { get; set; }
    }

    public class RequestHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }//کسی که اکشن را برای این درخواست اعمال میکند
        public string? Date { get; set; }
        public bool IsChecked { get; set; } = false;

        public int ActionId { get; set; }
        //هنگامی که درخواست در جدول "درخواست های کاربر" ثبت میشه، بصورت خودکار با توجه  
        // به "اکشن" های انتخاب شده برای این نوع درخواست، 
        //رکورد هایی در این جدول با توجه به "شماره مرحله" تعین شده در جدول اکشن ، ایجاد میشه
        // و مسئول مربوطه با توجه به "آدرس" که در جدول اکشن تعریف شده
        //تغیرات لازم را برای این زیرشاخه از درخواست اعمال میکنه
        //در نهایت چنانچه همچیز مورد تاید بود ، رکورد را تیک میزند
        public virtual Action Action { get; set; }
    }

    public class RequestFlow
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int RequestTypeId { get; set; }

        public virtual RequestType RequestType { get; set; }
        public ICollection<Action> Actions { get; set; }
    }
    public class Action
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int StepNumber { get; set; }
        public bool IsChecked { get; set; }
        public string URL {  get; set; }

        public int RequestFlowId { get; set; }
        public virtual RequestFlow RequestFlow { get; set; }
    }
}
