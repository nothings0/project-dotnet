using SV21T1020546.DomainModels;

namespace SV21T1020546.Web.Models
{
    public class OrderSearchResult : PaginationSearchResult
    {
        public int Status { get; set; } = 0;
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public required List<Order> Data { get; set; }
    }
}