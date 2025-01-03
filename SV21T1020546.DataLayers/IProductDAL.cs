﻿using Dapper;
using SV21T1020546.DomainModels;

namespace SV21T1020546.DataLayers.SQLServer
{
    public interface IProductDAL
    {
        List<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0,
                            int suplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);

        int Count(string searchValue = "", int categoryID = 0,
              int suplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);

        Product? Get(int productID);
        int Add(Product data);
        bool Update(Product data);
        bool Delete(int productID);
        bool InUsed(int productID);

        IList<ProductPhoto> ListPhotos(int productID);

        ProductPhoto? GetPhoto(long photoID);

        int AddPhoto(ProductPhoto data);
        bool UpdatePhoto(ProductPhoto data);
        bool DeletePhoto(long photoID);

        IList<ProductAttribute> ListAttributes(int productID);

        ProductAttribute? GetAttribute(long attributeID);

        int AddAttribute(ProductAttribute data);
        bool UpdateAttribute(ProductAttribute data);
        bool DeleteAttribute(long attributeID);
        List<Product> GetProducts(int limit, int offset);

    }
}
