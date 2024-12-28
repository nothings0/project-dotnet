using SV21T1020546.DomainModels;

namespace SV21T1020546.Shop.Models
{
    public class HomeModel
    {
        public List<Product> FeaturedProducts { get; set; }
        public List<Product> LatestProducts { get; set; }
    }
}
