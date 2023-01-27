namespace POS.Infrastructure.Commons.Bases.Request
{
    public class BasePaginationRequest
    {
        public int NumPage { get; set; } = 1;
        public int NumRecordsPage { get; set; } = 10;
        public string Order { get; set; } = "asc";
        public string? Sort { get; set; } = null;
        private readonly int NumMaxRecordsPage = 50;
        public int Records
        {
            get => NumRecordsPage;
            set
            {
                NumRecordsPage = value > NumMaxRecordsPage ? NumRecordsPage : value;
            }
        }


    }
}
