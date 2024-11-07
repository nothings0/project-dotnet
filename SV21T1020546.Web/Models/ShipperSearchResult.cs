using SV21T1020546.DomainModels;

namespace SV21T1020546.Web.Models
{
    public class ShipperSearchResult : PaginationSearchResult
    {
        public required List<Shipper> Data { get; set; }
    }
}
