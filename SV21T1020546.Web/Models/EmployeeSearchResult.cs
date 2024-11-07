using SV21T1020546.DomainModels;

namespace SV21T1020546.Web.Models
{
    public class EmployeeSearchResult : PaginationSearchResult
    {
        public required List<Employee> Data { get; set; }
    }
}
