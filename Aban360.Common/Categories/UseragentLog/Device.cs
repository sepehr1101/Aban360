namespace Aban360.Common.Categories.UseragentLog
{
    public record Device
    {
        public string? Title { get; set; }
        public bool IsBot { get; set; }
        public bool IsMobile { get; set; }
        public bool IsTablet { get; set; }
        public bool IsTouchEnabled { get; set; }
        public bool IsDesktop { get; set; }
        public bool IsBrwoser { get; set; }
        public string? Model { get; set; }
        public string? Brand { get; set; }
    }
}