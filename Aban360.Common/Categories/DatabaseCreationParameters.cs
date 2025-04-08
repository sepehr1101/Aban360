namespace Aban360.Common.Categories
{
    public class DatabaseCreationParameters
    {
        public string MdfName { get; set; } = null!;
        public string LdfName { get; set; } = null!;
        public string StreamName { get; set; } = default!;
        public string MdfFileName { get; set; } = null!;
        public string LdfFileName { get; set; } = null!;
        public string StreamFileName { get; set; } = null!;
        public string MdfSize { get; set; } = null!;
        public string LdfSize { get; set; } = null!;
        public string MdfMaxSize { get; set; } = null!;
        public string LdfMaxSize { get; set; } = null!;
        public string MdfFileGrowth { get; set; } = null!;
        public string LdfFileGrowth { get; set; } = null!;
    }
}
