namespace Aban360.Common.Entities.ApiResponse
{
    public record ApiMeta
    {
        public string NextAction { get; }
        public DateTime ServerDateTime { get; }

        public ApiMeta() : this(string.Empty)
        {

        }
        public ApiMeta(string nextAction)
        {
            NextAction = nextAction;
            ServerDateTime = DateTime.Now;
        }
    }
}
