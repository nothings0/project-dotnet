using SV21T1020546.DomainModels;

namespace SV21T1020546.Web.Models
{
    public class CustomerSearchResult : PaginationSearchResult
    {
        public required List<Customer> Data { get; set; }
    }
}
