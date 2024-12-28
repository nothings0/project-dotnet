using SV21T1020546.DomainModels;

namespace SV21T1020546.Shop.Models
{
    public class ProductDetailModel
    {
        public Product ProductData { get; set; }
        public List<Product>? RelatedProducts { get; set; }
    }
}
