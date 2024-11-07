using Azure.Core;
using SV21T1020546.BusinessLayers;
using SV21T1020546.DataLayers.SQLServer;
using SV21T1020546.DomainModels;

namespace SV21T1020546.BusinessLayers
{
    public static class ProductDataService
    {
        private static readonly IProductDAL productDB;
        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }
        public static List<Product> ListProducts(string searchValue = "")
        {
            return productDB.List();
        }
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int suplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue);
            return productDB.List(page, pageSize, searchValue, categoryID, suplierID, minPrice, maxPrice);
        }
        public static Product? GetProduct(int productID)
        {
            return productDB.Get(productID);
        }
        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }
        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }
        public static bool DeleteProduct(int productID)
        {
            return productDB.Delete(productID);
        }
        public static bool InUsedProduct(int productID)
        {
            return productDB.InUsed(productID);
        }

        public static List<ProductPhoto> ListPhotos(int productID)
        {
            return (List<ProductPhoto>)productDB.ListPhotos(productID);
        }

        public static ProductPhoto? GetPhoto(long PhotoID)
        {
            return productDB.GetPhoto(PhotoID);
        }
        public static long AddPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }
        public static bool UpdatePhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }
        public static bool DeletePhoto(long photoID)
        {
            return productDB.DeletePhoto(photoID);
        }
        public static List<ProductAttribute> ListAttributes(int ProductID)
        {
            return (List<ProductAttribute>)productDB.ListAttributes(ProductID);
        }
        public static ProductAttribute? GetAttribute(long attributeID)
        {
            return productDB.GetAttribute(attributeID);
        }
        public static long AddAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }
        public static bool UpdateAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }
        public static bool DeleteAttribute(long attributeID)
        {
            return productDB.DeleteAttribute(attributeID);
        }

    }
}