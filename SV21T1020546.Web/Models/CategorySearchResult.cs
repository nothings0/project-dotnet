using SV21T1020546.DomainModels;

namespace SV21T1020546.Web.Models
{
    public class CategorySearchResult : PaginationSearchResult
    {
        public required List<Category> Data { get; set; }
    }
}
