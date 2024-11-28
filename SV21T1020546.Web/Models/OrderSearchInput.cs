namespace SV21T1020546.Web.Models
{
    public class OrderSearchInput : PaginationSearchInput
    {
        public int Status { get; set; } = 0;
        public string? OrderTime { get; set; }

    }
}