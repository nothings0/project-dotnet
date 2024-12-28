namespace SV21T1020546.Shop.Models
{
    public class OrderSearchInput : PaginationSearchInput
    {
        public int Status { get; set; } = 0;
        public string? OrderTime { get; set; }

    }
}