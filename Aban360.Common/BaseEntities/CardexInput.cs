namespace Aban360.Common.BaseEntities
{
    public record CardexInput
    {
        public string Input { get; set; } = default!;
        public string? FromDateJalali { get; set; }
        public CardexInput(string input,string? fromDateJalali)
        {
            Input = input;
            FromDateJalali = fromDateJalali;
        }
        public CardexInput()
        {

        }
    }
}
